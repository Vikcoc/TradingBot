﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML;

namespace AiModel
{
    public partial class Test3
    {
        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(@"DayOfWeek", @"DayOfWeek", outputKind: OneHotEncodingEstimator.OutputKind.Indicator)      
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"CurrentValue", @"CurrentValue"),new InputOutputColumnPair(@"BestBid", @"BestBid"),new InputOutputColumnPair(@"BestAsk", @"BestAsk"),new InputOutputColumnPair(@"LowOfTheDay", @"LowOfTheDay"),new InputOutputColumnPair(@"HighOfTheDay", @"HighOfTheDay"),new InputOutputColumnPair(@"DayOfMonth", @"DayOfMonth"),new InputOutputColumnPair(@"Hour", @"Hour"),new InputOutputColumnPair(@"Minute", @"Minute"),new InputOutputColumnPair(@"Second", @"Second"),new InputOutputColumnPair(@"Millisecond", @"Millisecond"),new InputOutputColumnPair(@"Past50s", @"Past50s"),new InputOutputColumnPair(@"Past30s", @"Past30s"),new InputOutputColumnPair(@"Past10s", @"Past10s")}))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"DayOfWeek",@"CurrentValue",@"BestBid",@"BestAsk",@"LowOfTheDay",@"HighOfTheDay",@"DayOfMonth",@"Hour",@"Minute",@"Second",@"Millisecond",@"Past50s",@"Past30s",@"Past10s"}))      
                                    .Append(mlContext.Regression.Trainers.FastTree(new FastTreeRegressionTrainer.Options(){NumberOfLeaves=599,MinimumExampleCountPerLeaf=2,NumberOfTrees=681,MaximumBinCountPerFeature=74,FeatureFraction=0.657088435970543,LearningRate=0.999999776672986,LabelColumnName=@"Predicted50sAhead",FeatureColumnName=@"Features"}));

            return pipeline;
        }
    }
}
