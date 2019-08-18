
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace OCR
{
    public class TesseractTestBase
    {
        protected static TesseractEngine CreateEngine(string lang = "eng", EngineMode mode = EngineMode.Default)
        {
            return new TesseractEngine(@"./tessdata", lang, mode);
        }

        protected static string TestFilePath(string path)
        {
            return Path.Combine(@"C:\Users\julian\Desktop\tesseract-master\src\Tesseract.Tests\Data", path);
        }

        public static void Test()
        {
            using (var engine = CreateEngine())
            {
                for (int i = 1; i < 5; i++)
                {
                    //var inputFilename = TestFilePath(@"Ocr\uzn-test.png");
                    var inputFilename = @"C:\Users\julian\Desktop\OCR" + i + ".png";

                    var items = new List<string>();
                    ExtractText(engine, inputFilename, items);


                    var bitmap=(Bitmap)Image.FromFile(inputFilename);
                    ExtractText(bitmap);
                }
            }
        }

        private static void ExtractText(TesseractEngine engine, string inputFilename, List<string> items)
        {
            System.Diagnostics.Debug.WriteLine("--------------------------------");
            System.Diagnostics.Debug.WriteLine(inputFilename);

            using (var img = Pix.LoadFromFile(inputFilename))
            {
                items.AddRange(GetText(engine, img));
            }
        }

        private static List<string> GetText(TesseractEngine engine, Pix img)
        {
            var items = new List<string>();
            using (var page = engine.Process(img))
            {
                using (var iter = page.GetIterator())
                {
                    iter.Begin();
                    do
                    {
                        var text = iter.GetText(PageIteratorLevel.TextLine);
                        if (text != null) { items.Add(text.Trim()); }
                        System.Diagnostics.Debug.WriteLine(text);
                    } while (iter.Next(PageIteratorLevel.TextLine));
                }
            }

            return items;
        }

        public static List<string> ExtractText(Bitmap bitmap)
        {
            byte[] byteArray;
            using (MemoryStream byteStream = new MemoryStream())
            {
                bitmap.Save(byteStream, System.Drawing.Imaging.ImageFormat.Tiff);
                byteStream.Close();
                byteArray = byteStream.ToArray();
            }

            using (var engine = CreateEngine())
            {
                using (var img = Pix.LoadTiffFromMemory(byteArray))
                {
                    return GetText(engine, img);
                }
            }
        }
    }
}
