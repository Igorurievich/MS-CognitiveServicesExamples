using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Vision;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;

namespace ComputerVisionDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<AnalysisResult> AnalyzeUrl(string imageUrl)
        {
            string SubscriptionKey = "";
            string APIRoot = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0";

            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey, APIRoot);

            VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
            AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures);
            return analysisResult;
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(@"");
            b.EndInit();

            var image = sender as Image;            
            image.Source = b;
        }

        private async void RecognizeAsyncClick(object sender, RoutedEventArgs e)
        {
            runAsyncRecog.IsEnabled = false;

            var slowTask = await AnalyzeUrl(@"https://pp.userapi.com/c836231/v836231496/2cc31/7zjQanEbu7Q.jpg");

            ImagesRichTextBox.AppendText(slowTask.Faces.ToString());

            runAsyncRecog.IsEnabled = true;
        }
    }
}
