using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using System.Drawing;
using System.Diagnostics;
using da2mvc.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.view;
using R3EHUDManager.background.view;
using da2mvc.injection;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.model;
using System.Runtime.InteropServices;

namespace R3EHUDManager.screen.view
{
    class ScreenView : Panel, IEventDispatcher
    {
        private Dictionary<string, PlaceholderView> views;
        private const int SCREEN_MARGIN = 30;
        public event EventHandler MvcEventHandler;
        
        public static Size BASE_RESOLUTION = new Size(1920, 1080);
        public static decimal BASE_ASPECT_RATIO = (decimal)BASE_RESOLUTION.Width / BASE_RESOLUTION.Height;
        

        public const string EVENT_PLACEHOLDER_MOVED = "placeholderMoved";
        public const string EVENT_PLACEHOLDER_SELECTED = "placeholderSelected";
        public const string EVENT_BACKGROUND_CLICKED = "backgroundClicked";

        private BackgroundView backgroundView;
        private bool isTripleScreen;
        private ZoomLevel zoomLevel = ZoomLevel.FIT_TO_WINDOW;

        public ScreenView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            //DoubleBuffered = true;

            BackColor = Color.FromArgb(47,65,75);

            AutoScroll = true;
            Scroll += OnScrollChanged;

            backgroundView = (BackgroundView)Injector.GetInstance(typeof(BackgroundView));
            backgroundView.Location = new Point(SCREEN_MARGIN, SCREEN_MARGIN);

            Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_BACKGROUND_CLICKED));
            backgroundView.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_BACKGROUND_CLICKED));

            Controls.Add(backgroundView);
        }

        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern bool LockWindowUpdate(IntPtr hWnd);

        private void OnScrollChanged(object sender, ScrollEventArgs e)
        {
            foreach (PlaceholderView view in views.Values)
                view.OnScreenScrolled(backgroundView.Location);

            // TODO update data and refresh
            //Refresh();
            //Refresh();

            //if (e.Type == ScrollEventType.First)
            //{
            //    LockWindowUpdate(this.Handle);
            //}
            //else
            //{
            //    LockWindowUpdate(IntPtr.Zero);
            //    Update();
            //    if (e.Type != ScrollEventType.Last)
            //        LockWindowUpdate(this.Handle);
            //}
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        const int WS_EX_COMPOSITED = 0x02000000;
        //        var cp = base.CreateParams;
        //        cp.ExStyle |= WS_EX_COMPOSITED;
        //        return cp;
        //    }
        //}

        internal void BackgroundChanged(ScreenModel screenModel)
        {
            VerticalScroll.Value = HorizontalScroll.Value = 0;
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

        internal void DisplayPlaceHolders(List<PlaceholderModel> placeHolders)
        {
            UpdateScreenSize();

            RemovePlaceholders();
            views = new Dictionary<string, PlaceholderView>();

            foreach (PlaceholderModel model in placeHolders)
            {
                PlaceholderView view = new PlaceholderView(model, backgroundView.Size, new Point(backgroundView.Location.X, backgroundView.Location.Y), isTripleScreen);

                view.PositionChanged += OnViewPositionChanged;
                view.Dragging += OnPlaceholderDragging;
                view.MouseDown += OnPlaceholderMouseDown;

                views.Add(model.Name, view);
            }

            Controls.AddRange(views.Values.ToArray());
            backgroundView.SendToBack();
        }

        internal void SelectPlaceholder(PlaceholderModel placeholder, bool selected)
        {
            PlaceholderView view = views[placeholder.Name];
            view.SetSelected(selected);
            view.BringToFront();
        }

        internal void SetZoomLevel(ZoomLevel zoomLevel)
        {
            if (this.zoomLevel == zoomLevel) return;

            this.zoomLevel = zoomLevel;

            //AutoScroll = zoomLevel == ZoomLevel.FIT_HEIGHT;
            //AdjustFormScrollbars(false);

            if (zoomLevel == ZoomLevel.FIT_TO_WINDOW)
            {
                VerticalScroll.Value = HorizontalScroll.Value = 0;
                //AutoScroll = false;
            }
            else
            {
                //AutoScroll = true;
                backgroundView.Location = new Point(SCREEN_MARGIN, SCREEN_MARGIN);
            }

            UpdateScreenSize();
            UpdatePlaceholdersPosition();

            AutoScroll = zoomLevel == ZoomLevel.FIT_TO_HEIGHT;
        }

        private void OnPlaceholderMouseDown(object sender, MouseEventArgs e)
        {
            DispatchEvent(new StringEventArgs(EVENT_PLACEHOLDER_SELECTED, ((PlaceholderView)sender).Model.Name));
        }

        internal void UpdatePlaceholder(PlaceholderModel placeholder, UpdateType updateType)
        {
            views[placeholder.Name].Update(updateType);
        }

        private void UpdatePlaceholdersPosition()
        {
            if(views != null)
                foreach(PlaceholderView view in views.Values)
                {
                    view.SetScreenSize(backgroundView.Size, isTripleScreen, backgroundView.Location);
                }
        }

        private void RemovePlaceholders()
        {
            if (views != null)
                foreach (PlaceholderView placeholder in views.Values)
                {
                    placeholder.PositionChanged -= OnViewPositionChanged;
                    placeholder.Dragging -= OnPlaceholderDragging;
                    placeholder.MouseDown -= OnPlaceholderMouseDown;

                    Controls.Remove(placeholder);
                    placeholder.Dispose();
                }
        }

        private void OnViewPositionChanged(object sender, EventArgs e)
        {
            PlaceholderView view = (PlaceholderView)sender;

            R3ePoint r3eLocation = view.GetR3eLocation();

            DispatchEvent(new PlaceHolderMovedEventArgs(EVENT_PLACEHOLDER_MOVED,
                ((PlaceholderView)sender).Model.Name, r3eLocation));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateScreenSize();
            UpdatePlaceholdersPosition();
            base.OnSizeChanged(e);
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

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
