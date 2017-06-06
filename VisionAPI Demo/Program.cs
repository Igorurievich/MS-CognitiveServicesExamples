using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VisionAPI_Demo
{
    static class Program
    {
        static void Main()
        {
            Console.Write("Enter image file path: ");
            string imageFilePath = @"C:\Users\ibodia\Desktop\123.jpg";

            MakeAnalysisRequest(imageFilePath);

            Console.WriteLine("\n\nHit ENTER to exit...");
            Console.ReadLine();
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static async void MakeAnalysisRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "visualFeatures=Description&details=Landmarks&language=en";
            string uri = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;

            Console.WriteLine(uri);
            
            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(uri, content);

                Console.WriteLine("Is succes ?" + response.IsSuccessStatusCode);
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
                
                Stream receiveStream = await response.Content.ReadAsStreamAsync();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                Console.WriteLine(JObject.Parse(readStream.ReadToEnd()).ToString());
            }
        }
    }
}
