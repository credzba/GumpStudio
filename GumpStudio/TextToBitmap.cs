using System;
using System.Drawing;

namespace GumpStudio
{
    public class Utility
    {
        public static Bitmap TextToBitmap(string text, Font font = null, Color? textColor = null, Color? backgroundColor = null)
        {
            // Check if the text is null or empty and return a blank bitmap
            if (string.IsNullOrEmpty(text))
            {
                return new Bitmap(1, 1);
            }
            // Set default values if not provided
            font ??= new Font("Arial", 12);
            textColor ??= Color.Black;
            backgroundColor ??= Color.White;

            // Measure the string to create an appropriately sized bitmap
            using (var tempBitmap = new Bitmap(1, 1))
            using (var tempGraphics = Graphics.FromImage(tempBitmap))
            {
                var textSize = tempGraphics.MeasureString(text, font);

                // Create the final bitmap with the measured size
                var bitmap = new Bitmap((int)Math.Ceiling(textSize.Width), (int)Math.Ceiling(textSize.Height));

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    // Set up the graphics object
                    graphics.Clear(backgroundColor.Value);
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    graphics.DrawString(text, font, new SolidBrush(textColor.Value), 0, 0);
                }

                return bitmap;
            }
        }
    }
}
