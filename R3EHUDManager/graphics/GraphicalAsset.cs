using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
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
        // TODO error management loading

        private static Dictionary<string, Bitmap> cache = new Dictionary<string, Bitmap>();
        private static Bitmap defaultBitmap;

        public static Bitmap GetPlaceholderImage (string placeholderName)
        {
            // TODO use LocationModel
            switch (placeholderName)
            {
                case PlaceholderName.MOTEC:
                    return GetBitmap(@"_graphical_assets\motec.png");

                case PlaceholderName.TRACK_MAP:
                    return GetBitmap(@"_graphical_assets\trackmap.png");

                case PlaceholderName.FFB_GRAPH:
                    return GetBitmap(@"_graphical_assets\ffbgraph.png");

                case PlaceholderName.POSITION_BAR:
                    return GetBitmap(@"_graphical_assets\positionbar.png");

                case PlaceholderName.VIRTUAL_MIRROR:
                    return GetBitmap(@"_graphical_assets\virtualmirror.png");

                case PlaceholderName.MINI_MOTEC:
                    return GetBitmap(@"_graphical_assets\minimotec.png");

                default:
                    if(defaultBitmap == null)
                        defaultBitmap = new Bitmap(200, 100);
                    return defaultBitmap;
            }
        }

        public static Size GetPlaceholderSize(string placeholderName)
        {
            return GetPlaceholderImage(placeholderName).PhysicalDimension.ToSize();
        }

        public static Image GetNoCache(string path)
        {
            return new Bitmap(path);
        }

        public static Image GetPreferencesIcon()
        {
            return GetBitmap(@"_graphical_assets\preference_icon4_22_alpha85.png");
        }

        public static Image GetLayoutIcon(ScreenLayoutType layout)
        {
            string cacheKey = layout == ScreenLayoutType.SINGLE ? "ScreenLayoutType.SINGLE" : "ScreenLayoutType.TRIPLE";

            if (!cache.ContainsKey(cacheKey))
                cache.Add(cacheKey, GetScreenLayoutIcon(layout));

            return cache[cacheKey];
        }

        private static Bitmap GetScreenLayoutIcon(ScreenLayoutType layout)
        {
            string text = layout == ScreenLayoutType.SINGLE ? "S" : "T";

            Bitmap image = new Bitmap(16, 16);
            Graphics graphics = Graphics.FromImage(image);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphics.DrawString(text, new Font(FontFamily.GenericSansSerif, 8), new SolidBrush(Color.DarkGray), new Point(2, 2));
            graphics.Dispose();

            return image;
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
