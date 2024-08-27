using Azure.AI.Vision.ImageAnalysis;
using System;

namespace image_analysis.ProcessImage
{
    public class ProcessCaption : IProcessImageResult
    {
        public void Process(ImageAnalysisResult result, string imageFilePath)
        {
            // Display analysis results
            // Get image captions
            if (result.Caption.Text != null)
            {
                Console.WriteLine(" Caption:");
                Console.WriteLine($"   \"{result.Caption.Text}\", Confidence {result.Caption.Confidence:0.00}\n");
            }
        }
    }
}