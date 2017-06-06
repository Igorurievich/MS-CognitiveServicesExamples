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

            VisionServiceClient visionServiceClient = new VisionServiceClient(SubscriptionKey, APIRoot);

            VisualFeature[] visualFeatures = {VisualFeature.Description};
            AnalysisResult analysisResult = await visionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures);
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

        private async void RecognizeAsyncButtonClick(object sender, RoutedEventArgs e)
        {
            RunAsyncRecogButton.IsEnabled = false;

            var slowTask = await AnalyzeUrl(@"");

            ImagesRichTextBox.AppendText(slowTask.Description.Captions[0].Text);

            RunAsyncRecogButton.IsEnabled = true;
        }

        private void ChooseFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
