using Azure;
using Azure.AI.Vision.ImageAnalysis;
using image_analysis.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using image_analysis.ProcessImage;
using image_analysis.Analyze;
using Microsoft.Extensions.Options;

namespace image_analysis
{
    class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddJsonFile("appsettings.json")
                    //todo Change secret id for assembly
                   .AddUserSecrets("780126f1-21ac-4b58-ae35-41108f50e15b");
            })
            .ConfigureServices((hostContext, services) =>
            {
                // register your services here.
                services.Configure<AzOptions>(hostContext.Configuration.GetSection("AzOptions"));
                services.AddScoped<IAnalyzeService, AnalyzeService>();
                services.AddSingleton(service =>
                {
                    var azOptions = service.GetRequiredService<IOptions<AzOptions>>();
                    return new ImageAnalysisClient(
                                new Uri(azOptions.Value.AIServicesEndpoint),
                                new AzureKeyCredential(azOptions.Value.AIServicesKey));
                });
                services.AddScoped(services =>
                {
                    return new IProcessImageResult[]
                    {
                        new ProcessCaption(), new ProcessDenseCaption(), new ProcessObjects(), new ProcessPeople()
                    }.AsEnumerable();
                });
            });

        static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            using IHost host = builder.Build();

            // Application code should start here.
            await Console.Out.WriteLineAsync("mi rograma");
            try
            {
                // Get image
                string imageFile = "images/street.jpg";
                if (args.Length > 0)
                {
                    imageFile = args[0];
                }
                var analyzeService = host.Services.GetRequiredService<IAnalyzeService>();
                var result = analyzeService.GetImageAnalysisResult(imageFile);
                var processors = host.Services.GetRequiredService<IEnumerable<IProcessImageResult>>();
                processors.ToList().ForEach(p => p.Process(result,imageFile));
                var pictureBack = await analyzeService.BackgroundForeground(imageFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await host.RunAsync();
        }
    }
}