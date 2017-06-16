using System.Windows;

namespace VisionApiStreamDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //var localWebCam = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
            //localWebCam.NewFrame += FinalVideo_NewFrameAsync;
            //localWebCam.Start();

            //_recognizer = new Recognizer("9d5344f8aebf4d76928f4244fa87294a", "https://westeurope.api.cognitive.microsoft.com/vision/v1.0");
            //worker = new BackgroundWorker();
            //worker.DoWork += WorkerOnDoWork;
        }
    }
}
