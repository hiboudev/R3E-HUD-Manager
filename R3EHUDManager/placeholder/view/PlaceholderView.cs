using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.view;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace R3EHUDManager.placeholder.view
{
    class PlaceholderView : Panel, IEventDispatcher
    {
        public event EventHandler Dragging;
        public event EventHandler MvcEventHandler;

        private Label label;
        private bool isDragging;
        private Point dragStartCursorPosition;
        private Point dragStartLocation;
        private AnchorView anchor;
        private Size screenSize;
        //private double screenRatio;
        private Point screenOffset;
        private bool isTripleScreen;
        private bool selected;
        private readonly static Color SELECTION_COLOR = Color.DeepSkyBlue;
        private readonly static Color LABEL_BACK_COLOR = Color.LightGray;

        public static readonly int EVENT_REQUEST_SELECTION = EventId.New();

        public static readonly int EVENT_REQUEST_MOVE = EventId.New();

        public PlaceholderModel Model { get; private set; } // TODO remove model from view

        public void Initialize(PlaceholderModel model, Size screenSize, Point screenOffset, bool isTripleScreen)
        {
            Model = model;
            this.isTripleScreen = isTripleScreen;

            InitializeUI();
            SetScreenSize(screenSize, isTripleScreen, screenOffset);
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

        public void SetScreenSize(Size screenSize, bool isTripleScreen, Point screenOffset)
        {
            this.screenOffset = screenOffset;
            this.screenSize = screenSize;
            this.isTripleScreen = isTripleScreen;
            ComputeSize();
            RefreshLocation();
            Invalidate();
        }

        internal void OnScreenScrolled(Point screenOffset)
        {
            this.screenOffset = screenOffset;
            //RefreshLocation();
            //Invalidate();
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
                    ComputeSize();
                    RefreshLocation();
                    Invalidate();
                    break;
            }
        }

        internal void SetSelected(bool selected)
        {
            this.selected = selected;
            label.BackColor = selected ? SELECTION_COLOR : LABEL_BACK_COLOR;
            label.Font = selected ? new Font(label.Font, FontStyle.Bold) : new Font(label.Font, FontStyle.Regular);
            if (selected) BringToFront();
            Invalidate();
        }

        internal void ShowInvalidLayout(string description)
        {
            label.BackColor = Color.Red; // TODO quand les vues s'initialisent, l'event a déjà été envoyé. Passer par le modèle ?
        }

        internal void ShowValidLayout()
        {
            label.BackColor = Color.Gray;
        }

        private void ComputeSize()
        {
            Image originalImage = GraphicalAsset.GetPlaceholderImage(Model.Name);
            SizeF newSize = Model.ResizeRule.GetSize(ScreenView.BASE_RESOLUTION, screenSize, originalImage.PhysicalDimension.ToSize(), isTripleScreen);

            int width = (int)(newSize.Width * Model.Size.X);
            int height = (int)(newSize.Height * Model.Size.Y);

            Size = new Size(width, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

            e.Graphics.DrawImage(
                GraphicalAsset.GetPlaceholderImage(Model.Name),
                new Rectangle(0, 0, Size.Width, Size.Height)
                );

            if (selected)
            {
                Rectangle insideRectangle = new Rectangle(0, 0, Width - 1, Height - 1);

                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(210, SELECTION_COLOR), 1)
                {
                    Alignment = PenAlignment.Inset
                }, insideRectangle);
            }
        }

        void StartDrag(object sender, MouseEventArgs e)
        {
            if (!selected)
            {
                DispatchEvent(new IntEventArgs(EVENT_REQUEST_SELECTION, Model.Id));
                return;
            }

            isDragging = true;

            dragStartCursorPosition = Cursor.Position;
            dragStartLocation = Location;

            MouseMove += Drag;
        }

        void Drag(object sender, EventArgs e)
        {
            OnDrag();
        }

        void StopDrag(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            isDragging = false;

            MouseMove -= Drag;
            OnDrag();
        }

        private void OnDrag()
        {
            var requestedLocation = new Point(dragStartLocation.X + Cursor.Position.X - dragStartCursorPosition.X, dragStartLocation.Y + Cursor.Position.Y - dragStartCursorPosition.Y);

            if (!Location.Equals(dragStartCursorPosition))
            {
                DispatchEvent(new PlaceholderViewEventArgs(EVENT_REQUEST_MOVE, this, requestedLocation));
            }

            Dragging?.Invoke(this, EventArgs.Empty);
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

        private void OnDispose(object sender, EventArgs e)
        {
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}


//private void RefreshLocation()
//{
//    R3ePoint modelLocation = Model.Position.Clone();

//    if (isTripleScreen)
//    {
//        modelLocation = new R3ePoint(modelLocation.X / 3, modelLocation.Y);
//    }

//    SizeF objectScreenRatio = new SizeF((float)Width / screenSize.Width, (float)Height / screenSize.Height);

//    R3ePoint r3eLocation = new R3ePoint(
//        modelLocation.X - objectScreenRatio.Width * (Model.Anchor.X + 1),
//        modelLocation.Y - objectScreenRatio.Height * (Model.Anchor.Y - 1));

//    Point location = Coordinates.FromR3e(r3eLocation, new Size(screenSize.Width, screenSize.Height));
//    Point anchor = Coordinates.FromR3e(Model.Anchor, Size);

//    location.Offset(new Point(screenOffset.X, screenOffset.Y));

//    Location = location;

//    AnchorPosition = Coordinates.FromR3e(Model.Anchor, AnchorArea);
//}


