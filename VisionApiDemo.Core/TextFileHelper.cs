using System;
using System.Collections.Generic;
using System.IO;
using VisionApiDemo.Core;

namespace VisionApiDemo.Core
{
    public static class TextFileHelper
    {
        public static string VisionKey { get; }
        public static string VisionEndpoint { get; }

        public static string FacesKey { get; }
        public static string FacesEndpoint { get; }

        public static string EmotionsKey { get; }
        public static string EmotionsEndpoint { get; }

        static TextFileHelper()
        {
            var dataMassive = fillPropertiesFromTextFile("..\\..\\..\\VisionApiDemo.Core\\Resources\\KeysAndPoints.txt");

            VisionEndpoint = dataMassive[1];
            VisionKey = dataMassive[2];

            FacesEndpoint = dataMassive[5];
            FacesKey = dataMassive[6];

            EmotionsEndpoint = dataMassive[9];
            EmotionsKey = dataMassive[10];
        }

        private static string[] fillPropertiesFromTextFile(string path)
        {
            List<string> pointAndKeys = new List<string>();

            try
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    string s = "";
                    while ((s = reader.ReadLine()) != null)
                    {
                        pointAndKeys.Add(s);
                    }
                }
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            return pointAndKeys.ToArray();
        }
    }
}
