using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.graphics
{
    class GraphicalAsset
    {
        private static Dictionary<string, Bitmap> cache = new Dictionary<string, Bitmap>();
        private static Bitmap defaultBitmap;

        public static Bitmap GetPlaceholderImage (string placeholderName)
        {
            switch (placeholderName)
            {
                case "MoTeC":
                    return GetBitmap(@"_graphical_assets\motec.png");

                case "Track Map":
                    return GetBitmap(@"_graphical_assets\trackmap.png");

                case "FFB Graph":
                    return GetBitmap(@"_graphical_assets\ffbgraph.png");

                case "Position Bar":
                    return GetBitmap(@"_graphical_assets\positionbar.png");

                case "Virtual Mirror":
                    return GetBitmap(@"_graphical_assets\virtualmirror.png");

                default:
                    if(defaultBitmap == null)
                    {
                        defaultBitmap = new Bitmap(200, 100);
                        using (Graphics graph = Graphics.FromImage(defaultBitmap))
                        {
                            Rectangle ImageSize = new Rectangle(0, 0, defaultBitmap.Width, defaultBitmap.Height);
                            graph.FillRectangle(Brushes.Orange, ImageSize);
                        }
                    }
                    return defaultBitmap;
            }
        }

        public static Image GetBackground()
        {
            // TODO Jpg
            return GetBitmap(@"_graphical_assets\background.png");
        }

        private static Bitmap GetBitmap(string path)
        {
            if (cache.ContainsKey(path))
                return cache[path];

            Bitmap bitmap = new Bitmap(path);
            cache.Add(path, bitmap);

            return bitmap;
        }
    }
}
