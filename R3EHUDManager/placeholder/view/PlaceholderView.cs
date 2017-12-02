using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using System;
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
        private readonly string placeholderName;
        private readonly double sizeRatio;
        private Bitmap image;

        public string PlaceholderName { get => placeholderName; }

        public PlaceholderView(string placeholderName, double sizeRatio)
        {
            this.placeholderName = placeholderName;
            this.sizeRatio = sizeRatio;
            InitializeUI();
            Disposed += OnDispose;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            if (image != null) image.Dispose();
        }

        public Point AnchorPosition
        {
            get => new Point(Width * anchor.Location.X / AnchorArea.Width, Height * anchor.Location.Y / AnchorArea.Height);
            set { anchor.Location = value; }
        }

        public Size AnchorArea
        {
            get => new Size(Width - anchor.Width, Height - anchor.Height);
        }
        public Size FullSize { get; private set; }

        private void InitializeUI()
        {
            Image originalImage = new Bitmap(GraphicalAsset.GetPlaceholderImage(placeholderName));

            int width = (int)((double)originalImage.PhysicalDimension.Width * sizeRatio);
            int height = (int)((double)originalImage.PhysicalDimension.Height * sizeRatio);

            image = new Bitmap(originalImage, new Size(width, height));
            originalImage.Dispose();

            BackgroundImage = image;
            Size = new Size(width, height);

            BackColor = Color.LightGray;

            label = new Label()
            {
                Text = placeholderName,
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

        
    }
}
