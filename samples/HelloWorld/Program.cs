using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
   
 /*
 based on https://docs.microsoft.com/en-us/dotnet/machine-learning/how-does-mldotnet-work
  */  
class Program
   {
       public class ModelInput
       {
           public float Size { get; set; }
           public float Price { get; set; }
       }
   
       public class ModelOutput
       {
           [ColumnName("Score")]
           public float Price { get; set; }
       }
   
       static void Main(string[] args)
       {
           MLContext mlContext = new MLContext();
   
           // 1. Import or create training data
           ModelInput[] modelData = {
               new ModelInput() { Size = 1.1F, Price = 1.2F },
               new ModelInput() { Size = 1.9F, Price = 2.3F },
               new ModelInput() { Size = 2.8F, Price = 3.0F },
               new ModelInput() { Size = 3.4F, Price = 3.7F } };
           IDataView trainingData = mlContext.Data.LoadFromEnumerable(modelData);

           // 2. Specify data preparation and model training pipeline
           var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Size" })
               .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Price", maximumNumberOfIterations: 100));
   
           // 3. Train model
           var model = pipeline.Fit(trainingData);
   
           // 4. Make a prediction
           var size = new ModelInput() { Size = 2.5F };
           var price = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model).Predict(size);

           Console.WriteLine($"Predicted price for size: {size.Size*1000} sq ft= {price.Price*100:C}k");

           // Predicted price for size: 2500 sq ft= $261.98k
       }
   }