using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.motec.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R3EHUDManager.graphics
{
    public class GraphicalAssetFactory:IEventDispatcher
    {
        // TODO error management loading

        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_MOTEC_CHANGED = EventId.New();
        private Dictionary<string, BitmapSource> cache = new Dictionary<string, BitmapSource>();
        private BitmapSource defaultBitmap;
        private MotecModel _motec;

        public void SetMotec(MotecModel motecModel)
        {
            _motec = motecModel;
            DispatchEvent(new IntEventArgs(EVENT_MOTEC_CHANGED, _motec.Id));
        }

        public BitmapSource GetPlaceholderImage(string placeholderName)
        {
            // TODO use LocationModel
            switch (placeholderName)
            {
                case PlaceholderName.MOTEC:
                    return GetBitmap(_motec.FilePath);

                case PlaceholderName.TRACK_MAP:
                    return GetBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"_graphical_assets\trackmap.png"));

                case PlaceholderName.FFB_GRAPH:
                    return GetBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"_graphical_assets\ffbgraph.png"));

                case PlaceholderName.POSITION_BAR:
                    return GetBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"_graphical_assets\positionbar.png"));

                case PlaceholderName.VIRTUAL_MIRROR:
                    return GetBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"_graphical_assets\virtualmirror.png"));

                case PlaceholderName.MINI_MOTEC:
                    return GetBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"_graphical_assets\minimotec.png"));

                default:
                    if (defaultBitmap == null)
                        defaultBitmap = CreateBlankBitmap(200, 200);
                    return defaultBitmap;
            }
        }

        public Size GetPlaceholderSize(string placeholderName)
        {
            BitmapSource image = GetPlaceholderImage(placeholderName);
            return new Size(image.Width, image.Height);
        }

        public BitmapSource GetNoCache(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();
            return image;
        }

        public ImageSource GetPreferencesIcon()
        {
            return GetBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"_graphical_assets\preference_icon4_22_alpha85.png"));
        }

        public ImageSource GetLayoutIcon(ScreenLayoutType layout)
        {
            string cacheKey = layout == ScreenLayoutType.SINGLE ? "ScreenLayoutType.SINGLE" : "ScreenLayoutType.TRIPLE";

            if (!cache.ContainsKey(cacheKey))
                cache.Add(cacheKey, GetScreenLayoutIcon(layout));

            return cache[cacheKey];
        }

        private BitmapSource GetScreenLayoutIcon(ScreenLayoutType layout)
        {
            string text = layout == ScreenLayoutType.SINGLE ? "S" : "T";

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            Typeface typeface = new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Normal, new FontStretch());
            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 13, new SolidColorBrush(Colors.DarkGray));
            drawingContext.DrawText(formattedText, new Point(4, 2));
            drawingContext.Close();

            RenderTargetBitmap image = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            image.Render(drawingVisual);

            return image;
        }

        private BitmapSource GetBitmap(string path)
        {
            if (cache.ContainsKey(path))
                return cache[path];

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();

            cache.Add(path, image);

            return image;
        }

        private BitmapSource CreateBlankBitmap(int width, int height)
        {
            List<Color> colors = new List<Color>();
            colors.Add(Colors.Black);

            BitmapPalette palette = new BitmapPalette(colors);

            int stride = width * 4;
            byte[] pixels = new byte[height * stride];

            return BitmapSource.Create(200, 200, 96, 96, PixelFormats.Pbgra32, palette, pixels, stride);

            //var visual = new DrawingVisual();
            //using (var context = visual.RenderOpen())
            //{
            //    Typeface typeface = new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Bold, new FontStretch());
            //    context.DrawText(new FormattedText("Missing bitmap", CultureInfo.DefaultThreadCurrentCulture, FlowDirection.LeftToRight, typeface, 16, new SolidColorBrush(Colors.OrangeRed)), new Point(10, 100));
            //}
            //RenderTargetBitmap bitmap = new RenderTargetBitmap(200, 200, 96, 96, PixelFormats.Default);
            //bitmap.Render(visual);
            //return bitmap;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
