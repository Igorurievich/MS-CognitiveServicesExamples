namespace VisionApiDemo.Core.Helpers
{
    public class FacePosition
    {
        public FacePosition(int top, int down, int height, int width)
        {
            Top = top;
            Down = down;
            Height = height;
            Width = width;
        }
        public int Top { get; set; }
        public int Down { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
