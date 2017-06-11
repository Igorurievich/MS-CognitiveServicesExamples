using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using VisionApiDemo.Core;
using VisionApiDemo.Core.Enums;

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
            
            //VisualFeautures.ItemsSource = Enum.GetValues(typeof(VisualFeature));
        }

        private async void RecognizeAsyncButtonClick(object sender, RoutedEventArgs e)
        { 
            RunAsyncRecogButton.IsEnabled = false;

            //string resultString = await _recognizer.AnalyzeUrlAsync(ImageUrlTextBox.Text, new[] { (VisualFeature)VisualFeautures.SelectedItem});
            //ImagesRichTextBox.AppendText(resultString);

            RunAsyncRecogButton.IsEnabled = true;
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
