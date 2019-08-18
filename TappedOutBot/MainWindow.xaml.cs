using ADB;
using OCR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using TappedOut.Dialog;

namespace TappedOut.Bot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Device emulator;

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

            // create Screenshot object
            var image = Emulator.Screenshot();
            SaveScreenShot(image);
            var filteredImage = Dialog.Dialog.FilterImage((Bitmap)image);
            var screenshot = new Screenshot { Bitmap = (Bitmap)image, ProcessedBitmap = filteredImage };
            Dialog.Dialog.PopulateDialogInfo(screenshot);

            // are there any centre dialogs.
            var centreDialogs = screenshot.Dialogs.Where(d => d.IsCenterDialog).ToList();
            if (!centreDialogs.Any())
            {
                // tap centre to collect XP & $
                Emulator.Tap(Device.TapPosition.Center);
                return;
            }

            var dialog = ChooseDialog(centreDialogs);

            // click on the right middle
            var x = dialog.MaxX - 20;
            var y = dialog.MaxY - (dialog.Height / 2);
            Emulator.Tap(x, y);
        }

        System.Timers.Timer timer;

        private void Timer_Click(object sender, RoutedEventArgs e)
        {
            timer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            System.Diagnostics.Debug.WriteLine("Timer fired" + DateTime.Now.ToLongTimeString());
            Tap_Click(null, null);
            timer.Enabled = true;
        }

        private DialogInfo ChooseDialog(List<DialogInfo> centreDialogs)
        {
            return centreDialogs.Where(x => x.Name.Contains("XP") || x.Name.Contains("Reward") || x.Name.Contains("Time")).FirstOrDefault();
        }

        private void SaveScreenShot(System.Drawing.Image image)
        {
            var filename = @"C:\Users\julian\Desktop\TappedOutScreenShots\screenshot_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            while (File.Exists(filename))
            {
                System.Threading.Thread.Sleep(1000);
                filename = @"C:\Users\julian\Desktop\TappedOutScreenShots\screenshot_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            }
            image.Save(filename);
        }

        private void Screenshot_Click(object sender, RoutedEventArgs e)
        {
            var filename = @"C:\Users\julian\Desktop\TappedOutScreenShots\screenshot_" + Guid.NewGuid().ToString();
            var screenshot = Emulator.Screenshot();
            screenshot.Save(filename + "_0.png");

            var filteredScreenshot = Dialog.Dialog.FilterImage((Bitmap)screenshot);
            filteredScreenshot.Save(filename + "_1.png");

            new Dialog.Dialog().FindQuadrilaterals(filteredScreenshot);
            filteredScreenshot.Save(filename + "_2.png");

            //BitmapImage x

            //this.screenshot.Source = ToBitmapImage(filteredScreenshot);
        }

        private void ScreenshotProcessor_Click(object sender, RoutedEventArgs e)
        {
            new ScreenshotProcessor().ShowDialog();
        }
    }
}