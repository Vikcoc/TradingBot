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
    public partial class DailyTradeModel
    {
        /// <summary>
        /// model input class for DailyTradeModel.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"CurrentValue")]
            public float CurrentValue { get; set; }

            [ColumnName(@"BestBid")]
            public float BestBid { get; set; }

            [ColumnName(@"BestAsk")]
            public float BestAsk { get; set; }

            [ColumnName(@"Hour")]
            public float Hour { get; set; }

            [ColumnName(@"Minute")]
            public float Minute { get; set; }

            [ColumnName(@"Second")]
            public float Second { get; set; }

            [ColumnName(@"Millisecond")]
            public float Millisecond { get; set; }

            [ColumnName(@"Past50s")]
            public float Past50s { get; set; }

            [ColumnName(@"Past30s")]
            public float Past30s { get; set; }

            [ColumnName(@"Past10s")]
            public float Past10s { get; set; }

            [ColumnName(@"Predicted50sAhead")]
            public float Predicted50sAhead { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for DailyTradeModel.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"CurrentValue")]
            public float CurrentValue { get; set; }

            [ColumnName(@"BestBid")]
            public float BestBid { get; set; }

            [ColumnName(@"BestAsk")]
            public float BestAsk { get; set; }

            [ColumnName(@"Hour")]
            public float Hour { get; set; }

            [ColumnName(@"Minute")]
            public float Minute { get; set; }

            [ColumnName(@"Second")]
            public float Second { get; set; }

            [ColumnName(@"Millisecond")]
            public float Millisecond { get; set; }

            [ColumnName(@"Past50s")]
            public float Past50s { get; set; }

            [ColumnName(@"Past30s")]
            public float Past30s { get; set; }

            [ColumnName(@"Past10s")]
            public float Past10s { get; set; }

            [ColumnName(@"Predicted50sAhead")]
            public float Predicted50sAhead { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"Score")]
            public float Score { get; set; }

        }

        #endregion

        private static string MLNetModelPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DailyTradeModel.zip");

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
