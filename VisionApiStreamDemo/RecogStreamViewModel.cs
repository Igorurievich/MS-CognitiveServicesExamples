using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Expression.Interactivity.Core;
using VisionApiDemo.Core;
using Color = System.Drawing.Color;
using System.Drawing;
using System.Drawing.Imaging;

namespace VisionApiStreamDemo
{
    public class RecogStreamViewModel : ViewModelBase
    {
        private readonly Recognizer _recognizer;
        private FilterInfoCollection _localCameraDevicesList;
        private IEnumerable<string> _recognizeMode;
        private ImageSource _actualFrame;
        private Image _actualFrameImage;
        private ImageSource _streamImageSourceObj;
        private ImageSource _actualFrameSourceObj;
        private VideoCaptureDevice _localWebCam;
        private BackgroundWorker _backgroudWorker;

        public ICommand WindowLoaded { get; set; }
        public ICommand StartVideoCameraStream { get; set; }
        public ICommand ReleaseResourcesBeforeClosing { get; set; }
        public ICommand StopVideoCameraStream { get; set; }

        public FilterInfoCollection LocalCameraDevicesCollection
        {
            get { return _localCameraDevicesList; }
            set
            {
                _localCameraDevicesList = value;
                NotifyPropertyChanged(nameof(LocalCameraDevicesCollection));
            }
        }
        public IEnumerable<string> RecognizeMode
        {
            get { return _recognizeMode; }
            set
            {
                _recognizeMode = value;
                NotifyPropertyChanged(nameof(RecognizeMode));
            }
        }
        public ImageSource StreamImageSourceObj
        {
            get { return _streamImageSourceObj; }
            set
            {
                _streamImageSourceObj = value;
                NotifyPropertyChanged(nameof(StreamImageSourceObj));
            }
        }
        public ImageSource ActualFrameSourceObj
        {
            get { return _actualFrameSourceObj; }
            set
            {
                _actualFrameSourceObj = value;
                NotifyPropertyChanged(nameof(ActualFrameSourceObj));
            }
        }

        public RecogStreamViewModel()
        {
            _recognizer = new Recognizer("9d5344f8aebf4d76928f4244fa87294a", "https://westeurope.api.cognitive.microsoft.com/vision/v1.0");
            LocalCameraDevicesCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
           
            _backgroudWorker = new BackgroundWorker();
            _backgroudWorker.DoWork += WorkerOnDoWork;
            _backgroudWorker.WorkerSupportsCancellation = true;

            InitializeCommand();
        }

        private void InitializeCommand()
        {
            WindowLoaded = new ActionCommand(WindowLoadedCallback);
            StartVideoCameraStream = new ActionCommand(StartVideoCameraStreamCallback);
            ReleaseResourcesBeforeClosing = new ActionCommand(ReleaseResourcesBeforeClosingCallback);
            StopVideoCameraStream = new ActionCommand(StopVideoCameraStreamCallback);
        }

        private void StopVideoCameraStreamCallback()
        {
            if (_backgroudWorker.IsBusy)
            {
                _backgroudWorker.CancelAsync();
            }
            _backgroudWorker.CancelAsync();

            if (_localWebCam == null) return;
            if (_localWebCam.IsRunning)
            {
                _localWebCam.SignalToStop();
                _localWebCam = null;
            }
        }

        private void ReleaseResourcesBeforeClosingCallback()
        {
            StopVideoCameraStreamCallback();
        }

        public void WindowLoadedCallback()
        {
            
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            _backgroudWorker = sender as BackgroundWorker;
            int delay = 2000;

            while (!_backgroudWorker.CancellationPending)
            {
                var tempActualFrame = _actualFrameImage;

                tempActualFrame = DrawRectangleOnFrame(tempActualFrame);

                MemoryStream ms = new MemoryStream();
                tempActualFrame.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                bi.Freeze();

                ActualFrameSourceObj = bi;

                Thread.Sleep(delay);
            }
            
            doWorkEventArgs.Cancel = true;
        }

        public void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Image img = (Bitmap)eventArgs.Frame.Clone();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                bi.Freeze();
                
                StreamImageSourceObj = bi;
                _actualFrame = bi;
                _actualFrameImage = img;
            }
            catch (Exception ex)
            {

            }
        }

        private static BitmapImage convertImageToBI(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            bi.Freeze();
            return bi;
        }

        public void StartVideoCameraStreamCallback()
        {
            _localWebCam = new VideoCaptureDevice(LocalCameraDevicesCollection[0].MonikerString);
            _localWebCam.NewFrame += new NewFrameEventHandler(video_NewFrame);

            _localWebCam.Start();

            if (!_backgroudWorker.IsBusy)
            {
                Thread.Sleep(1000);
                _backgroudWorker.RunWorkerAsync();
            }
        }

        private Image DrawRectangleOnFrame(Image sourceImage)
        {
            using (Graphics g = Graphics.FromImage(sourceImage))
            {
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Brushes.Red, 5),
                    new Rectangle(50, 50, 100, 100));

                return sourceImage;
            }
        }
    }
}
