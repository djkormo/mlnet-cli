using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
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
            return BadRequest();
        }

        ModelOutput prediction = _predictionEnginePool.Predict(input);

        string sentiment = Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative";

        return Ok(sentiment);
    }
}
}