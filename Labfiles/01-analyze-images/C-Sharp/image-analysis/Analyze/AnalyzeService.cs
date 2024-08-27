using Azure.AI.Vision.ImageAnalysis;
using image_analysis.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace image_analysis.Analyze
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly ImageAnalysisClient _client;
        private readonly IOptions<AzOptions> _azOptions;

        public AnalyzeService(ImageAnalysisClient client, IOptions<AzOptions> azOptions)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _azOptions = azOptions ?? throw new ArgumentNullException(nameof(azOptions));
        }

        public ImageAnalysisResult GetImageAnalysisResult(string imageFile)
        {
            Console.WriteLine($"\nAnalyzing {imageFile} \n");

            // Use a file stream to pass the image data to the analyze call
            using var stream = new FileStream(imageFile, FileMode.Open);

            // Get result with specified features to be retrieved
            var result = _client.Analyze(
                BinaryData.FromStream(stream),
                VisualFeatures.Caption |
                VisualFeatures.DenseCaptions |
                VisualFeatures.Objects |
                VisualFeatures.Tags |
                VisualFeatures.People);

            return result;
        }
        public async Task<byte[]> BackgroundForeground(string imageFile)
        {
            // Remove the background from the image or generate a foreground matte
            Console.WriteLine($" Background removal:");
            // Define the API version and mode
            string apiVersion = "2023-02-01-preview";
            string mode = "backgroundRemoval"; // Can be "foregroundMatting" or "backgroundRemoval"

            string url = $"computervision/imageanalysis:segment?api-version={apiVersion}&mode={mode}";

            // Make the REST call
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_azOptions.Value.AIServicesEndpoint);
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _azOptions.Value.AIServicesKey);

                var data = new
                {
                    url = $"https://github.com/MicrosoftLearning/mslearn-ai-vision/blob/main/Labfiles/01-analyze-images/Python/image-analysis/{imageFile}?raw=true"
                };

                var jsonData = JsonSerializer.Serialize(data);
                var contentData = new StringContent(jsonData, Encoding.UTF8, contentType);
                var response = await client.PostAsync(url, contentData);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsByteArrayAsync().Result;
                    File.WriteAllBytes("background.png", result);
                    Console.WriteLine("  Results saved in background.png\n");
                    return result;
                }
                else
                {
                    Console.WriteLine($"API error: {response.ReasonPhrase} - Check your body url, key, and endpoint.");
                }
            }
            return [];
        }
    }
}
