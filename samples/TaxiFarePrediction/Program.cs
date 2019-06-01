using System;
using System.IO;
using Microsoft.ML;
using ModelPrediction;

/* based on 
    https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/predict-prices 
    https://docs.microsoft.com/en-us/dotnet/machine-learning/how-does-mldotnet-work
    https://docs.microsoft.com/en-us/dotnet/machine-learning/resources/tasks

*/


namespace ModelPrediction
{
    class Program
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "data", "taxi-fare-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "data", "taxi-fare-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "data", "model.zip");

        static void Main(string[] args)
        {
            Console.WriteLine("#### Building model start");

            MLContext mlContext = new MLContext(seed: 0);
            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestSinglePrediction(mlContext, model);
            SaveModel(mlContext, model);
            Console.WriteLine("#### Building model end");
        }
        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            // Load data from flat file
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(dataPath, hasHeader: true, separatorChar: ',');

            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName:"FareAmount")
            // using categorical features -> OneHotEncoding
            .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName:"VendorId"))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
            .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
            // all columns for input model -> concatenate to Features column
            .Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripTime", "TripDistance", "PaymentTypeEncoded")) 
            // using regression algorithm
            .Append(mlContext.Regression.Trainers.FastTree());
            //  train model
            var model = pipeline.Fit(dataView);
            // return model
            return model;
        }
        private static void Evaluate(MLContext mlContext, ITransformer model)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(_testDataPath, hasHeader: true, separatorChar: ',');
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*      Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*      RSquared Score:          {metrics.RSquared:0.##}");
            Console.WriteLine($"*      Root Mean Squared Error: {metrics.RootMeanSquaredError:#.##}");

        }

        private static void TestSinglePrediction(MLContext mlContext, ITransformer model)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
            var ModelnputExample = new ModelInput()
            {
                VendorId = "VTS",
                RateCode = "1",
                PassengerCount = 1,
                TripTime = 1140,
                TripDistance = 3.75f,
                PaymentType = "CRD",
                FareAmount = 0 // To predict. Actual/Observed = 15.5
            };

            var prediction = predictionFunction.Predict(ModelnputExample);        
            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted fare: {prediction.FareAmount:0.####}, actual fare: 15.5");
            Console.WriteLine($"**********************************************************************");
        }

        public static void SaveModel(MLContext mlContext, ITransformer model)
        {
             IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(_trainDataPath, hasHeader: true, separatorChar: ',');
            // Save/persist the trained model to a .ZIP file
            Console.WriteLine($"=============== Saving the model  ===============");
            mlContext.Model.Save(model, dataView.Schema,_modelPath);
            Console.WriteLine("The model is saved to {0}", _modelPath);
        }

    }
}
