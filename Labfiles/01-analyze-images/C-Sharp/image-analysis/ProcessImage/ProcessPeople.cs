using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Drawing;

namespace image_analysis.ProcessImage
{
    public class ProcessPeople : IProcessImageResult
    {
        public void Process(ImageAnalysisResult result, string imageFilePath)
        {
            // Get people in the image
            if (result.People.Values.Count > 0)
            {
                Console.WriteLine($" People:");

                // Prepare image for drawing
                Image image = Image.FromFile(imageFilePath);
                Graphics graphics = Graphics.FromImage(image);
                Pen pen = new Pen(Color.Cyan, 3);
                Font font = new Font("Arial", 16);
                SolidBrush brush = new SolidBrush(Color.WhiteSmoke);

                foreach (DetectedPerson person in result.People.Values)
                {
                    // Draw object bounding box
                    var r = person.BoundingBox;
                    Rectangle rect = new Rectangle(r.X, r.Y, r.Width, r.Height);
                    graphics.DrawRectangle(pen, rect);

                    // Return the confidence of the person detected
                    //Console.WriteLine($"   Bounding box {person.BoundingBox.ToString()}, Confidence: {person.Confidence:F2}");
                }

                // Save annotated image
                string output_file = "persons.jpg";
                image.Save(output_file);
                Console.WriteLine("  Results saved in " + output_file + "\n");
            }
        }
    }
}