using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Logging;
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
    // POST api/predict/model?SentimentText=Good
    // GET api/predict/model?SentimentText=Scary
    [HttpPost]
    [Route("model")]
    // TODO pobranie Jsona ... na wejsciu ...
    public ActionResult<string> Post([FromBody] ModelInput input)
    //public async Task<IActionResult> Get([FromQuery(Name = "query")] string query)
    {
        if(!ModelState.IsValid)
        {
            Console.WriteLine("Bad request");
            //throw new ArgumentException("Classify service cannot be null.");
            return BadRequest();
        }
        Console.WriteLine("Beginning prediction");
        Console.WriteLine("input->:",input);
        ModelOutput prediction = _predictionEnginePool.Predict(input);
        Console.WriteLine("output->:",prediction);

        //string sentiment = Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative";
        string output = prediction.Prediction;
        Console.WriteLine("Ending prediction");
        return Ok(output);
    }
}
}