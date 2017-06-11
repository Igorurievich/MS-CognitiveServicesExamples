using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisionApiDemo.Core;
using VisionApiDemo.Core.Enums;

namespace ComputerVisionDemo
{
    public class RecogImageViewModel : ViewModelBase
    {
        private IEnumerable<VisualFeature> _visualFeatures;
        private VisualFeature _selectedVisualFeature;
        private readonly Recognizer _recognizer;
        private ImageSource _imageSourceObject;

        private string _analysisResultText;
        private string _imageUrlString;
        
        private bool _isRecogButtonEnabled;
        
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

        public RecogImageViewModel()
        {
            VisualFeatures = Enum.GetValues(typeof(VisualFeature)).Cast<VisualFeature>();
            _recognizer = new Recognizer("9d5344f8aebf4d76928f4244fa87294a", "https://westeurope.api.cognitive.microsoft.com/vision/v1.0");
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
        }

    }
}
