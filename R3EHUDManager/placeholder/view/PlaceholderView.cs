using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.view;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        //private double screenRatio;
        private readonly Point screenOffset;
        private bool isTripleScreen;
        private bool selected;
        private readonly static Color SELECTION_COLOR = Color.DeepSkyBlue;
        private readonly static Color LABEL_BACK_COLOR = Color.LightGray;

        public PlaceholderModel Model { get; }

        public PlaceholderView(PlaceholderModel model, Size screenSize, Point screenOffset, bool isTripleScreen)
        {
            Model = model;
            this.screenOffset = screenOffset;
            this.isTripleScreen = isTripleScreen;

            InitializeUI();
            SetScreenSize(screenSize, isTripleScreen);
            Disposed += OnDispose;
        }

        private Point AnchorPosition
        {
            get => new Point(Width * anchor.Location.X / AnchorArea.Width, Height * anchor.Location.Y / AnchorArea.Height);
            set { anchor.Location = value; }
        }

        private Size AnchorArea
        {
            get => new Size(Width - anchor.Width, Height - anchor.Height);
        }

        private void RefreshLocation()
        {
            //TODO pour plus de précision, faire l'offset avant la conversion de coordonnées
            R3ePoint modelLocation = Model.Position.Clone();
            if (isTripleScreen)
            {
                modelLocation = new R3ePoint(modelLocation.X / 3, modelLocation.Y);
            }

            Point location = Coordinates.FromR3e(modelLocation, new Size(screenSize.Width, screenSize.Height));
            Point anchor = Coordinates.FromR3e(Model.Anchor, Size);

            location.Offset(new Point(-anchor.X + screenOffset.X, -anchor.Y + screenOffset.Y));
            

            Location = location;

            AnchorPosition = Coordinates.FromR3e(Model.Anchor, AnchorArea);
        }

        public R3ePoint GetR3eLocation()
        {
            Point position = new Point(Location.X - screenOffset.X + AnchorPosition.X, Location.Y - screenOffset.Y + AnchorPosition.Y);
            R3ePoint R3ePosition = Coordinates.ToR3e(position, screenSize);

            if(isTripleScreen)
                return new R3ePoint(R3ePosition.X * 3, R3ePosition.Y);
            else
                return Coordinates.ToR3e(position, screenSize);
        }

        public void SetScreenSize(Size screenSize, bool isTripleScreen)
        {
            this.screenSize = screenSize;
            this.isTripleScreen = isTripleScreen;
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

                case UpdateType.ANCHOR:
                    RefreshLocation();
                    break;

                case UpdateType.SIZE:
                    RedrawImage();
                    RefreshLocation();
                    break;
            }
        }

        internal void SetSelected(bool selected)
        {
            this.selected = selected;
            label.BackColor = selected ? SELECTION_COLOR : LABEL_BACK_COLOR;
            label.Font = selected ? new Font(label.Font, FontStyle.Bold) : new Font(label.Font, FontStyle.Regular);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            if (selected)
            {
                Rectangle insideRectangle = new Rectangle(0, 0, Width - 1, Height - 1);

                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(210, SELECTION_COLOR), 1)
                {
                    Alignment = PenAlignment.Inset
                }, insideRectangle);
            }  
        }

        private void InitializeUI()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(0x22, 0x22, 0x22);

            label = new Label()
            {
                Text = Model.Name,
                AutoSize = true,
                BackColor = LABEL_BACK_COLOR,
                ForeColor = Color.Black,
                Enabled = false,
            };

            anchor = new AnchorView
            {
                Enabled = false
            };

            Controls.Add(anchor);
            Controls.Add(label);

            MouseDown += StartDrag;
            MouseUp += StopDrag;
            MouseClick += StopDrag; // To avoid the item to stay stuck to mouse when a break point triggers while dragging it.
        }

        private void RedrawImage()
        {
            if (image != null) image.Dispose();

            Image originalImage = GraphicalAsset.GetPlaceholderImage(Model.Name);

            decimal resizeRatio = Model.ResizeRule.GetResizeRatio(ScreenView.BASE_RESOLUTION, screenSize, originalImage.PhysicalDimension.ToSize());

            //if (isTripleScreen) resizeRatio /= 3;

            int width = (int)((decimal)originalImage.PhysicalDimension.Width * resizeRatio * (decimal)Model.Size.X);
            int height = (int)((decimal)originalImage.PhysicalDimension.Height * resizeRatio * (decimal)Model.Size.Y);

            image = new Bitmap(originalImage, new Size(Math.Max(1, width), Math.Max(1,height)));

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
