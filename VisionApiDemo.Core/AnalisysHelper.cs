using System.Linq;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace VisionApiDemo.Core
{
    public class AnalisysHelper
    {
        public static string GetInfo(AnalysisResult result, VisualFeature visualFeature)
        {
            string resultString = string.Empty;
            switch (visualFeature)
            {
                case VisualFeature.Adult:
                    resultString += "Adult score: " + result.Adult.AdultScore+ "\r";
                    resultString += "Racy score: " + result.Adult.RacyScore +"\n";
                    break;

                case VisualFeature.Categories:
                    foreach (var item in result.Categories)
                    {
                        resultString += "Category name: " + item.Name + "\r";
                        resultString += "\tCategory details: " + item.Detail + "\r";
                        resultString += "\tCategory score: " + item.Score + "\n";
                    }
                    break;

                case VisualFeature.Description:
                    foreach (var item in result.Description.Captions)
                    {
                        resultString += "Caption text: " + item.Text + "\r";
                        resultString += "Caption confidence: " + item.Confidence + "\n";
                    }
                    break;
                case VisualFeature.Color:
                    resultString += "Accent color: " + result.Color.AccentColor + "\r";
                    resultString += "Dominant background color: " + result.Color.DominantColorBackground + "\r";
                    resultString += "Dominant foreground color: " + result.Color.DominantColorForeground + "\r";
                    resultString += "Dominant foreground colors: " + "\n";
                    resultString = result.Color.DominantColors.Aggregate(resultString, (current, item) => current + "\t\tColor: " + item + "\r");
                    break;

                case VisualFeature.Faces:

                    foreach (var item in result.Faces)
                    {
                        resultString += "Face age: " + item.Age + "\r";
                        resultString += "Face position: " + item.FaceRectangle + "\r";
                        resultString += "Face gender: " + item.Gender + "\n";
                    }
                    break;

                case VisualFeature.Tags:
                    resultString += "Tags: " + "\n";
                    foreach (var item in result.Tags)
                    {
                        resultString += "\tTag name: " + item.Name + "\r";
                        resultString += "\tTag confidence: " + item.Confidence + "\n";

                    }
                    break;

                case VisualFeature.ImageType:
                        resultString += "Clip art type: " + result.ImageType.ClipArtType + "\r";
                        resultString += "Line drawing type: " + result.ImageType.LineDrawingType + "\n";
                    break;

                default:
                    resultString += "Image format: " + result.Metadata.Format + "\r";
                    resultString += "Image height: " + result.Metadata.Height + "\r";
                    resultString += "Image width: " + result.Metadata.Width + "\n";
                    break;
            }
            return resultString;
        }
    }
}
