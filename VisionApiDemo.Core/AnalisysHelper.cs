using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json.Linq;

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
                    resultString += result.Adult.AdultScore+"\n";
                    resultString += result.Adult.RacyScore +"\n";
                    break;

                case VisualFeature.Categories:
                    foreach (var item in result.Categories)
                    {
                        resultString += item.Name + "\n";
                        resultString += item.Detail + "\n";
                    }
                    break;

                case VisualFeature.Description:
                    foreach (var item in result.Description.Captions)
                    {
                        resultString += item.Text + "\n";
                        resultString += item.Confidence + "\n";
                    }
                    break;
            }
            return resultString;
        }
    }
}
