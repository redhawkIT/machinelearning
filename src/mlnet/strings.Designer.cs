﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.ML.CLI {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.ML.CLI.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Best pipeline.
        /// </summary>
        internal static string BestPipeline {
            get {
                return ResourceManager.GetString("BestPipeline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Time expired before creating a model. Try increasing the exploration time from {0} seconds to a longer duration using the --max-exploration-time option. Learn about recommended training time at https://aka.ms/cli-trainingtime.
        /// </summary>
        internal static string CouldNotFinshOnTime {
            get {
                return ResourceManager.GetString("CouldNotFinshOnTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creating Data loader ....
        /// </summary>
        internal static string CreateDataLoader {
            get {
                return ResourceManager.GetString("CreateDataLoader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error occured while retreiving best pipeline..
        /// </summary>
        internal static string ErrorBestPipeline {
            get {
                return ResourceManager.GetString("ErrorBestPipeline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception occured while generating the project..
        /// </summary>
        internal static string ErrorGeneratingProject {
            get {
                return ResourceManager.GetString("ErrorGeneratingProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception occured while saving the model.
        /// </summary>
        internal static string ErrorSavingModel {
            get {
                return ResourceManager.GetString("ErrorSavingModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exiting ....
        /// </summary>
        internal static string Exiting {
            get {
                return ResourceManager.GetString("Exiting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exploring multiple ML algorithms and settings to find you the best model for ML task.
        /// </summary>
        internal static string ExplorePipeline {
            get {
                return ResourceManager.GetString("ExplorePipeline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception occured while exploring pipelines.
        /// </summary>
        internal static string ExplorePipelineException {
            get {
                return ResourceManager.GetString("ExplorePipelineException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to For further learning check.
        /// </summary>
        internal static string FurtherLearning {
            get {
                return ResourceManager.GetString("FurtherLearning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generated log file .
        /// </summary>
        internal static string GenerateLogFile {
            get {
                return ResourceManager.GetString("GenerateLogFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generated C# code for model consumption.
        /// </summary>
        internal static string GenerateModelConsumption {
            get {
                return ResourceManager.GetString("GenerateModelConsumption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generated C# code for model training.
        /// </summary>
        internal static string GenerateModelTraining {
            get {
                return ResourceManager.GetString("GenerateModelTraining", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generating a console project for the best pipeline at location .
        /// </summary>
        internal static string GenerateProject {
            get {
                return ResourceManager.GetString("GenerateProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An Error occured during inferring columns.
        /// </summary>
        internal static string InferColumnError {
            get {
                return ResourceManager.GetString("InferColumnError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inferring Columns ....
        /// </summary>
        internal static string InferColumns {
            get {
                return ResourceManager.GetString("InferColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://aka.ms/mlnet-cli.
        /// </summary>
        internal static string LearningHttpLink {
            get {
                return ResourceManager.GetString("LearningHttpLink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading data ....
        /// </summary>
        internal static string LoadData {
            get {
                return ResourceManager.GetString("LoadData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please see the log file for more info..
        /// </summary>
        internal static string LookIntoLogFile {
            get {
                return ResourceManager.GetString("LookIntoLogFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Metrics for Binary Classification models.
        /// </summary>
        internal static string MetricsForBinaryClassModels {
            get {
                return ResourceManager.GetString("MetricsForBinaryClassModels", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Metrics for multi-class models.
        /// </summary>
        internal static string MetricsForMulticlassModels {
            get {
                return ResourceManager.GetString("MetricsForMulticlassModels", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Metrics for regression models.
        /// </summary>
        internal static string MetricsForRegressionModels {
            get {
                return ResourceManager.GetString("MetricsForRegressionModels", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Retrieving best pipeline ....
        /// </summary>
        internal static string RetrieveBestPipeline {
            get {
                return ResourceManager.GetString("RetrieveBestPipeline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generated trained model for consumption.
        /// </summary>
        internal static string SavingBestModel {
            get {
                return ResourceManager.GetString("SavingBestModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Check out log file for more information.
        /// </summary>
        internal static string SeeLogFileForMoreInfo {
            get {
                return ResourceManager.GetString("SeeLogFileForMoreInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported ml-task.
        /// </summary>
        internal static string UnsupportedMlTask {
            get {
                return ResourceManager.GetString("UnsupportedMlTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for the first iteration to complete ....
        /// </summary>
        internal static string WaitingForFirstIteration {
            get {
                return ResourceManager.GetString("WaitingForFirstIteration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for the last iteration to complete ....
        /// </summary>
        internal static string WaitingForLastIteration {
            get {
                return ResourceManager.GetString("WaitingForLastIteration", resourceCulture);
            }
        }
    }
}
