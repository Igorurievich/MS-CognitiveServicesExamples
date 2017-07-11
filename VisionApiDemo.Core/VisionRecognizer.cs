using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using VisionApiDemo.Core.Helpers;

namespace VisionApiDemo.Core
{
    public class VisionRecognizer
    {
        public string SubscriptionKey { get; }
        public string ApiRoot { get; }

        private readonly VisionServiceClient _visionServiceClient;

        public VisionRecognizer(string subscriptionKey, string apiRoot)
        {
            if (string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(apiRoot))
            {
                return;
            }
            SubscriptionKey = subscriptionKey;
            ApiRoot = apiRoot;
            _visionServiceClient = new VisionServiceClient(SubscriptionKey, ApiRoot);
        }

        public async Task<string> AnalyzeUrlAsync(string imageUrl, Enums.VisualFeature visualFeature)
        {
            AnalysisResult analysisResult = await _visionServiceClient.AnalyzeImageAsync(imageUrl, new [] { (VisualFeature)visualFeature});
            string formattedResultString = AnalisysHelper.GetVisionInfo(analysisResult, (VisualFeature)visualFeature);
            return formattedResultString;
        }

        public async Task<string> AnalyzeImageAsync(Stream imageStream, Enums.VisualFeature visualFeature)
        {
            try
            {
                AnalysisResult analysisResult = await _visionServiceClient.AnalyzeImageAsync(imageStream, new[] { (VisualFeature)visualFeature });
                string formattedResultString = AnalisysHelper.GetVisionInfo(analysisResult, (VisualFeature)visualFeature);
                return formattedResultString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
