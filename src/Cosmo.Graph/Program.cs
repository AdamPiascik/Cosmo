using System;
using SkiaSharp;

namespace Cosmo.Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Import all data into lists of classes
            For each graph in config file
                - Plot axes
                - Plot axis labels
                - Plot data (line graph with bullets)
                - Plot title
                - Save as png */

            // create the bitmap that will hold the pixels
            var bitmap = new SKBitmap(256, 256);

            // create the document
            var stream = SKFileWStream.OpenStream("document.pdf");
            var document = SKDocument.CreatePdf(stream);

            // get the canvas from the page
            var canvas = document.BeginPage(256, 256);

            // clear the canvas, so that it is fresh
            canvas.Clear(SKColors.Transparent);

            var paint = new SKPaint
            {
                IsAntialias = true,                               // smooth text
                TextSize = 50,                                    // 50px high text
                TextAlign = SKTextAlign.Center,                   // center the text
                Color = 0xFF3498DB,                               // Xamarin light blue text
                Style = SKPaintStyle.Fill,                        // solid text
                Typeface = SKTypeface.FromFamilyName("Trebuchet") // use the Trebuchet typeface
            };

            // clear the canvas, just in case we are running this a second time
            canvas.Clear(SKColors.Transparent);

            // draw the text using the paint
            canvas.DrawText("SkiaSharp", 128, 128 + (paint.TextSize / 2), paint);

            // end the page and document
            document.EndPage();
            document.Close();
        }
    }
}
