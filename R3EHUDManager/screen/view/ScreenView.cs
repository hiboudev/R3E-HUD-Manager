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

namespace R3EHUDManager.screen.view
{
    class ScreenView : Panel, IEventDispatcher
    {
        private Dictionary<string, PlaceholderView> views = new Dictionary<string, PlaceholderView>();
        private const int SCREEN_MARGIN = 30;
        public event EventHandler MvcEventHandler;
        
        public static Size BASE_RESOLUTION = new Size(1920, 1080);
        public static decimal BASE_ASPECT_RATIO = (decimal)BASE_RESOLUTION.Width / BASE_RESOLUTION.Height;

        public static readonly int EVENT_BACKGROUND_CLICKED = EventId.New();

        private BackgroundView backgroundView;
        private bool isTripleScreen;
        private ZoomLevel zoomLevel = ZoomLevel.FIT_TO_WINDOW;

        public Rectangle ScreenArea { get => new Rectangle(backgroundView.Location, backgroundView.Size); }

        public ScreenView()
        {
            InitializeUI();
        }

        internal void DisplayPlaceHolders(PlaceholderModel[] placeHolders)
        {
            UpdateScreenSize();

            foreach (PlaceholderModel model in placeHolders)
            {
                PlaceholderView view = Injector.GetInstance<PlaceholderView>();
                view.Initialize(model, backgroundView.Size, new Point(backgroundView.Location.X, backgroundView.Location.Y), isTripleScreen);

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
                Scroll -= OnScrollChanged;
                VerticalScroll.Value = HorizontalScroll.Value = 0;
                AutoScroll = false;
            }
            else
            {
                Scroll += OnScrollChanged;
                AutoScroll = true;
                //TODO Gérer le scroll en manuel pour virer la barre verticale qui peut appraître avec la molette souris sur le position preset.
            }
        }

        private void OnScrollChanged(object sender, ScrollEventArgs e)
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
            if(zoomLevel == ZoomLevel.FIT_TO_WINDOW)
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

        private void InitializeUI()
        {
            //DoubleBuffered = true;

            BackColor = Colors.SCREEN_BACKGROUND;
            backgroundView = Injector.GetInstance<BackgroundView>();
            backgroundView.Location = new Point(SCREEN_MARGIN, SCREEN_MARGIN);

            Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_BACKGROUND_CLICKED));
            backgroundView.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_BACKGROUND_CLICKED));

            Controls.Add(backgroundView);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
