using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace TappedOut.Dialog
{
    public class Screenshot
    {
        public Bitmap Bitmap { get; set; }

        public Bitmap ProcessedBitmap { get; set; }

        //public BitmapImage Image
        //{
        //    get
        //    {
        //        return ToBitmapImage(Bitmap);
        //    }
        //}

        public string Filename { get; set; }

        //public BitmapImage ProcessedImage
        //{
        //    get
        //    {
        //        return ToBitmapImage(ProcessedBitmap);
        //    }
        //}

        public List<DialogInfo> Dialogs { get; set; }

        //public static BitmapImage ToBitmapImage(Bitmap bitmap)
        //{
        //    var ms = new MemoryStream();
        //    bitmap.Save(ms, ImageFormat.Bmp);
        //    byte[] buffer = ms.GetBuffer();
        //    var bufferPasser = new MemoryStream(buffer);

        //    var bitmapImage = new BitmapImage();
        //    bitmapImage.BeginInit();
        //    bitmapImage.StreamSource = bufferPasser;
        //    bitmapImage.EndInit();

        //    return bitmapImage;
        //}


    }

}
