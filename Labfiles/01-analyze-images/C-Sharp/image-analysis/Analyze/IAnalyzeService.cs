using Azure.AI.Vision.ImageAnalysis;
using System.Threading.Tasks;

namespace image_analysis.Analyze
{
    public interface IAnalyzeService
    {
        ImageAnalysisResult GetImageAnalysisResult(string imageFile);
        Task<byte[]> BackgroundForeground(string imageFile);
    }
}
