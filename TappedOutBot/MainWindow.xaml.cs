using ADB;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TappedOutDialog;

namespace TappedOutBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Device emulator;

        public MainWindow()
        {
            InitializeComponent();
        }

        public Device Emulator
        {
            get
            {
                if (this.emulator == null)
                {
                    var id = Device.GetDevices().FirstOrDefault();
                    emulator = Device.Create(id);
                }
                return emulator;
            }
        }

        private void Tap_Click(object sender, RoutedEventArgs e)
        {
            Emulator.Tap(Device.TapPosition.Character);
            System.Threading.Thread.Sleep(100);
            Emulator.Tap(Device.TapPosition.Center);
        }

        private void Screenshot_Click(object sender, RoutedEventArgs e)
        {
            var screenshot = Emulator.Screenshot();
            var filteredScreenshot = FilterImage(screenshot);
            var processedScreenshot = new Dialog().Process(filteredScreenshot);

            var filename = @"C:\Users\julian\Desktop\screenshot" + Guid.NewGuid() + ".png";
            processedScreenshot.Save(filename);

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            this.screenshot.Source = bi;
        }

        private static Bitmap FilterImage(System.Drawing.Image screenshot)
        {
            var bitmap = (Bitmap)screenshot;
            var imagem = bitmap.Clone(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var colorFilter = new ColorFiltering();
            colorFilter.FillColor = new RGB(255, 255, 255);
            colorFilter.Red = colorFilter.Green = colorFilter.Blue = new IntRange(0, 40);
            colorFilter.ApplyInPlace(bitmap);
            bitmap = new FiltersSequence(colorFilter, new Invert(), new Erosion()).Apply(imagem);
            return bitmap;
        }

    }
}
