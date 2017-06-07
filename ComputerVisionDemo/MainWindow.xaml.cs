using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Vision;
using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using VisionApiDemo.Core;

namespace ComputerVisionDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //https://pp.userapi.com/c836231/v836231496/2ad35/BXrrAm_FzLM.jpg
        private readonly Recognizer _recognizer;

        public MainWindow()
        {
            InitializeComponent();

            VisualFeautures.ItemsSource = Enum.GetValues(typeof(VisualFeature));
        }

        private async void RecognizeAsyncButtonClick(object sender, RoutedEventArgs e)
        { 
            RunAsyncRecogButton.IsEnabled = false;

            AnalysisResult analysisResult = await _recognizer.AnalyzeUrl(ImageUrlTextBox.Text, new[] { (VisualFeature)VisualFeautures.SelectedItem});
            ImagesRichTextBox.AppendText(AnalisysHelper.GetInfo(analysisResult, (VisualFeature)VisualFeautures.SelectedItem));

            RunAsyncRecogButton.IsEnabled = true;
        }

        private void ChooseFolderButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ImageUrlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(ImageUrlTextBox.Text, UriKind.Absolute);
            b.EndInit();
           
            ImageContainer.Source = b;
        }
    }
}
