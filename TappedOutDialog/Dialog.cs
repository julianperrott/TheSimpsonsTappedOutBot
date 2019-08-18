using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TappedOutDialog
{
    public class Dialog
    {
        public List<List<System.Drawing.Point>> FindQuadrilaterals(Bitmap image)
        {
            var result = new List<List<System.Drawing.Point>>();

            // locating objects
            var blobCounter = new BlobCounter(image);

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 20;
            blobCounter.MinWidth = 20;

            blobCounter.ProcessImage(image);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // check for rectangles
            var shapeChecker = new SimpleShapeChecker();

            foreach (var blob in blobs)
            {
                var edgePoints = blobCounter.GetBlobsEdgePoints(blob);

                // use the shape checker to extract the corner points
                if (shapeChecker.IsQuadrilateral(edgePoints, out List<IntPoint> cornerPoints))
                {
                    var points = cornerPoints.Select(point => new System.Drawing.Point(point.X, point.Y)).ToList();
                    result.Add(points);
                    Graphics g = Graphics.FromImage(image);
                    g.DrawPolygon(new Pen(Color.Red, 1.0f), points.ToArray());
                }
            }

            return result;
        }

        public static Bitmap FilterImage(Bitmap originalBitmap)
        {
            var bitmap = new Bitmap(originalBitmap);

            var imagem = bitmap.Clone(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var colorFilter = new ColorFiltering();
            colorFilter.FillColor = new RGB(255, 255, 255);
            colorFilter.Red = colorFilter.Green = colorFilter.Blue = new IntRange(0, 40);
            colorFilter.ApplyInPlace(bitmap);
            bitmap = new FiltersSequence(colorFilter, new Invert(), new Erosion()).Apply(imagem);
            return bitmap;
        }

        public static List<Bitmap> ExtractDialogs(Bitmap bitmap, List<List<System.Drawing.Point>> cornerPoints)
        {
            return cornerPoints.Select(points => CopyFromBitmap(bitmap, points)).ToList();
        }

        public static Bitmap CopyFromBitmap(Bitmap originalBitmap, List<System.Drawing.Point> points)
        {
            var bitmap = new Bitmap(originalBitmap);

            var minX = points.Select(p => p.X).Min();
            var maxX = points.Select(p => p.X).Max();

            var minY = points.Select(p => p.Y).Min();
            var maxY = points.Select(p => p.Y).Max();

            var bmpImage = new Bitmap(bitmap);
            return bmpImage.Clone(new Rectangle(minX, minY, maxX - minX, maxY - minY), bmpImage.PixelFormat);
        }
    }
}
