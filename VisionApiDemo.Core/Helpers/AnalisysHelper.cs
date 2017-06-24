using System;
using System.Linq;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Collections.Generic;

namespace VisionApiDemo.Core.Helpers
{
    public class AnalisysHelper
    {
        public static string GetVisionInfo(AnalysisResult result, VisualFeature visualFeature)
        {
            string resultString = string.Empty;
            switch (visualFeature)
            {
                case VisualFeature.Adult:
                    resultString += "Adult score: " + result.Adult.AdultScore+ "\r";
                    resultString += "Racy score: " + result.Adult.RacyScore +"\n";
                    break;

                case VisualFeature.Categories:
                    foreach (var category in result.Categories)
                    {
                        resultString += "Category name: " + category.Name + "\r";
                        resultString += "\tCategory details: " + category.Detail + "\r";
                        resultString += "\tCategory score: " + category.Score + "\n";
                    }
                    break;

                case VisualFeature.Description:
                    foreach (var captions in result.Description.Captions)
                    {
                        resultString += "Caption text: " + captions.Text + "\r";
                        resultString += "Caption confidence: " + captions.Confidence + "\n";
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
                    foreach (var faces in result.Faces)
                    {
                        resultString += "Face age: " + faces.Age + "\r";
                        resultString += "Face position: " + faces.FaceRectangle + "\r";
                        resultString += "Face gender: " + faces.Gender + "\n";
                    }
                    break;

                case VisualFeature.Tags:
                    resultString += "Tags: " + "\n";
                    foreach (var tags in result.Tags)
                    {
                        resultString += "\tTag name: " + tags.Name + "\r";
                        resultString += "\tTag confidence: " + tags.Confidence + "\n";
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

        public static List<FacePosition> GetFaceInfo(Microsoft.ProjectOxford.Face.Contract.Face[] faces)
        {
            List<FacePosition> facesDictionary = new List<FacePosition>();
            foreach (var face in faces)
            {
                var rect = face.FaceRectangle;
                facesDictionary.Add(new FacePosition(face.FaceRectangle.Height, face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Width));
            }
            return facesDictionary;
        }

        public static string GetEmotionInfo()
        {
            new NotImplementedException();
            return null;
        }
    }
}
