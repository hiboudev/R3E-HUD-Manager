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

namespace R3EHUDManager.background.view
{
    class BackgroundView:Control
    {
        private Image baseBitmap;
        private Size screenArea;
        private bool isTripleScreen;

        public BackgroundView()
        {
            DoubleBuffered = true;
            Disposed += OnDispose;
        }

        internal void SetScreenArea(Size screenArea)
        {
            this.screenArea = screenArea;
            ComputeSize();
            Invalidate();
        }

        internal void SetBackground(ScreenModel model)
        {
            DisposeBaseBitmap();
            baseBitmap = model.GetBackgroundImage();
            ComputeSize();
            Invalidate();
        }

        public void SetTripleScreen(bool value)
        {
            isTripleScreen = value;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (baseBitmap == null) return;

            Bitmap resizedBitmap = new Bitmap(baseBitmap, Size);
            e.Graphics.DrawImage(resizedBitmap, new Point());
            resizedBitmap.Dispose();

            int centerLeft = (int)((decimal)Width / 3);
            int centerRight = (int)(2 * (decimal)Width / 3);

            if (isTripleScreen)
            {
                Color lineColor = Color.FromArgb(100, Color.Black);

                e.Graphics.DrawLine(
                    new Pen(lineColor, 2) { Alignment = PenAlignment.Center },
                    new Point(centerLeft, 0), new Point(centerLeft, Height));

                e.Graphics.DrawLine(
                    new Pen(lineColor, 2) { Alignment = PenAlignment.Center },
                    new Point(centerRight, 0), new Point(centerRight, Height));
            }
        }

        private void ComputeSize()
        {
            if (baseBitmap == null) return;

            float screenRatio = (float)screenArea.Width / screenArea.Height;
            float bitmapRatio = baseBitmap.PhysicalDimension.Width / baseBitmap.PhysicalDimension.Height;

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

        private void OnDispose(object sender, EventArgs e)
        {
            DisposeBaseBitmap();
        }

        private void DisposeBaseBitmap()
        {
            if (baseBitmap != null)
            {
                baseBitmap.Dispose();
                baseBitmap = null;
            }
        }
    }
}
