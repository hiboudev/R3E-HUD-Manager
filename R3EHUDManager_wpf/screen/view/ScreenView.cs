using da2mvc.core.events;
using da2mvc.core.injection;
using da2mvc.core.view;
using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using R3EHUDManager_wpf.application.view;
using R3EHUDManager_wpf.placeholder.view;
using R3EHUDManager_wpf.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R3EHUDManager_wpf.screen.view
{
    public class ScreenView : FrameworkElement, IView, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_SCREEN_AREA_CHANGED = EventId.New();
        public event EventHandler ScreenAreaChanged;
        public event EventHandler Disposed;

        private VisualCollection views;
        private bool isTripleScreen;
        public static Size BASE_RESOLUTION = new Size(1920, 1080);
        private BitmapSource background;
        private ZoomLevel zoomLevel = ZoomLevel.FIT_TO_WINDOW;
        public const double MARGIN = 30;
        private Rect screenArea;
        private double scrollValue = 0;

        public Rect ScreenArea { get => screenArea; }

        public ScreenView(ScreenViewMouseInteraction mouseInteraction)
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            ClipToBounds = true;

            views = new VisualCollection(this);
            mouseInteraction.Initialize(this);
        }

        internal void DisplayPlaceHolders(PlaceholderModel[] models)
        {
            foreach (var model in models)
            {
                PlaceholderView view = Injector.GetInstance<PlaceholderView>();
                view.Initialize(model, isTripleScreen);
                views.Add(view);
            }
            InvalidateVisual();
        }

        internal void RemovePlaceholders()
        {
            foreach (PlaceholderView view in views)
                view.Dispose();

            views.Clear();
            InvalidateVisual();
        }

        internal void PlaceholderSelected(PlaceholderModel model)
        {
            PlaceholderView selectedView = null;

            foreach (PlaceholderView view in views)
            {
                view.ShowDecoration = true;
                if (view.Model.Id == model.Id)
                    selectedView = view;
            }

            BringToFront(selectedView);
        }

        internal void SetZoomLevel(ZoomLevel zoomLevel)
        {
            if (this.zoomLevel == zoomLevel) return;

            this.zoomLevel = zoomLevel;

            InvalidateVisual();
        }

        internal void Scroll(double scrollValue)
        {
            this.scrollValue = scrollValue;
            InvalidateVisual();
        }

        private void BringToFront(PlaceholderView view)
        {
            views.Remove(view);
            views.Add(view);
        }

        internal void PlaceholderUnselected()
        {
            foreach (PlaceholderView view in views)
                view.ShowDecoration = false;
        }

        internal void BackgroundChanged(ScreenModel screenModel)
        {
            background = screenModel.GetBackgroundImage();
            SetTripleScreen(screenModel.Layout == ScreenLayoutType.TRIPLE);
            InvalidateVisual();
        }

        private void SetTripleScreen(bool isTripleScreen)
        {
            this.isTripleScreen = isTripleScreen;
            foreach (PlaceholderView view in views)
                view.IsTripleScreen = isTripleScreen;
        }

        internal PlaceholderView GetViewUnder(Point mousePosition)
        {
            for (int index = views.Count - 1; index >= 0; index--)
            {
                PlaceholderView view = (PlaceholderView)views[index];

                Rect bounds = view.ContentBounds;
                if (!bounds.Contains(mousePosition)) continue;

                // For track map we don't go through the transparent pixels cause it's too hard to select it.
                if (view.ShowDecoration || view.Model.Name == PlaceholderName.TRACK_MAP)
                    return view;

                // Transparent pixel detection
                BitmapSource bitmap = GraphicalAsset.GetPlaceholderImage(view.Model.Name);
                double widthRatio = bitmap.Width / bounds.Width;
                double heightRatio = bitmap.Height / bounds.Height;
                Point clickPoint = new Point(widthRatio * (mousePosition.X - bounds.X), heightRatio * (mousePosition.Y - bounds.Y));
                Color pixel = BitmapUtils.GetPixel(bitmap, (int)clickPoint.X, (int)clickPoint.Y);

                if (pixel.A > 0)
                    return view;
            }

            return null;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (background != null)
                DrawBackground(drawingContext);
            else
                screenArea = new Rect(new Point(), RenderSize);

            ScreenAreaChanged?.Invoke(this, new BaseEventArgs(EVENT_SCREEN_AREA_CHANGED));

            foreach (PlaceholderView view in views)
            {
                view.ScreenArea = screenArea;
                view.Render();
            }
        }

        private void DrawBackground(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(new SolidColorBrush(AppColors.SCREEN_BACKGROUND), null, new Rect(new Point(), RenderSize));

            Size renderSize;
            if (RenderSize.Width < 2 * MARGIN || RenderSize.Height < 2 * MARGIN)
                renderSize = new Size();
            else
                renderSize = new Size(RenderSize.Width - 2 * MARGIN, RenderSize.Height - 2 * MARGIN);

            Size backgroundSize = renderSize;
            Point backgroundPosition = new Point();
            double bitmapRatio = (double)background.PixelWidth / background.PixelHeight;

            if (zoomLevel == ZoomLevel.FIT_TO_WINDOW)
            {
                double screenRatio = renderSize.Width / renderSize.Height;

                if (screenRatio < bitmapRatio)
                {
                    backgroundSize.Width = renderSize.Width;
                    backgroundSize.Height = renderSize.Width / bitmapRatio;
                }
                else
                {
                    backgroundSize.Height = renderSize.Height;
                    backgroundSize.Width = renderSize.Height * bitmapRatio;
                }

                backgroundPosition = new Point(
                   MARGIN + (renderSize.Width - backgroundSize.Width) / 2,
                   MARGIN + (renderSize.Height - backgroundSize.Height) / 2
               );
            }
            else if (zoomLevel == ZoomLevel.FIT_TO_HEIGHT)
            {
                backgroundSize.Height = renderSize.Height;
                backgroundSize.Width = renderSize.Height * bitmapRatio;

                // Scroll
                double hOffset = 0;
                double fullWidth = 2 * MARGIN + backgroundSize.Width;
                double scrollableWidth = fullWidth - RenderSize.Width;

                if (scrollValue > scrollableWidth)
                    if (scrollableWidth <= 0)
                        hOffset = MARGIN + (renderSize.Width - backgroundSize.Width) / 2;
                    else
                        hOffset = MARGIN - scrollableWidth;
                else
                    hOffset = MARGIN - scrollValue;

                backgroundPosition = new Point(hOffset, MARGIN);
            }

            drawingContext.DrawImage(background, new Rect(backgroundPosition, backgroundSize));

            screenArea = new Rect(backgroundPosition, backgroundSize);
        }

        protected override int VisualChildrenCount
        {
            get { return views.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= views.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return views[index];
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
