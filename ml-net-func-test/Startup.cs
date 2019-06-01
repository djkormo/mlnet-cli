using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.ML.Data;
using PredictFunctionsApp;

[assembly: WebJobsStartup(typeof(Startup))]
namespace PredictFunctionsApp
{
    class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
           

            builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>()
                .FromFile("model/model.zip");
        }
       
    }
}
