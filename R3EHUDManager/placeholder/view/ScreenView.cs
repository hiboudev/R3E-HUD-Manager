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
        private const int SCREEN_MARGIN = 100;
        public event EventHandler MvcEventHandler;
        private const double ASPECT_RATIO = 16f / 9;
        private static Size BASE_RESOLUTION = new Size(1920, 1080);
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

            foreach (PlaceholderModel placeholder in placeHolders)
            {
                PlaceholderView view = new PlaceholderView(placeholder.Name, sizeRatio);
                // TODO Check if it's correct with a screen with aspect ratio different than 16/9.

                view.PositionChanged += OnViewPositionChanged;
                view.Dragging += OnPlaceholderDragging;

                view.Location = GetLocation(placeholder, view);// GetCoordinate(placeHolder.Position, new Size(Width - 2 * SCREEN_MARGIN, Height - 2 * SCREEN_MARGIN), new Point(SCREEN_MARGIN, SCREEN_MARGIN));
                view.AnchorPosition = GetCoordinate(placeholder.Anchor, view.AnchorArea, new Point());

                views.Add(placeholder.Name, view);
            }

            Controls.AddRange(views.Values.ToArray());
            backgroundView.SendToBack();
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

            Point position = new Point(view.Location.X - SCREEN_MARGIN + view.AnchorPosition.X, view.Location.Y - SCREEN_MARGIN + view.AnchorPosition.Y);
            Size size = new Size(Width - 2 * SCREEN_MARGIN, Height - 2 * SCREEN_MARGIN);

            DispatchEvent(new PlaceHolderMovedEventArgs(EVENT_POSITION_CHANGED,
                ((PlaceholderView)sender).PlaceholderName,
                Coordinates.ToR3e(position, size)));
        }

        private Point GetLocation(PlaceholderModel model, PlaceholderView view)
        {
            Point position = Coordinates.FromR3e(model.Position, new Size(screenSize.Width, screenSize.Height));
            Point anchor = Coordinates.FromR3e(model.Anchor, view.Size);

            position.Offset(new Point(-anchor.X + SCREEN_MARGIN, -anchor.Y + SCREEN_MARGIN));
            return position;
        }

        private Point GetCoordinate(R3ePoint point, Size size, Point offset)
        {
            Point coords = Coordinates.FromR3e(point, size);

            coords.X += offset.X;
            coords.Y += offset.Y;

            return coords;
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    e.Graphics.Clear(Color.Gray);
        //    base.OnPaint(e);


        //    //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), DisplayRectangle);

        //    UpdateScreenSize();
        //    Image back0 = GraphicalAsset.GetBackground();
        //    Image back = new Bitmap(back0, new Size(screenSize.Width, screenSize.Height));

        //    e.Graphics.DrawImage(back, new Point(SCREEN_MARGIN, SCREEN_MARGIN));
        //    //e.Graphics.FillRectangle(new SolidBrush(Color.WhiteSmoke),
        //    //    new Rectangle(
        //    //        new Point(SCREEN_MARGIN, SCREEN_MARGIN),
        //    //        new Size(screenSize.Width, screenSize.Height)));

        //    back0.Dispose();
        //    back.Dispose();
        //    // TODO redraw items if size changed?
        //}

        private void UpdateScreenSize()
        {
            screenSize.Width = Width - SCREEN_MARGIN * 2;
            screenSize.Height = (int)(screenSize.Width / ASPECT_RATIO);

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
