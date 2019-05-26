using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.ML;
using Djkormo.Function;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Djkormo.Function
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
