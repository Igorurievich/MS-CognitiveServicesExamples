using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace VisionApiDemo.Core
{
    public class Recognizer
    {
        public string SubscriptionKey { get; }
        public string ApiRoot { get; }

        private readonly VisionServiceClient _visionServiceClient;

        public Recognizer(string subscriptionKey, string apiRoot)
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
            string formattedResultString = AnalisysHelper.GetInfo(analysisResult, (VisualFeature)visualFeature);
            return formattedResultString;
        }

        public async Task<string> AnalyzeImageFromDisk(Stream imageStream, Enums.VisualFeature visualFeature)
        {
            AnalysisResult analysisResult = await _visionServiceClient.AnalyzeImageAsync(imageStream, new [] { (VisualFeature)visualFeature });
            string formattedResultString = AnalisysHelper.GetInfo(analysisResult, (VisualFeature)visualFeature);
            return formattedResultString;
        }
    }
}
