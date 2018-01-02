using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Expression.Interactivity.Core;
using VisionApiDemo.Core;
using System.Drawing;
using System.Drawing.Imaging;
using VisionApiDemo.Core.Enums;
using VisionApiDemo.Core.Helpers;

namespace VisionApiStreamDemo
{
    public class RecogStreamViewModel : ViewModelBase
    {
        #region Fields
        private readonly VisionRecognizer _visionRecognizer;
        private readonly FaceRecognizer _faceRecognizer;
        private readonly EmotionRecognizer _emotionRecognizer;
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
            _visionRecognizer = new VisionRecognizer(TextFileHelper.VisionKey, TextFileHelper.VisionEndpoint);
            _faceRecognizer = new FaceRecognizer(TextFileHelper.FacesKey, TextFileHelper.FacesEndpoint);
            _emotionRecognizer = new EmotionRecognizer(TextFileHelper.EmotionsKey, TextFileHelper.EmotionsEndpoint);

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
                RecognizeFrameAsync();
                Thread.Sleep(delay);
            }
            
            doWorkEventArgs.Cancel = true;
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Image img = (Bitmap)eventArgs.Frame.Clone();
                StreamImageSourceObj = ConvertImageToBitmapImage((Bitmap)eventArgs.Frame.Clone());
                _actualFrameImage = img;
            }
            catch (Exception) { }
        }

        private static BitmapImage ConvertImageToBitmapImage(Image img)
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
        
        private Image DrawRectangleOnFrame(Image sourceImage, List<FacePosition> faces)
        {
            foreach (var face in faces)
            {
                if (sourceImage == null)
                {
                    return null;
                }
                using (Graphics g = Graphics.FromImage(sourceImage))
                {
                    g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Brushes.Red, 5),
                        new Rectangle(face.Down, face.Top, face.Width, face.Height));
                }
            }
            return sourceImage;
        }

        private async void RecognizeFrameAsync()
        {
            Image imageForRecognizing;
            imageForRecognizing = _actualFrameImage;
            switch (SelectedRecognizeMode)
            {
                case Globals.DescriptionMode:
                    
                    //string resultString = await _visionRecognizer.AnalyzeImage(imageForRecognizing.ToStream(ImageFormat.Jpeg), VisualFeature.Description);
                    break;
                case Globals.EmotionsMode:

                    //string resultString = await _emotionRecognizer.AnalyzeImageFromDisk(imageForRecognizing.ToStream(ImageFormat.Jpeg), VisualFeature.Description);

                    break;
                case Globals.FacesMode:

                    //ActualFrameSourceObj = ConvertImageToBi(DrawRectangleOnFrame(imageForRecognizing, await _faceRecognizer.AnalyzeImageFromDisk(imageForRecognizing.ToStream(ImageFormat.Jpeg))));

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


