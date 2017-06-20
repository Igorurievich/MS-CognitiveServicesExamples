using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VisionApiStreamDemo
{
    public static class ImageExtension
    {
        public static Stream ToStream(this Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }
    }
}
