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

        public const string EVENT_POSITION_CHANGED = "positionChanged";
        private BackgroundView backgroundView;

        public ScreenView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            BackColor = Color.DarkSlateGray;

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

                views.Add(model.Name, view);
            }

            Controls.AddRange(views.Values.ToArray());
            backgroundView.SendToBack();
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
                    Controls.Remove(placeholder);
                    placeholder.Dispose();
                }
        }

        private void OnViewPositionChanged(object sender, EventArgs e)
        {
            PlaceholderView view = (PlaceholderView)sender;

            DispatchEvent(new PlaceHolderMovedEventArgs(EVENT_POSITION_CHANGED,
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
