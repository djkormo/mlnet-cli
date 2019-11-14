using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
// for test , train split
using static Microsoft.ML.DataOperationsCatalog;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.Text;

namespace SentimentAnalysis
{
 
    class Program
    {        
        /* dataset from 
        https://archive.ics.uci.edu/ml/machine-learning-databases/00331/sentiment%20labelled%20sentences.zip
         */
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "data", "yelp_labelled.txt");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "data", "Model.zip");
        static void Main(string[] args)
        {
            
            MLContext mlContext = new MLContext();
            // load data
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>
                (_dataPath, hasHeader: true, separatorChar: '\t');
            // splitting data into  train and test sets
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            IDataView trainingDataView = splitDataView.TrainSet;
            IDataView testingDataView = splitDataView.TestSet;
            // training model on train dataset
            var estimator = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ModelInput.SentimentText))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            Console.WriteLine("=============== Create and Train the Model ===============");
            var model = estimator.Fit(trainingDataView);
            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();
            // evaluating model using test dataset
            Console.WriteLine("=============== Evaluating Model accuracy with Test data===============");
            IDataView predictions = model.Transform(testingDataView);
            CalibratedBinaryClassificationMetrics metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");
            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End of model evaluation ===============");
            // saving model
            Console.WriteLine($"=============== Saving the model  ===============");
            mlContext.Model.Save(model, trainingDataView.Schema,_modelPath);
            Console.WriteLine("The model is saved to {0}", _modelPath);

            IEnumerable<ModelInput> sentiments = new[]
            {
                new ModelInput
                {
                    SentimentText = "This was a horrible meal"
                },
                new ModelInput
                {
                    SentimentText = "I love this spaghetti."
                },
                new ModelInput
                {
                    SentimentText = "I hate rock and roll."
                },
                new ModelInput
                {
                    SentimentText = "I hate loving you."
                }
            };

            IDataView batchComments = mlContext.Data.LoadFromEnumerable(sentiments);

            IDataView bpredictions = model.Transform(batchComments);

            // Use model to predict whether comment data is Positive (1) or Negative (0).
            IEnumerable<ModelOutput> predictedResults = mlContext.Data.CreateEnumerable<ModelOutput>(bpredictions, reuseRowObject: false);

            Console.WriteLine();
            // Display predicted data
            Console.WriteLine("=============== Prediction Test of loaded model with multiple samples ===============");    
            foreach (ModelOutput prediction  in predictedResults)
                {
                     Console.WriteLine($"Sentiment: {prediction.SentimentText} | Prediction: {(Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative")} | Probability: {prediction.Probability} ");                     
                }
            Console.WriteLine("=============== End of predictions ===============");
       }


}

}