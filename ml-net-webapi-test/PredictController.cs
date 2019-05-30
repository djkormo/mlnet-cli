using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.ComponentModel;
using Djkormo.Web;
namespace Djkormo.Web
{
[Route("api/[controller]")]
[ApiController]
public class PredictController : ControllerBase
{
    private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

     //The predictionEnginePool is injected through the constructor
    public PredictController(PredictionEnginePool<ModelInput,ModelOutput> predictionEnginePool)
    {
        _predictionEnginePool = predictionEnginePool;
    }

    

    // POST api/predict/model
    
    [HttpPost]
    [Route("model")]

    public ActionResult<string> Post([FromBody] ModelInput input)
    
    {
        if(!ModelState.IsValid)
        {
            Console.WriteLine("Bad request");
            
            return BadRequest();
        }
        Console.WriteLine("Beginning prediction...");
        Console.WriteLine("ModelInput class:");
        Type tinput = input.GetType(); 
        PropertyInfo [] pin = tinput.GetProperties();
        foreach (PropertyInfo p in pin)
        {
            System.Console.WriteLine("{0} : {1} : {2}",p.Name ,p.GetValue(input), p.ToString() );
        }
        // Getting output from ML model  input class(ModelInput) -> prediction class (ModelOutput)
        ModelOutput prediction = _predictionEnginePool.Predict(input);
        Console.WriteLine("ModelOutput class: ");

        Type toutput = prediction.GetType(); 
        PropertyInfo [] pout = toutput.GetProperties();
        foreach (PropertyInfo p in pout)
        {
            System.Console.WriteLine("{0} : {1} : {2}",p.Name ,p.GetValue(prediction),p.ToString() );
        }
        
        string output = prediction.Prediction;
        Console.WriteLine("Ending prediction...");
        // send output value 
        return Ok(output);
    }


}
}