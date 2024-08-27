using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Drawing;

namespace image_analysis.ProcessImage
{
    public class ProcessObjects : IProcessImageResult
    {
        public void Process(ImageAnalysisResult result, string imageFilePath)
        {
            // Get objects in the image
            if (result.Objects.Values.Count > 0)
            {
                Console.WriteLine(" Objects:");

                // Prepare image for drawing
                //stream.Close();
                Image image = Image.FromFile(imageFilePath);
                Graphics graphics = Graphics.FromImage(image);
                Pen pen = new Pen(Color.Cyan, 3);
                Font font = new Font("Arial", 16);
                SolidBrush brush = new SolidBrush(Color.WhiteSmoke);

                foreach (DetectedObject detectedObject in result.Objects.Values)
                {
                    Console.WriteLine($"   \"{detectedObject.Tags[0].Name}\"");

                    // Draw object bounding box
                    var r = detectedObject.BoundingBox;
                    Rectangle rect = new Rectangle(r.X, r.Y, r.Width, r.Height);
                    graphics.DrawRectangle(pen, rect);
                    graphics.DrawString(detectedObject.Tags[0].Name, font, brush, r.X, r.Y);
                }

                // Save annotated image
                string output_file = "objects.jpg";
                image.Save(output_file);
                Console.WriteLine("  Results saved in " + output_file + "\n");
            }
        }
    }
}