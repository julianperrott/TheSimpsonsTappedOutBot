using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media.Imaging;
using TappedOutDialog;

namespace TappedOutBot
{
    public class Screenshot
    {
        public Bitmap Bitmap { get; set; }

        public Bitmap ProcessedBitmap { get; set; }

        public BitmapImage Image
        {
            get
            {
                return MainWindow.ToBitmapImage(Bitmap);
            }
        }

        public  string Filename { get;set; }

        public BitmapImage ProcessedImage
        {
            get
            {
                return MainWindow.ToBitmapImage(ProcessedBitmap);
            }
        }

        public List<DialogInfo> Dialogs { get; set; }
}

    public class DialogInfo
    {
        public BitmapImage Image { get; set; }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
