using R3EHUDManager.coordinates;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace R3EHUDManager.placeholder.view
{
    class PlaceholderView : Panel
    {
        public string PlaceholderName { get => label.Text; set => label.Text = value; }
        public event EventHandler PositionChanged;
        private Label label;
        private Point dragStartPosition;
        private Point dragMouseOffset;
        private AnchorView anchor;

        public PlaceholderView()
        {
            InitializeUI();
        }

        public Point AnchorPosition
        {
            get { return new Point(anchor.Location.X + anchor.Width / 2, anchor.Location.Y + anchor.Height/ 2); }
            set { anchor.Location = new Point(value.X - anchor.Width / 2, value.Y - anchor.Height / 2); }
        }

        private void InitializeUI()
        {
            //AutoSize = true;
            //AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Size = new Size(100, 30);

            BackColor = Color.LightGray;
            //BorderStyle = BorderStyle.FixedSingle;

            label = new Label()
            {
                //AutoSize = true,
                BackColor = Color.Orange,
                Enabled = false, // TODO no mouseEnabled ?
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
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
