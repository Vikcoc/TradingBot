﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace AiModel
{
    public partial class MinMaxNorm
    {
        /// <summary>
        /// model input class for MinMaxNorm.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"col0")]
            public float Col0 { get; set; }

            [ColumnName(@"col1")]
            public float Col1 { get; set; }

            [ColumnName(@"col2")]
            public float Col2 { get; set; }

            [ColumnName(@"col3")]
            public float Col3 { get; set; }

            [ColumnName(@"col4")]
            public float Col4 { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for MinMaxNorm.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"col0")]
            public float Col0 { get; set; }

            [ColumnName(@"col1")]
            public float Col1 { get; set; }

            [ColumnName(@"col2")]
            public float Col2 { get; set; }

            [ColumnName(@"col3")]
            public float Col3 { get; set; }

            [ColumnName(@"col4")]
            public float Col4 { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"Score")]
            public float Score { get; set; }

        }

        #endregion

        public static float Normalize(float value) => (float)((value - 1407.74) / (1841.27 - 1407.74));

        private static string MLNetModelPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MinMaxNorm.zip");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }
}
