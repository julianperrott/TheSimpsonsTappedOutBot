using ADB;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using OCR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

            TesseractTestBase.Test();
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
            var filename = @"C:\Users\julian\Desktop\TappedOutScreenShots\screenshot_" + Guid.NewGuid().ToString();
            var screenshot = Emulator.Screenshot();
            screenshot.Save(filename + "_0.png");

            var filteredScreenshot = Dialog.FilterImage((Bitmap)screenshot);
            filteredScreenshot.Save(filename + "_1.png");

            new Dialog().FindQuadrilaterals(filteredScreenshot);
            filteredScreenshot.Save(filename + "_2.png");

            this.screenshot.Source = ToBitmapImage(filteredScreenshot);
        }
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            byte[] buffer = ms.GetBuffer();
            var bufferPasser = new MemoryStream(buffer);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = bufferPasser;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        private void ScreenshotProcessor_Click(object sender, RoutedEventArgs e)
        {
            new ScreenshotProcessor().ShowDialog();
        }
    }
}
