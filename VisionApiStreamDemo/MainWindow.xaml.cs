using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Threading;
using VisionApiDemo.Core;

namespace VisionApiStreamDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage _actualFrame;
        private readonly Recognizer _recognizer;
        private int ticks = 0;

        public FilterInfoCollection LoaclWebCamsCollection;
        private BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();

            LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var localWebCam = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
            localWebCam.NewFrame += FinalVideo_NewFrameAsync;
            localWebCam.Start();

            _recognizer = new Recognizer("9d5344f8aebf4d76928f4244fa87294a", "https://westeurope.api.cognitive.microsoft.com/vision/v1.0");
            worker = new BackgroundWorker();
            worker.DoWork += WorkerOnDoWork;
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int delay = 2000;

            while (!worker.CancellationPending)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    RightImage.Source = _actualFrame;
                }));
                
                Thread.Sleep(delay);
            }
            doWorkEventArgs.Cancel = true;
        }

        public delegate void UpdateTextCallback(string text);

        private void UpdateText(string text)
        {
            MessageArea.Text = text;
        }

        private void FinalVideo_NewFrameAsync(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                bi.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    LeftImage.Source = bi;
                }));
                _actualFrame = bi;
            }
            catch (Exception ex)
            {

            }
        }

        private void GetFrame(Bitmap frame)
        {
            MessageArea.Dispatcher.BeginInvoke(new UpdateTextCallback(UpdateText), "TICKS:" + ticks++);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync(2000);
        }

        private void CameraList_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void ModeList_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ModeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void AnaliseActualFrame()
        {
            using (Stream imageFileStream = _actualFrame.StreamSource)
            {

                //return analysisResult;
            }
        }
    }
}
