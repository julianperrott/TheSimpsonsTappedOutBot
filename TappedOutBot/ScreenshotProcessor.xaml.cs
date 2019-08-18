using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Drawing;
using TappedOutDialog;
using OCR;
using AForge.Imaging.Filters;

namespace TappedOutBot
{
    /// <summary>
    /// Interaction logic for ScreenshotProcessor.xaml
    /// </summary>
    public partial class ScreenshotProcessor : Window, INotifyPropertyChanged
    {
        public List<Screenshot> Screenshots;
        public int selectedIndex = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public Screenshot SelectedScreenshot
        {
            get
            {
                var screenshot = this.Screenshots[selectedIndex];

                if (screenshot.Dialogs == null)
                {
                    var dialog = new Dialog();
                    var cornerPoints = dialog.FindQuadrilaterals(screenshot.ProcessedBitmap);
                    var extractedDialogs = Dialog.ExtractDialogs(screenshot.Bitmap, cornerPoints);

                    screenshot.Dialogs = extractedDialogs.Select(bmp => new DialogInfo
                    {
                        Image = MainWindow.ToBitmapImage(bmp),
                        Name = ExtractText(bmp),
                        Width = bmp.Width,
                        Height = bmp.Height
                    }).ToList();
                }

                return screenshot;
            }
        }

        private static string ExtractText(Bitmap bmp)
        {
            // create filter
            var resizeBilinear = new ResizeBilinear(bmp.Width*2, bmp.Height*2);
            var greyScale = new Grayscale(0.2125, 0.7154, 0.0721);

            var biggerImage = new FiltersSequence(resizeBilinear).Apply(bmp);

            return string.Join(", ", TesseractTestBase.ExtractText(biggerImage));
        }

        public ScreenshotProcessor()
        {
            InitializeComponent();
            this.DataContext = this;


            Screenshots = Directory.GetFiles(@"C:\Users\julian\Desktop\TappedOutScreenShots", "*_0.png")
                .ToList()
                .Select(f =>
                {
                    var image =System.Drawing.Image.FromFile(f);
                    var processedBitmap = Dialog.FilterImage((Bitmap)image);
                    
                    return new Screenshot { Filename = f.Split('\\').ToList().Last(), Bitmap = (Bitmap)image, ProcessedBitmap = processedBitmap};

                })
                .ToList();

            selectedIndex = 0;
            OnPropertyChanged("SelectedScreenshot");
        }

        private void Screenshots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnPropertyChanged("SelectedScreenshot");
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            selectedIndex--;
            if (selectedIndex <0)
            {
                selectedIndex = this.Screenshots.Count-1;
            }

            OnPropertyChanged("SelectedScreenshot");
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            selectedIndex++;
            if (selectedIndex >= this.Screenshots.Count)
            {
                selectedIndex = 0;
            }
            OnPropertyChanged("SelectedScreenshot");
        }
    }
}
