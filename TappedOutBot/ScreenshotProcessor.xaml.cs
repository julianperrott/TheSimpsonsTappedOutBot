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
using OCR;
using AForge.Imaging.Filters;
using TappedOut.Dialog;
using TappedOut.Bot.ViewModels;

namespace TappedOut.Bot
{
    public partial class ScreenshotProcessor : Window, INotifyPropertyChanged
    {
        public List<Screenshot> Screenshots;
        public int selectedIndex = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage Image
        {
            get
            {
                return Screenshots[selectedIndex].Bitmap.ToBitmapImage();
            }
        }

        public BitmapImage ProcessedImage
        {
            get
            {
                return Screenshots[selectedIndex].ProcessedBitmap.ToBitmapImage();
            }
        }

        public List<DialogInfoViewModel> Dialogs
        {
            get
            {
                var screenshot = Screenshots[selectedIndex];
                if (screenshot.Dialogs == null)
                {
                    Dialog.Dialog.PopulateDialogInfo(screenshot);
                }

                return screenshot.Dialogs.Select(d => DialogInfoViewModel.Create(d)).ToList();
            }
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
                    var processedBitmap = Dialog.Dialog.FilterImage((Bitmap)image);
                    
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
