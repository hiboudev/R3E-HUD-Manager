using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using System.Drawing;
using System.Diagnostics;
using Test_MVC.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.coordinates;

namespace R3EHUDManager.placeholder.view
{
    class ScreenView : Panel, IEventDispatcher
    {
        private Dictionary<string, PlaceholderView> views;
        private const int SCREEN_MARGIN = 100;
        public event EventHandler MyEventHandler;
        public const string EVENT_POSITION_CHANGED = "positionChanged";

        public ScreenView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            BackColor = Color.PaleVioletRed;
        }

        internal void DisplayPlaceHolders(List<PlaceholderModel> placeHolders)
        {
            Controls.Clear();
            views = new Dictionary<string, PlaceholderView>();

            foreach (PlaceholderModel placeHolder in placeHolders)
            {
                PlaceholderView view = new PlaceholderView()
                {
                    PlaceholderName = placeHolder.Name,
                };
                view.PositionChanged += OnViewPositionChanged;
                view.Location = GetLocation(placeHolder, view);// GetCoordinate(placeHolder.Position, new Size(Width - 2 * SCREEN_MARGIN, Height - 2 * SCREEN_MARGIN), new Point(SCREEN_MARGIN, SCREEN_MARGIN));
                view.AnchorPosition = GetCoordinate(placeHolder.Anchor, view.Size, new Point());

                views.Add(placeHolder.Name, view);
            }

            Controls.AddRange(views.Values.ToArray());
        }

        private void OnViewPositionChanged(object sender, EventArgs e)
        {
            PlaceholderView view = (PlaceholderView)sender;

            Point position = new Point(view.Location.X - SCREEN_MARGIN + view.AnchorPosition.X, view.Location.Y - SCREEN_MARGIN + view.AnchorPosition.Y);
            Size size = new Size(Width - 2 * SCREEN_MARGIN, Height - 2 * SCREEN_MARGIN);

            dispatchEvent(new PlaceHolderMovedEventArgs(EVENT_POSITION_CHANGED,
                ((PlaceholderView)sender).PlaceholderName,
                Coordinates.ToR3e(position, size)));
        }

        private Point GetLocation(PlaceholderModel model, PlaceholderView view)
        {
            Point position = Coordinates.FromR3e(model.Position, new Size(Width - 2 * SCREEN_MARGIN, Height - 2 * SCREEN_MARGIN));
            Point anchor = Coordinates.FromR3e(model.Anchor, view.Size);

            position.Offset(new Point(-anchor.X + SCREEN_MARGIN, -anchor.Y + SCREEN_MARGIN));
            return position;
        }

        private Point GetCoordinate(R3ePoint point, Size size, Point offset)
        {
            Point coords = Coordinates.FromR3e(point,  size);

            coords.X += offset.X;
            coords.Y += offset.Y;

            return coords;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), DisplayRectangle);

            e.Graphics.FillRectangle(new SolidBrush(Color.WhiteSmoke),
                new Rectangle(
                    new Point(SCREEN_MARGIN, SCREEN_MARGIN),
                    new Size(Width - SCREEN_MARGIN * 2, Height - SCREEN_MARGIN * 2)));
        }

        public void dispatchEvent(BaseEventArgs args)
        {
            MyEventHandler?.Invoke(this, args);
        }
    }
}
