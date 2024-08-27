using Azure.AI.Vision.ImageAnalysis;
using System;

namespace image_analysis.ProcessImage
{
    public class ProcessDenseCaption : IProcessImageResult
    {
        public void Process(ImageAnalysisResult result, string imageFilePath)
        {
            // Get image dense captions
            Console.WriteLine(" Dense Captions:");
            foreach (DenseCaption denseCaption in result.DenseCaptions.Values)
            {
                Console.WriteLine($"   Caption: '{denseCaption.Text}', Confidence: {denseCaption.Confidence:0.00}");
            }
        }
    }
}