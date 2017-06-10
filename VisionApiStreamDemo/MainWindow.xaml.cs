using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AForge.Video;

namespace VisionApiStreamDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        VideoCaptureDevice LocalWebCam;
        public FilterInfoCollection LoaclWebCamsCollection;

        private BackgroundWorker worker;


        private int ticks = 0;

        public MainWindow()
        {
            InitializeComponent();

            LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            LocalWebCam = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
            LocalWebCam.NewFrame += FinalVideo_NewFrame;

            LocalWebCam.Start();


            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public delegate void UpdateTextCallback(string text);

        private void UpdateText(string text)
        {
            MessageArea.Text = text;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {


            MessageArea.Dispatcher.BeginInvoke(new UpdateTextCallback(UpdateText), "TICKS:" + ticks++);
        }

        public async void TheEnclosingMethod()
        {
            await Task.Delay(10000).ContinueWith( e =>
            {
               
            });
        }

        private void FinalVideo_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap) eventArgs.Frame.Clone();

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
            }
            catch (Exception ex)
            {

            }

            TheEnclosingMethod();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            
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
    }
}
