// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.CommandLine;
using Microsoft.ML.Data;
using Microsoft.ML.Internal.Internallearn;
using Microsoft.ML.Internal.Utilities;
using Microsoft.ML.Runtime;
using Microsoft.ML.Trainers.Ensemble.SubsetSelector;

namespace Microsoft.ML.Trainers.Ensemble
{
    using Stopwatch = System.Diagnostics.Stopwatch;

    internal abstract class EnsembleTrainerBase<TOutput, TSelector, TCombiner> : ITrainer<IPredictor>
         where TSelector : class, ISubModelSelector<TOutput>
         where TCombiner : class, IOutputCombiner<TOutput>
    {
        public abstract class ArgumentsBase : TrainerInputBaseWithLabel
        {
#pragma warning disable CS0649 // These are set via reflection.
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Number of models per batch. If not specified, will default to 50 if there is only one base predictor, " +
                "or the number of base predictors otherwise.", ShortName = "nm", SortOrder = 3)]
            [TGUI(Label = "Number of Models per batch")]
            public int? NumModels;

            [Argument(ArgumentType.AtMostOnce, HelpText = "Batch size", ShortName = "bs", SortOrder = 107)]
            [TGUI(Label = "Batch Size",
                Description =
                "Number of instances to be loaded in memory to create an ensemble out of it. All the instances will be loaded if the value is -1.")]
            public int BatchSize = -1;

            [Argument(ArgumentType.Multiple, HelpText = "Sampling Type", ShortName = "st", SortOrder = 2)]
            [TGUI(Label = "Sampling Type", Description = "Subset Selection Algorithm to induce the base learner.Sub-settings can be used to select the features")]
            public ISupportSubsetSelectorFactory SamplingType = new BootstrapSelector.Arguments();

            [Argument(ArgumentType.AtMostOnce, HelpText = "All the base learners will run asynchronously if the value is true", ShortName = "tp", SortOrder = 106)]
            [TGUI(Label = "Train parallel", Description = "All the base learners will run asynchronously if the value is true")]
            public bool TrainParallel;

            [Argument(ArgumentType.AtMostOnce,
                HelpText = "True, if metrics for each model need to be evaluated and shown in comparison table. This is done by using validation set if available or the training set",
                ShortName = "sm", SortOrder = 108)]
            [TGUI(Label = "Show Sub-Model Metrics")]
            public bool ShowMetrics;

            internal abstract IComponentFactory<ITrainerEstimator<ISingleFeaturePredictionTransformer<IPredictorProducing<TOutput>>, IPredictorProducing<TOutput>>>[] GetPredictorFactories();
#pragma warning restore CS0649
        }

        private const int DefaultNumModels = 50;
        /// <summary> Command-line arguments </summary>
        private protected readonly ArgumentsBase Args;
        private protected readonly int NumModels;
        private protected readonly IHost Host;

        /// <summary> Ensemble members </summary>
        private protected readonly ITrainerEstimator<ISingleFeaturePredictionTransformer<IPredictorProducing<TOutput>>, IPredictorProducing<TOutput>>[] Trainers;

        private readonly ISubsetSelector _subsetSelector;
        private protected ISubModelSelector<TOutput> SubModelSelector;
        private protected IOutputCombiner<TOutput> Combiner;

        public TrainerInfo Info { get; }

        PredictionKind ITrainer.PredictionKind => PredictionKind;
        private protected abstract PredictionKind PredictionKind { get; }

        private protected EnsembleTrainerBase(ArgumentsBase args, IHostEnvironment env, string name)
        {
            Contracts.CheckValue(env, nameof(env));
            Host = env.Register(name);

            Args = args;

            using (var ch = Host.Start("Init"))
            {
                var predictorFactories = Args.GetPredictorFactories();
                ch.CheckUserArg(Utils.Size(predictorFactories) > 0, nameof(EnsembleTrainer.Arguments.BasePredictors), "This should have at-least one value");

                NumModels = Args.NumModels ??
                    (predictorFactories.Length == 1 ? DefaultNumModels : predictorFactories.Length);

                ch.CheckUserArg(NumModels > 0, nameof(Args.NumModels), "Must be positive, or null to indicate numModels is the number of base predictors");

                if (Utils.Size(predictorFactories) > NumModels)
                    ch.Warning("The base predictor count is greater than models count. Some of the base predictors will be ignored.");

                _subsetSelector = Args.SamplingType.CreateComponent(Host);

                Trainers = new ITrainerEstimator<ISingleFeaturePredictionTransformer<IPredictorProducing<TOutput>>, IPredictorProducing<TOutput>>[NumModels];
                for (int i = 0; i < Trainers.Length; i++)
                    Trainers[i] = predictorFactories[i % predictorFactories.Length].CreateComponent(Host);
                // We infer normalization and calibration preferences from the trainers. However, even if the internal trainers
                // don't need caching we are performing multiple passes over the data, so it is probably appropriate to always cache.
                Info = new TrainerInfo(
                    normalization: Trainers.Any(t => t.Info.NeedNormalization),
                    calibration: Trainers.Any(t => t.Info.NeedCalibration));
            }
        }

        IPredictor ITrainer<IPredictor>.Train(TrainContext context)
        {
            Host.CheckValue(context, nameof(context));

            using (var ch = Host.Start("Training"))
            {
                return TrainCore(ch, context.TrainingSet);
            }
        }

        IPredictor ITrainer.Train(TrainContext context)
            => ((ITrainer<IPredictor>)this).Train(context);

        private IPredictor TrainCore(IChannel ch, RoleMappedData data)
        {
            Host.AssertValue(ch);
            ch.AssertValue(data);

            // 1. Subset Selection
            var stackingTrainer = Combiner as IStackingTrainer<TOutput>;

            //REVIEW: Implement stacking for Batch mode.
            ch.CheckUserArg(stackingTrainer == null || Args.BatchSize <= 0, nameof(Args.BatchSize), "Stacking works only with Non-batch mode");

            var validationDataSetProportion = SubModelSelector.ValidationDatasetProportion;
            if (stackingTrainer != null)
                validationDataSetProportion = Math.Max(validationDataSetProportion, stackingTrainer.ValidationDatasetProportion);

            var needMetrics = Args.ShowMetrics || Combiner is IWeightedAverager;
            var models = new List<FeatureSubsetModel<TOutput>>();

            _subsetSelector.Initialize(data, NumModels, Args.BatchSize, validationDataSetProportion);
            int batchNumber = 1;
            foreach (var batch in _subsetSelector.GetBatches(Host.Rand))
            {
                // 2. Core train
                ch.Info("Training {0} learners for the batch {1}", Trainers.Length, batchNumber++);
                var batchModels = new FeatureSubsetModel<TOutput>[Trainers.Length];

                Parallel.ForEach(_subsetSelector.GetSubsets(batch, Host.Rand),
                    new ParallelOptions() { MaxDegreeOfParallelism = Args.TrainParallel ? -1 : 1 },
                    (subset, state, index) =>
                    {
                        ch.Info("Beginning training model {0} of {1}", index + 1, Trainers.Length);
                        Stopwatch sw = Stopwatch.StartNew();
                        try
                        {
                            if (EnsureMinimumFeaturesSelected(subset))
                            {
                                // REVIEW: How to pass the role mappings to the trainer?
                                var model = new FeatureSubsetModel<TOutput>(
                                    Trainers[(int)index].Fit(subset.Data.Data).Model,
                                    subset.SelectedFeatures,
                                    null);
                                SubModelSelector.CalculateMetrics(model, _subsetSelector, subset, batch, needMetrics);
                                batchModels[(int)index] = model;
                            }
                        }
                        catch (Exception ex)
                        {
                            ch.Assert(batchModels[(int)index] == null);
                            ch.Warning(ex.Sensitivity(), "Trainer {0} of {1} was not learned properly due to the exception '{2}' and will not be added to models.",
                                index + 1, Trainers.Length, ex.Message);
                        }
                        ch.Info("Trainer {0} of {1} finished in {2}", index + 1, Trainers.Length, sw.Elapsed);
                    });

                var modelsList = batchModels.Where(m => m != null).ToList();
                if (Args.ShowMetrics)
                    PrintMetrics(ch, modelsList);

                modelsList = SubModelSelector.Prune(modelsList).ToList();

                if (stackingTrainer != null)
                    stackingTrainer.Train(modelsList, _subsetSelector.GetTestData(null, batch), Host);

                models.AddRange(modelsList);
                int modelSize = Utils.Size(models);
                if (modelSize < Utils.Size(Trainers))
                    ch.Warning("{0} of {1} trainings failed.", Utils.Size(Trainers) - modelSize, Utils.Size(Trainers));
                ch.Check(modelSize > 0, "Ensemble training resulted in no valid models.");
            }
            return CreatePredictor(models);
        }

        private protected abstract IPredictor CreatePredictor(List<FeatureSubsetModel<TOutput>> models);

        private bool EnsureMinimumFeaturesSelected(Subset subset)
        {
            if (subset.SelectedFeatures == null)
                return true;
            for (int i = 0; i < subset.SelectedFeatures.Count; i++)
            {
                if (subset.SelectedFeatures[i])
                    return true;
            }

            return false;
        }

        private protected virtual void PrintMetrics(IChannel ch, List<FeatureSubsetModel<TOutput>> models)
        {
            // REVIEW: The formatting of this method is bizarre and seemingly not even self-consistent
            // w.r.t. its usage of |. Is this intentional?
            if (models.Count == 0 || models[0].Metrics == null)
                return;

            ch.Info("{0}| Name of Model |", string.Join("", models[0].Metrics.Select(m => string.Format("| {0} |", m.Key))));

            foreach (var model in models)
                ch.Info("{0}{1}", string.Join("", model.Metrics.Select(m => string.Format("| {0} |", m.Value))), model.Predictor.GetType().Name);
        }

        private protected static FeatureSubsetModel<TOutput>[] CreateModels<T>(List<FeatureSubsetModel<TOutput>> models) where T : IPredictorProducing<TOutput>
        {
            var subsetModels = new FeatureSubsetModel<TOutput>[models.Count];
            for (int i = 0; i < models.Count; i++)
            {
                subsetModels[i] = new FeatureSubsetModel<TOutput>(
                    (T)models[i].Predictor,
                    models[i].SelectedFeatures,
                    models[i].Metrics);
            }
            return subsetModels;
        }
    }
}
