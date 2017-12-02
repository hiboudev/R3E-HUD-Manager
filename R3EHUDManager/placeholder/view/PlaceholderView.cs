using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.model;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace R3EHUDManager.placeholder.view
{
    class PlaceholderView : Panel
    {
        public event EventHandler PositionChanged;
        public event EventHandler Dragging;
        private Label label;
        private Point dragStartPosition;
        private Point dragMouseOffset;
        private AnchorView anchor;
        private Bitmap image;
        private Size screenSize;
        private double screenRatio;
        private readonly Point screenOffset;
        public PlaceholderModel Model { get; }

        private Point AnchorPosition
        {
            get => new Point(Width * anchor.Location.X / AnchorArea.Width, Height * anchor.Location.Y / AnchorArea.Height);
            set { anchor.Location = value; }
        }

        private Size AnchorArea
        {
            get => new Size(Width - anchor.Width, Height - anchor.Height);
        }

        public PlaceholderView(PlaceholderModel model, Size screenSize, Point screenOffset)
        {
            Model = model;
            this.screenOffset = screenOffset;
            InitializeUI();
            SetScreenSize(screenSize);
            Disposed += OnDispose;
        }

        private void RefreshLocation()
        {
            //TODO pour plus de précision, faire l'offset avant la conversion de coordonnées
            Point location = Coordinates.FromR3e(Model.Position, new Size(screenSize.Width, screenSize.Height));
            Point anchor = Coordinates.FromR3e(Model.Anchor, Size);

            location.Offset(new Point(-anchor.X + screenOffset.X, -anchor.Y + screenOffset.Y));
            Location = location;

            AnchorPosition = Coordinates.FromR3e(Model.Anchor, AnchorArea);
        }

        public R3ePoint GetR3eLocation()
        {
            Point position = new Point(Location.X - screenOffset.X + AnchorPosition.X, Location.Y - screenOffset.Y + AnchorPosition.Y);
            return Coordinates.ToR3e(position, screenSize);
        }

        public void SetScreenSize(Size size)
        {
            screenSize = size;
            screenRatio = (double)screenSize.Width / ScreenView.BASE_RESOLUTION.Width;
            RedrawImage();
            RefreshLocation();
        }

        internal void Update(UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.POSITION:
                    RefreshLocation();
                    break;
            }
        }

        private void InitializeUI()
        {
            BackColor = Color.Black;

            label = new Label()
            {
                Text = Model.Name,
                AutoSize = true,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
            };


            anchor = new AnchorView();
            anchor.Enabled = false;

            Controls.Add(anchor);
            Controls.Add(label);

            MouseEnter += (sender, args) => BringToFront();

            MouseDown += StartDrag;
            MouseUp += StopDrag;
        }

        private void RedrawImage()
        {
            if (image != null) image.Dispose();

            Image originalImage = GraphicalAsset.GetPlaceholderImage(Model.Name);

            int width = (int)((double)originalImage.PhysicalDimension.Width * screenRatio);
            int height = (int)((double)originalImage.PhysicalDimension.Height * screenRatio);

            image = new Bitmap(originalImage, new Size(width, height));

            BackgroundImage = image;
            Size = new Size(width, height);
        }

        void StartDrag(object sender, MouseEventArgs e)
        {
            dragStartPosition = Location;
            dragMouseOffset = e.Location;

            MouseMove += Drag;
        }

        void Drag(object sender, MouseEventArgs e)
        {
            var location = Location;
            location.Offset(e.Location.X - dragMouseOffset.X, e.Location.Y - dragMouseOffset.Y);
            Location = location;

            Dragging?.Invoke(this, EventArgs.Empty);
        }

        void StopDrag(object sender, MouseEventArgs e)
        {
            MouseMove -= Drag;

            if (!Location.Equals(dragStartPosition))
            {
                PositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDispose(object sender, EventArgs e)
        {
            if (image != null) image.Dispose();
        }

    }
}
