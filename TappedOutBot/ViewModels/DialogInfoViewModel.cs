using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media.Imaging;
using TappedOut.Dialog;

namespace TappedOut.Bot.ViewModels
{
    public class DialogInfoViewModel
    {
        public static DialogInfoViewModel Create(DialogInfo dialog)
        {
            var dialogInfo = new DialogInfoViewModel();
            dialogInfo.Bitmap = dialog.Bitmap;
            dialogInfo.Name = dialog.Name;
            dialogInfo.Width = dialog.Width;
            dialogInfo.Height = dialog.Height;
            dialogInfo.IsCenterDialog = dialog.IsCenterDialog;
            return dialogInfo;
        }

        private DialogInfoViewModel() { }

        public BitmapImage Image
        {
            get
            {
                return Bitmap.ToBitmapImage();
            }
        }

        public Bitmap Bitmap { get; set; }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsCenterDialog { get; set; }
    }
}
