using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Logging;
using Djkormo.Function;
namespace Djkormo.Function
{
public class PredictController : ControllerBase
{
    private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

    public PredictController(PredictionEnginePool<ModelInput,ModelOutput> predictionEnginePool)
    {
        _predictionEnginePool = predictionEnginePool;
    }

    [HttpPost]
    public ActionResult<string> Post([FromBody] ModelInput input)
    {
        if(!ModelState.IsValid)
        {
            Console.WriteLine("Bad request");
            return BadRequest();
        }
        Console.WriteLine("Beginning prediction");
        ModelOutput prediction = _predictionEnginePool.Predict(input);

        string sentiment = Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative";
        Console.WriteLine("Ending prediction");
        return Ok(sentiment);
    }
}
}