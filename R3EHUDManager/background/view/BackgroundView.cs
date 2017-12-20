using R3EHUDManager.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.model;
using System.Drawing.Drawing2D;
using R3EHUDManager.screen.view;
using System.Diagnostics;

namespace R3EHUDManager.background.view
{
    class BackgroundView : Control
    {
        private Image baseBitmap;
        private Image displayedBitmap;
        private ZoomLevel zoomLevel = ZoomLevel.FIT_TO_WINDOW;
        private Size screenArea;
        private bool isTripleScreen;

        public delegate void BackgroundDrawHook (Graphics graphics);

        public BackgroundDrawHook DrawHook;

        public BackgroundView()
        {
            DoubleBuffered = true;
            Disposed += OnDispose;
        }

        internal void SetScreenArea(Size screenArea, ZoomLevel zoomLevel)
        {
            this.screenArea = screenArea;
            this.zoomLevel = zoomLevel;
            ComputeSize();
            Invalidate();
        }

        internal void SetBackground(ScreenModel model)
        {
            isTripleScreen = model.Layout == ScreenLayoutType.TRIPLE;
            baseBitmap = model.GetBackgroundImage();
            ComputeSize();
            Invalidate();
        }

        public void SetTripleScreen(bool value)
        {
            isTripleScreen = value;
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (baseBitmap == null) return;

            if (zoomLevel == ZoomLevel.FIT_TO_HEIGHT)
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.DrawImage(displayedBitmap, new Point());
            }
            else
                e.Graphics.DrawImage(baseBitmap, new Rectangle(0, 0, Width, Height));

            if (isTripleScreen)
            {
                int centerLeft = (int)((decimal)Width / 3);
                int centerRight = (int)(2 * (decimal)Width / 3);

                Color lineColor = Color.FromArgb(100, Color.Black);

                e.Graphics.DrawLine(
                    new Pen(lineColor, 2) { Alignment = PenAlignment.Center },
                    new Point(centerLeft, 0), new Point(centerLeft, Height));

                e.Graphics.DrawLine(
                    new Pen(lineColor, 2) { Alignment = PenAlignment.Center },
                    new Point(centerRight, 0), new Point(centerRight, Height));
            }

            DrawHook?.Invoke(e.Graphics);
        }

        private void ComputeSize()
        {
            if (baseBitmap == null)
            {
                Size = new Size(screenArea.Width, screenArea.Height);
                return;
            }

            float bitmapRatio = baseBitmap.PhysicalDimension.Width / baseBitmap.PhysicalDimension.Height;

            if (zoomLevel == ZoomLevel.FIT_TO_WINDOW)
            {
                float screenRatio = (float)screenArea.Width / screenArea.Height;

                if (screenRatio < bitmapRatio)
                {
                    Width = screenArea.Width;
                    Height = (int)(screenArea.Width / bitmapRatio);
                }
                else
                {
                    Height = screenArea.Height;
                    Width = (int)(screenArea.Height * bitmapRatio);
                }
            }
            else if (zoomLevel == ZoomLevel.FIT_TO_HEIGHT)
            {
                Height = screenArea.Height;
                Width = (int)(screenArea.Height * bitmapRatio);
            }

            PrepareBitmap();
        }

        private void PrepareBitmap()
        {
            if (displayedBitmap != null) displayedBitmap.Dispose();

            if (zoomLevel == ZoomLevel.FIT_TO_HEIGHT)
            {
                // More CPU when resizing window but better scrolling. Still searching a better way.
                displayedBitmap = new Bitmap(baseBitmap, Size);
            }
        }

        private void OnDispose(object sender, EventArgs e)
        {
            if (displayedBitmap != null) displayedBitmap.Dispose();
        }
    }
}
