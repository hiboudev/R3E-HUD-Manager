using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R3EHUDManager_wpf.utils
{
    class BitmapUtils
    {
        static public Color GetPixel(BitmapSource bitmap, int X, int Y)
        {
            if (X < 0 || Y < 0 || X > bitmap.PixelWidth || Y > bitmap.PixelHeight)
            {
                Debug.WriteLine("Pixel outside of image.");
                return Color.FromArgb(0, 0, 0, 0);
            }

            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;

            byte[] pixels = new byte[size];

            bitmap.CopyPixels(pixels, stride, 0);

            int pixelIndex = Y * stride + 4 * X;

            return Color.FromArgb(
                pixels[pixelIndex + 3],
                pixels[pixelIndex + 2],
                pixels[pixelIndex + 1],
                pixels[pixelIndex]
                );
        }
    }
}
