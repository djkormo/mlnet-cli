using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Djkormo.Function;

/* based on for swagger https://docs.microsoft.com/pl-pl/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio-code */
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

        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Info
            {
                Version = "v1",
                Title = "Prediction API",
                Description = "A simple ML.NET example with Core Web API",
                TermsOfService = "None",
                Contact = new Contact
            {
            Name = "Krzysztof Pudlowski",
            Email = string.Empty,
            Url = "http://wchmurze.cloud"
        },
        License = new License
        {
            Name = "Use under LICX",
            Url = "https://github.com/djkormo/mlnet-cli"
        }
    });
});
}


    // Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,IHostingEnvironment env)
    {

      app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
    // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
      });
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
