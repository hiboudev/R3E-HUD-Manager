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

namespace R3EHUDManager.placeholder.view
{
    class ScreenView : Panel, IEventDispatcher
    {
        private Dictionary<string, PlaceholderView> views;
        private const int SCREEN_MARGIN = 70;
        public event EventHandler MvcEventHandler;
        
        public static Size BASE_RESOLUTION = new Size(1920, 1080);
        private double BASE_ASPECT_RATIO = (double)BASE_RESOLUTION.Width / BASE_RESOLUTION.Height;

        private Size screenSize = new Size(100, 100);

        public const string EVENT_PLACEHOLDER_MOVED = "placeholderMoved";
        public const string EVENT_PLACEHOLDER_SELECTED = "placeholderSelected";

        private BackgroundView backgroundView;

        public ScreenView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            BackColor = Color.FromArgb(47,79,89);
            
            backgroundView = new BackgroundView
            {
                Location = new Point(SCREEN_MARGIN, SCREEN_MARGIN)
            };

            Controls.Add(backgroundView);
        }

        internal void DisplayPlaceHolders(List<PlaceholderModel> placeHolders)
        {
            UpdateScreenSize();

            RemovePlaceholders();
            views = new Dictionary<string, PlaceholderView>();

            double sizeRatio = (double)screenSize.Width / BASE_RESOLUTION.Width;

            foreach (PlaceholderModel model in placeHolders)
            {
                PlaceholderView view = new PlaceholderView(model, screenSize, new Point(SCREEN_MARGIN, SCREEN_MARGIN));
                // TODO Check if it's correct with a screen with aspect ratio different than 16/9.

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
                    view.SetScreenSize(screenSize);
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

            DispatchEvent(new PlaceHolderMovedEventArgs(EVENT_PLACEHOLDER_MOVED,
                ((PlaceholderView)sender).Model.Name, view.GetR3eLocation()));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateScreenSize();
            UpdatePlaceholdersPosition();
        }

        private void UpdateScreenSize()
        {
            // When minimizing the window, size is zero.
            if (Width == 0 && Height == 0)
                return;

            double screenRatio = (double)(Width - SCREEN_MARGIN * 2) / (Height - SCREEN_MARGIN * 2);

            if(screenRatio < BASE_ASPECT_RATIO)
            {
                screenSize.Width = Width - SCREEN_MARGIN * 2;
                screenSize.Height = (int)((double)screenSize.Width / BASE_ASPECT_RATIO);
            }
            else
            {
                screenSize.Height = Height- SCREEN_MARGIN * 2;
                screenSize.Width = (int)((double)screenSize.Height * BASE_ASPECT_RATIO);
            }

            backgroundView.SetSize(screenSize);
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
