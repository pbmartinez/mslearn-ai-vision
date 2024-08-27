using Azure.AI.Vision.ImageAnalysis;

namespace image_analysis.ProcessImage
{
    public interface IProcessImageResult
    {
        void Process(ImageAnalysisResult result, string imageFilePath);
    }
}
