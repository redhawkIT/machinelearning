﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.ML.Data;
using Microsoft.ML.RunTests;
using Microsoft.ML.Transforms.TimeSeries;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.ML.Tests
{
    public class TimeSeriesEstimatorTests : TestDataPipeBase
    {
        private const int inputSize = 150528;

        private class Data
        {
            public float Value;

            public Data(float value)
            {
                Value = value;
            }
        }

        private class TestDataXY
        {
            [VectorType(inputSize)]
            public float[] A;
        }
        private class TestDataDifferntType
        {
            [VectorType(inputSize)]
            public string[] data_0;
        }

        public TimeSeriesEstimatorTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        void TestSsaChangePointEstimator()
        {
            int Confidence = 95;
            int ChangeHistorySize = 10;
            int SeasonalitySize = 10;
            int NumberOfSeasonsInTraining = 5;
            int MaxTrainingSize = NumberOfSeasonsInTraining * SeasonalitySize;

            List<Data> data = new List<Data>();
            var dataView = ML.Data.LoadFromEnumerable(data);

            for (int j = 0; j < NumberOfSeasonsInTraining; j++)
                for (int i = 0; i < SeasonalitySize; i++)
                    data.Add(new Data(i));

            for (int i = 0; i < ChangeHistorySize; i++)
                data.Add(new Data(i * 100));

            var pipe = new SsaChangePointEstimator(Env, "Change", 
                Confidence, ChangeHistorySize, MaxTrainingSize, SeasonalitySize, "Value");

            var xyData = new List<TestDataXY> { new TestDataXY() { A = new float[inputSize] } };
            var stringData = new List<TestDataDifferntType> { new TestDataDifferntType() { data_0 = new string[inputSize] } };

            var invalidDataWrongNames = ML.Data.LoadFromEnumerable(xyData);
            var invalidDataWrongTypes = ML.Data.LoadFromEnumerable(stringData);

            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongTypes);
            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongNames);

            Done();
        }

        [Fact]
        void TestSsaForecastingEstimator()
        {
            const int ChangeHistorySize = 10;
            const int SeasonalitySize = 10;
            const int NumberOfSeasonsInTraining = 5;

            List<Data> data = new List<Data>();

            var ml = new MLContext(seed: 1);
            var dataView = ml.Data.LoadFromEnumerable(data);

            for (int j = 0; j < NumberOfSeasonsInTraining; j++)
                for (int i = 0; i < SeasonalitySize; i++)
                    data.Add(new Data(i));

            for (int i = 0; i < ChangeHistorySize; i++)
                data.Add(new Data(i * 100));

            // Train
            var pipe = new SsaForecastingEstimator(Env, "Forecast", "Value", 10, 11, 22, 4,
                    forcastingConfidentLowerBoundColumnName: "ConfidenceLowerBound",
                    forcastingConfidentUpperBoundColumnName: "ConfidenceUpperBound");

            var xyData = new List<TestDataXY> { new TestDataXY() { A = new float[inputSize] } };
            var stringData = new List<TestDataDifferntType> { new TestDataDifferntType() { data_0 = new string[inputSize] } };

            var invalidDataWrongNames = ML.Data.LoadFromEnumerable(xyData);
            var invalidDataWrongTypes = ML.Data.LoadFromEnumerable(stringData);

            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongTypes);
            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongNames);

            Done();
        }

        [Fact]
        void TestSsaSpikeEstimator()
        {
            int Confidence = 95;
            int PValueHistorySize = 10;
            int SeasonalitySize = 10;
            int NumberOfSeasonsInTraining = 5;
            int MaxTrainingSize = NumberOfSeasonsInTraining * SeasonalitySize;

            List<Data> data = new List<Data>();
            var dataView = ML.Data.LoadFromEnumerable(data);

            for (int j = 0; j < NumberOfSeasonsInTraining; j++)
                for (int i = 0; i < SeasonalitySize; i++)
                    data.Add(new Data(i));

            for (int i = 0; i < PValueHistorySize; i++)
                data.Add(new Data(i * 100));

            var pipe = new SsaSpikeEstimator(Env, "Change",
                Confidence, PValueHistorySize, MaxTrainingSize, SeasonalitySize, "Value");

            var xyData = new List<TestDataXY> { new TestDataXY() { A = new float[inputSize] } };
            var stringData = new List<TestDataDifferntType> { new TestDataDifferntType() { data_0 = new string[inputSize] } };

            var invalidDataWrongNames = ML.Data.LoadFromEnumerable(xyData);
            var invalidDataWrongTypes = ML.Data.LoadFromEnumerable(stringData);

            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongTypes);
            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongNames);

            Done();
        }

        [Fact]
        void TestIidChangePointEstimator()
        {
            int Confidence = 95;
            int ChangeHistorySize = 10;

            List<Data> data = new List<Data>();
            var dataView = ML.Data.LoadFromEnumerable(data);

            for (int i = 0; i < ChangeHistorySize; i++)
                data.Add(new Data(i * 100));

            var pipe = new IidChangePointEstimator(Env,
                "Change", Confidence, ChangeHistorySize, "Value");

            var xyData = new List<TestDataXY> { new TestDataXY() { A = new float[inputSize] } };
            var stringData = new List<TestDataDifferntType> { new TestDataDifferntType() { data_0 = new string[inputSize] } };

            var invalidDataWrongNames = ML.Data.LoadFromEnumerable(xyData);
            var invalidDataWrongTypes = ML.Data.LoadFromEnumerable(stringData);

            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongTypes);
            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongNames);

            Done();
        }

        [Fact]
        void TestIidSpikeEstimator()
        {
            int Confidence = 95;
            int PValueHistorySize = 10;

            List<Data> data = new List<Data>();
            var dataView = ML.Data.LoadFromEnumerable(data);

            for (int i = 0; i < PValueHistorySize; i++)
                data.Add(new Data(i * 100));

            var pipe = new IidSpikeEstimator(Env,
                "Change", Confidence, PValueHistorySize, "Value");

            var xyData = new List<TestDataXY> { new TestDataXY() { A = new float[inputSize] } };
            var stringData = new List<TestDataDifferntType> { new TestDataDifferntType() { data_0 = new string[inputSize] } };

            var invalidDataWrongNames = ML.Data.LoadFromEnumerable(xyData);
            var invalidDataWrongTypes = ML.Data.LoadFromEnumerable(stringData);

            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongTypes);
            TestEstimatorCore(pipe, dataView, invalidInput: invalidDataWrongNames);

            Done();
        }
    }
}
