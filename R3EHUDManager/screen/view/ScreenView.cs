using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using System.Drawing;
using System.Diagnostics;
using da2mvc.core.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.view;
using R3EHUDManager.background.view;
using da2mvc.core.injection;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.model;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace R3EHUDManager.screen.view
{
    class ScreenView : Panel, IEventDispatcher
    {
        private Dictionary<string, PlaceholderView> views = new Dictionary<string, PlaceholderView>();
        private const int SCREEN_MARGIN = 30;
        public event EventHandler MvcEventHandler;

        public static Size BASE_RESOLUTION = new Size(1920, 1080);
        public static decimal BASE_ASPECT_RATIO = (decimal)BASE_RESOLUTION.Width / BASE_RESOLUTION.Height;

        public static readonly int EVENT_REQUEST_DESELECTION = EventId.New();
        public static readonly int EVENT_REQUEST_SELECTION = EventId.New();

        private BackgroundView backgroundView;
        private bool isTripleScreen;
        private ZoomLevel zoomLevel = ZoomLevel.FIT_TO_WINDOW;
        private bool inPreviewMode;

        public Rectangle ScreenArea { get => new Rectangle(backgroundView.Location, backgroundView.Size); }

        public ScreenView()
        {
            InitializeUI();
            SetPreviewMode(true);
        }

        internal void DisplayPlaceHolders(PlaceholderModel[] placeHolders)
        {
            UpdateScreenSize();

            foreach (PlaceholderModel model in placeHolders)
            {
                PlaceholderView view = Injector.GetInstance<PlaceholderView>();
                view.Initialize(model, backgroundView.Size, new Point(backgroundView.Location.X, backgroundView.Location.Y), isTripleScreen);
                view.Visible = !inPreviewMode;

                view.Dragging += OnPlaceholderDragging;

                views.Add(model.Name, view);
            }

            Controls.AddRange(views.Values.ToArray());
            backgroundView.SendToBack();
        }

        internal void RemovePlaceholders()
        {
            if (views != null)
            {
                foreach (PlaceholderView placeholder in views.Values)
                {
                    placeholder.Dragging -= OnPlaceholderDragging;

                    Controls.Remove(placeholder);
                    placeholder.Dispose();
                }

                views.Clear();
            }
        }

        internal void BackgroundChanged(ScreenModel screenModel)
        {
            isTripleScreen = screenModel.Layout == ScreenLayoutType.TRIPLE;
            UpdateScreenSize();
            UpdatePlaceholdersPosition();
        }

        internal void TripleScreenChanged(ScreenModel screenModel)
        {
            isTripleScreen = screenModel.Layout == ScreenLayoutType.TRIPLE;
            backgroundView.SetTripleScreen(isTripleScreen);
            UpdateScreenSize();
            UpdatePlaceholdersPosition();
        }

        internal void SetZoomLevel(ZoomLevel zoomLevel)
        {
            if (this.zoomLevel == zoomLevel) return;

            this.zoomLevel = zoomLevel;

            backgroundView.Location = new Point(SCREEN_MARGIN, SCREEN_MARGIN);
            UpdateScreenSize();
            UpdatePlaceholdersPosition();

            if (zoomLevel == ZoomLevel.FIT_TO_WINDOW)
            {
                backgroundView.LocationChanged -= OnScrollChanged;
                VerticalScroll.Value = HorizontalScroll.Value = 0;
                AutoScroll = false;
            }
            else
            {
                backgroundView.LocationChanged += OnScrollChanged;
                AutoScroll = true;
                //TODO Gérer le scroll en manuel pour virer la barre verticale qui peut appraître avec la molette souris sur le position preset.
            }
        }

        internal void PlaceholderSelected()
        {
            SetPreviewMode(false);
        }

        internal void PlaceholderUnselected()
        {
            SetPreviewMode(true);
        }

        private void SetPreviewMode(bool value)
        {
            if (inPreviewMode == value) return;
            inPreviewMode = value;

            foreach (var view in views.Values)
                view.Visible = !inPreviewMode;

            if (value)
            {
                backgroundView.DrawHook = BackgroundDrawHook;
                backgroundView.MouseDown += OnMouseDownInvisible;
            }
            else
            {
                backgroundView.DrawHook = null;
                backgroundView.MouseDown -= OnMouseDownInvisible;
            }

            backgroundView.Invalidate();
        }

        private void OnMouseDownInvisible(object sender, MouseEventArgs e)
        {
            PlaceholderView[] orderedViews = views.Values.OrderBy(x => Controls.GetChildIndex(x)).ToArray();

            foreach (var view in orderedViews)
            {
                if (new Rectangle(view.Location.X - backgroundView.Location.X, view.Location.Y - backgroundView.Location.Y, view.Size.Width, view.Size.Height)
                    .Contains(e.Location))
                {
                    Bitmap bitmap = GraphicalAsset.GetPlaceholderImage(view.Model.Name);
                    float widthRatio = (float)bitmap.Width / view.Width;
                    float heightRatio = (float)bitmap.Height / view.Height;
                    PointF clickPoint = new PointF(widthRatio * (e.X - view.Location.X + backgroundView.Location.X), heightRatio * (e.Y - view.Location.Y + backgroundView.Location.Y));
                    Color pixelColor = bitmap.GetPixel((int)clickPoint.X, (int)clickPoint.Y);
                    
                    if (pixelColor.A == 0) continue;

                    DispatchEvent(new IntEventArgs(EVENT_REQUEST_SELECTION, view.Model.Id));
                    return;
                }
            }
        }

        private void OnScrollChanged(object sender, EventArgs e)
        {
            foreach (PlaceholderView view in views.Values)
                view.OnScreenScrolled(backgroundView.Location);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateScreenSize();
            UpdatePlaceholdersPosition();
            base.OnSizeChanged(e);
        }

        private void UpdatePlaceholdersPosition()
        {
            if (views != null)
                foreach (PlaceholderView view in views.Values)
                {
                    view.SetScreenSize(backgroundView.Size, isTripleScreen, backgroundView.Location);
                }
        }

        private void UpdateScreenSize()
        {
            // When minimizing the window, size is zero.
            if (Width == 0 && Height == 0)
                return;

            Size screenArea = new Size(Width - 2 * SCREEN_MARGIN, Height - 2 * SCREEN_MARGIN);
            backgroundView.SetScreenArea(screenArea, zoomLevel);

            // Center background
            if (zoomLevel == ZoomLevel.FIT_TO_WINDOW)
            {
                Point location = new Point(
                    SCREEN_MARGIN + (screenArea.Width - backgroundView.Width) / 2,
                    SCREEN_MARGIN + (screenArea.Height - backgroundView.Height) / 2
                    );

                backgroundView.Location = location;
            }
            //else backgroundView.Location = new Point(SCREEN_MARGIN, SCREEN_MARGIN);
        }

        private void OnPlaceholderDragging(object sender, EventArgs e)
        {
            // To avoid some artefacts while mouseMove.
            Update();
        }

        private void BackgroundDrawHook(Graphics graphics)
        {
            PlaceholderView[] orderedViews = views.Values.OrderByDescending(x => Controls.GetChildIndex(x)).ToArray();

            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //graphics.CompositingQuality = CompositingQuality.HighQuality;

            // TODO: simplifier 
            foreach (var view in orderedViews)
            {
                var model = view.Model;

                Image originalImage = GraphicalAsset.GetPlaceholderImage(model.Name);
                SizeF newSize = model.ResizeRule.GetSize(BASE_RESOLUTION, backgroundView.Size, originalImage.PhysicalDimension.ToSize(), isTripleScreen);
                newSize = new SizeF(newSize.Width * (float)model.Size.X, newSize.Height * (float)model.Size.Y);

                R3ePoint modelLocation = model.Position.Clone();
                if (isTripleScreen)
                    modelLocation = new R3ePoint(modelLocation.X / 3, modelLocation.Y);

                PointF location = Coordinates.FromR3eF(modelLocation, new Size(backgroundView.Width, backgroundView.Height));
                PointF anchor = Coordinates.FromR3eF(model.Anchor, newSize);
                location = new PointF(location.X - anchor.X, location.Y - anchor.Y);

                graphics.DrawImage(originalImage, new RectangleF(location.X, location.Y, newSize.Width, newSize.Height));
            }
        }

        private void InitializeUI()
        {
            //DoubleBuffered = true;

            BackColor = Colors.SCREEN_BACKGROUND;
            backgroundView = Injector.GetInstance<BackgroundView>();
            backgroundView.Location = new Point(SCREEN_MARGIN, SCREEN_MARGIN);

            Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_REQUEST_DESELECTION));
            backgroundView.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_REQUEST_DESELECTION)); // TODO small bug: mouseDown in item, move out, release => deselection

            Controls.Add(backgroundView);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
