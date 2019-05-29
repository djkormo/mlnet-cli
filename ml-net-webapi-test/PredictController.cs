using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Logging;
using System.Reflection;
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
        Console.WriteLine("Beginning prediction");
        Console.WriteLine("input->:");
        Type tinput = input.GetType(); // Where obj is object whose properties you need.
        PropertyInfo [] pin = tinput.GetProperties();
        foreach (PropertyInfo p in pin)
        {
            System.Console.WriteLine(p.Name + " : " + p.GetType()+" : "+p.ToString() );
        }

        ModelOutput prediction = _predictionEnginePool.Predict(input);
        Console.WriteLine("output->:");

        Type toutput = prediction.GetType(); // Where obj is object whose properties you need.
        PropertyInfo [] pout = toutput.GetProperties();
        foreach (PropertyInfo p in pout)
        {
            System.Console.WriteLine(p.Name + " : " + p.GetType()+" : "+p.ToString() );
        }
        
        string output = prediction.Prediction;
        Console.WriteLine("Ending prediction:" + output);
        return Ok(output);
    }
}
}