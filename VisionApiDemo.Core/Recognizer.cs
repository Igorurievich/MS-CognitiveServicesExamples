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

        public async Task<AnalysisResult> AnalyzeUrl(string imageUrl, VisualFeature[] visualFeatures)
        {
            AnalysisResult analysisResult = await _visionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures);
            return analysisResult;
        }
    }
}
