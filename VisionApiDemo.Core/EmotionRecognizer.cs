using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Emotion;
using VisionApiDemo.Core.Helpers;

namespace VisionApiDemo.Core
{
    public class EmotionRecognizer
    {
        public string SubscriptionKey { get; }
        public string ApiRoot { get; }

        private readonly EmotionServiceClient _faceServiceClient;

        public EmotionRecognizer(string subscriptionKey, string apiRoot)
        {
            if (string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(apiRoot))
            {
                return;
            }
            SubscriptionKey = subscriptionKey;
            ApiRoot = apiRoot;
            _faceServiceClient = new EmotionServiceClient(SubscriptionKey, ApiRoot);
        }

        public async Task<string> AnalyzeUrlAsync(string imageUrl, Rectangle[] faceRectangles)
        {
            var analysisResult = await _faceServiceClient.RecognizeAsync(imageUrl, faceRectangles);
            string formattedResultString = AnalisysHelper.GetEmotionInfo();
            return formattedResultString;
        }

        public async Task<string> AnalyzeImageFromDisk(Stream imageStream, Rectangle[] faceRectangles)
        {
            try
            {
                var analysisResult = await _faceServiceClient.RecognizeAsync(imageStream, faceRectangles);
                string formattedResultString = AnalisysHelper.GetEmotionInfo();
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
