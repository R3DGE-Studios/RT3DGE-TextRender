using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TTFPixelExtractor
{
    public class TTFPixelExtractor
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        private const uint FR_PRIVATE = 0x10;

        public static byte[] GetPixelData(string fontFilePath, string text, int fontSize, int width, int height)
        {
            // Load the font file
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(fontFilePath);

            // Select the first font in the collection
            FontFamily fontFamily = fontCollection.Families[0];

            // Create a font object
            Font font = new Font(fontFamily, fontSize);

            // Create a bitmap to draw the text onto
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);

            // Draw text using the font onto the bitmap
            graphics.DrawString(text, font, Brushes.Black, new PointF(0, 0));

            // Access individual pixel data
            byte[] pixelData = new byte[width * height * 3]; // Assuming RGB format
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    pixelData[index++] = pixelColor.R;
                    pixelData[index++] = pixelColor.G;
                    pixelData[index++] = pixelColor.B;
                }
            }

            // Dispose resources
            graphics.Dispose();
            bitmap.Dispose();
            font.Dispose();

            return pixelData;
        }
    }
}
