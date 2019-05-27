using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Logging;
using Djkormo.Function;

namespace Djkormo.Function
{
    class Startup
    {
    private readonly IHostingEnvironment _env;
    private readonly IConfiguration _config;
    private readonly ILoggerFactory _loggerFactory;
    public Startup(IHostingEnvironment env, IConfiguration config, 
        ILoggerFactory loggerFactory)
    {
        _env = env;
        _config = config;
        _loggerFactory = loggerFactory;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        services.AddMemoryCache();
        services.AddPredictionEnginePool<ModelInput, ModelOutput>()
        .FromFile("model/model.zip");
        Console.WriteLine("Loading model/model.zip");
}


    // Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,IHostingEnvironment env)
    {

        if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Console.WriteLine("Loading UseDeveloperExceptionPage()");
            }

        app.UseMvc();
        Console.WriteLine("Loading Configure()");
    }
    }
}
