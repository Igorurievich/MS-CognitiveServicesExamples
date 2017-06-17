using System;
using System.Collections.Generic;
using System.IO;

namespace VisionApiDemo.Core
{
    class TextFileHelper
    {
        public static string[] GetStringFromTextFle(string path)
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
