using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using VisionApiDemo.Core.Helpers;
using System.Collections.Generic;

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

        public async Task<List<FacePosition>> AnalyzeUrlAsync(string imageUrl)
        {
            var analysisResult = await _faceServiceClient.DetectAsync(imageUrl, false, true);
            return AnalisysHelper.GetFaceInfo(analysisResult);
        }

        public async Task<List<FacePosition>> AnalyzeImageFromDisk(Stream imageStream)
        {
            try
            {
                var analysisResult = await _faceServiceClient.DetectAsync(imageStream, true, true);
                return AnalisysHelper.GetFaceInfo(analysisResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

    }
}
