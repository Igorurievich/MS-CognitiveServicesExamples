using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using VisionApiDemo.Core.Helpers;

namespace VisionApiDemo.Core
{
    public class FaceRecognizer
    {
        public string SubscriptionKey { get; }
        public string ApiRoot { get; }

        private readonly FaceServiceClient _faceServiceClient;

        public FaceRecognizer(string subscriptionKey, string apiRoot)
        {
            if (string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(apiRoot))
            {
                return;
            }
            SubscriptionKey = subscriptionKey;
            ApiRoot = apiRoot;
            _faceServiceClient = new FaceServiceClient(SubscriptionKey, ApiRoot);
        }

        public async Task<string> AnalyzeUrlAsync(string imageUrl, Enums.VisualFeature visualFeature)
        {
            var analysisResult = await _faceServiceClient.DetectAsync(imageUrl, false, true);
            string formattedResultString = AnalisysHelper.GetFaceInfo();
            return formattedResultString;
        }

        public async Task<string> AnalyzeImageFromDisk(Stream imageStream, Enums.VisualFeature visualFeature)
        {
            try
            {
                var analysisResult = await _faceServiceClient.DetectAsync(imageStream, true, true);
                string formattedResultString = AnalisysHelper.GetFaceInfo();
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
