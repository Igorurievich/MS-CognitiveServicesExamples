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
using System.Threading.Tasks;
using VisionApiDemo.Core.Enums;

namespace VisionApiStreamDemo
{
    public class RecogStreamViewModel : ViewModelBase
    {
        #region Fields
        private readonly VisionRecognizer _recognizer;
        private FilterInfoCollection _localCameraDevicesList;
        private IEnumerable<string> _recognizeMode;
        private Image _actualFrameImage;
        private ImageSource _streamImageSourceObj;
        private ImageSource _actualFrameSourceObj;
        private VideoCaptureDevice _localWebCam;
        private BackgroundWorker _backgroudWorker;
        private string _selectedRecognizeMode;
        #endregion

        #region Commands
        public ICommand WindowLoaded { get; set; }
        public ICommand StartVideoCameraStream { get; set; }
        public ICommand ReleaseResourcesBeforeClosing { get; set; }
        public ICommand StopVideoCameraStream { get; set; }
        #endregion

        #region Properties
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
        public string SelectedRecognizeMode
        {
            get { return _selectedRecognizeMode; }
            set
            {
                _selectedRecognizeMode = value;
                NotifyPropertyChanged(nameof(SelectedRecognizeMode));
            }
        }
        #endregion

        public RecogStreamViewModel()
        {
            _recognizer = new VisionRecognizer("6c8189eb1bfc4ca1bb608261e7d56052", "https://westeurope.api.cognitive.microsoft.com/vision/v1.0");
            LocalCameraDevicesCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
           
            _backgroudWorker = new BackgroundWorker();
            _backgroudWorker.DoWork += WorkerOnDoWork;
            _backgroudWorker.WorkerSupportsCancellation = true;

            RecognizeMode = new List<string>
            {
               Globals.FacesMode,
               Globals.EmotionsMode,
               Globals.FacesWithEmotionsMode,
               Globals.TagsMode,
               Globals.DescriptionMode
            };
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            WindowLoaded = new ActionCommand(WindowLoadedCallback);
            StartVideoCameraStream = new ActionCommand(StartVideoCameraStreamCallback);
            ReleaseResourcesBeforeClosing = new ActionCommand(ReleaseResourcesBeforeClosingCallback);
            StopVideoCameraStream = new ActionCommand(StopVideoCameraStreamCallback);
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            _backgroudWorker = sender as BackgroundWorker;
            int delay = 2000;

            while (!_backgroudWorker.CancellationPending)
            {
                var tempActualFrame = _actualFrameImage;
                
                //
                tempActualFrame = DrawRectangleOnFrame(tempActualFrame);
                recognizeFrameAsync();
                //

                ActualFrameSourceObj = convertImageToBI(tempActualFrame);
                Thread.Sleep(delay);
            }
            
            doWorkEventArgs.Cancel = true;
        }

        public void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Image img = (Bitmap)eventArgs.Frame.Clone();
                StreamImageSourceObj = convertImageToBI((Bitmap)eventArgs.Frame.Clone());
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
        
        private Image DrawRectangleOnFrame(Image sourceImage)
        {
            using (Graphics g = Graphics.FromImage(sourceImage))
            {
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Brushes.Red, 5),
                    new Rectangle(50, 50, 100, 100));

                return sourceImage;
            }
        }

        private async Task recognizeFrameAsync()
        {
            switch (SelectedRecognizeMode)
            {
                case Globals.DescriptionMode:

                    //string resultString = await _recognizer.AnalyzeUrlAsync(, SelectedVisualFeature);
                    //AnalysisResultText = resultString;

                    break;
                case Globals.EmotionsMode:

                    break;
                case Globals.FacesMode:

                    break;
                case Globals.FacesWithEmotionsMode:

                    break;
                case Globals.TagsMode:

                    break;
            }
        }

        #region Commands implementation

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

        private void ReleaseResourcesBeforeClosingCallback()
        {
            StopVideoCameraStreamCallback();
        }

        public void WindowLoadedCallback()
        {

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

        #endregion
    }
}
