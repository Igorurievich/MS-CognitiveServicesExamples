using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionApiDemo.Core;
using VisionApiDemo.Core.Enums;

namespace ComputerVisionDemo
{
    ///*https://pp.userapi.com/c836231/v836231496/2ad35/BXrrAm_FzLM.jpg*/
    public class RecogImageViewModel : ViewModelBase
    {
        #region Fields
        private IEnumerable<VisualFeature> _visualFeatures;
        private VisualFeature _selectedVisualFeature;
        private readonly VisionRecognizer _recognizer;
        private ImageSource _imageSourceObject;
        private string _analysisResultText;
        private string _imageUrlString;
        private bool _isRecogButtonEnabled;
        #endregion

        #region  Properties 
        public IEnumerable<VisualFeature> VisualFeatures
        {
            get
            {
                return _visualFeatures;
            }
            set
            {
                _visualFeatures = value;
                NotifyPropertyChanged(nameof(VisualFeatures));
            }
        }
        public VisualFeature SelectedVisualFeature
        {
            get
            {
                return _selectedVisualFeature;
            }
            set
            {
                _selectedVisualFeature = value;
                NotifyPropertyChanged(nameof(SelectedVisualFeature));
            }
        }
        public string AnalysisResultText
        {
            get
            {
                return _analysisResultText;
            }
            set
            {
                _analysisResultText = value;
                NotifyPropertyChanged(nameof(AnalysisResultText));
            }
        }
        public string ImageUrlPath
        {
            get
            {
                return _imageUrlString;
            }
            set
            {
                _imageUrlString = value;
                NotifyPropertyChanged(nameof(ImageUrlPath));
                UrlAddressTextChanged();
            }
        }
        public bool IsRecogButtonEnabled
        {
            get
            {
                return _isRecogButtonEnabled;
            }
            set
            {
                _isRecogButtonEnabled = value;
                NotifyPropertyChanged(nameof(IsRecogButtonEnabled));
            }
        }
        public ImageSource ImageSourceObject
        {
            get { return _imageSourceObject; }
            set
            {
                _imageSourceObject = value;
                NotifyPropertyChanged(nameof(ImageSourceObject));
            }
        }

        #endregion

        public RecogImageViewModel()
        {
            VisualFeatures = Enum.GetValues(typeof(VisualFeature)).Cast<VisualFeature>();
            _recognizer = new VisionRecognizer(TextFileHelper.VisionKey, TextFileHelper.VisionEndpoint);
        }

        public async void AsyncRecognizeImage()
        {
            IsRecogButtonEnabled = false;

            string resultString = await _recognizer.AnalyzeUrlAsync(ImageUrlPath, SelectedVisualFeature);
            AnalysisResultText = resultString;

            IsRecogButtonEnabled = true;
        }

        public void UrlAddressTextChanged()
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(ImageUrlPath, UriKind.Absolute);
            b.EndInit();

            ImageSourceObject = b;

            IsRecogButtonEnabled = ImageSourceObject != null;
        }
    }
}
