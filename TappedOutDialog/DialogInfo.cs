using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;


namespace TappedOut.Dialog
{
    public class DialogInfo
    {
        public Bitmap Bitmap { get; set; }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsCenterDialog { get; set; }

        public int MinX { get; set; }

        public int MaxX { get; set; }

        public int MinY { get; set; }

        public int MaxY { get; set; }
    }
}
