using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.ML;
using PredictFunctionsApp;

/* based on https://github.com/BorisWilhelms/azure-function-dependency-injection/blob/master/readme.md
https://github.com/dotnet/docs/blob/master/docs/machine-learning/how-to-guides/serve-model-serverless-azure-functions-ml-net.md
 */
namespace PredictFunctionsApp
{
 public class HttpTriggerMLNet
    {
    [FunctionName("PredictionService")]
    
    public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, // was post
    ILogger log)
    {
    log.LogInformation("C# HTTP trigger function processed a request.");

    //Parse HTTP Request Body
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    // send input data to ML model
    ModelInput input = JsonConvert.DeserializeObject<ModelInput>(requestBody);

    // Print for logs
    Type tinput = input.GetType();  
        PropertyInfo [] pin = tinput.GetProperties();
        foreach (PropertyInfo p in pin)
        {
            System.Console.WriteLine("{0} : {1} : {2}",p.Name ,p.GetValue(input), p.ToString() );
        }


    //Make Prediction
    ModelOutput prediction = _predictionEnginePool.Predict(input);

    //Convert prediction to string
    //string sentiment = Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative";
    //string output =prediction.Prediction;
    string output="Awesome function!" +requestBody;  
    //Return Prediction
    return (ActionResult) new OkObjectResult(output);
    } /* of Run */

    private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

    // Predict Model class constructor

    public HttpTriggerMLNet(PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
    {
        _predictionEnginePool = predictionEnginePool;
    }
   
    }

    
}
