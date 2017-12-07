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
            // TODO use LocationModel
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
                    }
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
