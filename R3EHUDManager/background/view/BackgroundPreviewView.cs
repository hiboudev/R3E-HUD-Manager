using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class BackgroundPreviewView : Panel
    {
        private Bitmap bitmap;
        private Size bitmapSize;
        private float rectangleRightRatio = 0;
        private float rectangleXRatio;

        public BackgroundPreviewView()
        {
            DoubleBuffered = true;
            Disposed += OnDispose;
            SizeChanged += (sender, args) => RefreshBackground();
        }

        internal void SetBitmap(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            RefreshBackground();
        }

        private void RefreshBackground()
        {
            if (bitmap == null) return;

            decimal panelRatio = (decimal)Width / Height;
            decimal bitmapRatio = (decimal)(bitmap.PhysicalDimension.Width / bitmap.PhysicalDimension.Height);
            bitmapSize = new Size();
            if(bitmapRatio > panelRatio)
            {
                bitmapSize.Width = Width;
                bitmapSize.Height = (int) (Width / bitmapRatio);
            }
            else
            {
                bitmapSize.Height = Height;
                bitmapSize.Width = (int)(Height * bitmapRatio);
            }

            Invalidate();
        }

        internal void DrawRectangle(int x, int width)
        {
            if (bitmap == null) return;

            rectangleRightRatio = (x+width) / bitmap.PhysicalDimension.Width;
            rectangleXRatio = x / bitmap.PhysicalDimension.Width;

            Invalidate();
        }

        internal void ClearRectangle()
        {
            rectangleRightRatio = 0;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (bitmap == null) return;

            int marginX = (Width - bitmapSize.Width) / 2;
            int marginY = (Height - bitmapSize.Height) / 2;

            Bitmap resizedBitmap = new Bitmap(bitmap, bitmapSize);
            e.Graphics.DrawImage(resizedBitmap, new Point(marginX, marginY));
            resizedBitmap.Dispose();


            if (rectangleRightRatio <= 0 || bitmapSize == null)
                return;

            int areaLeft = (int)Math.Round(rectangleXRatio * bitmapSize.Width);
            int areaRight = (int)Math.Round(rectangleRightRatio * bitmapSize.Width);

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), 
                new Rectangle(
                    marginX,
                    marginY,
                    areaLeft,
                    bitmapSize.Height));

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)),
                new Rectangle(
                    areaRight + marginX,
                    marginY,
                    bitmapSize.Width - areaRight,
                    bitmapSize.Height));

            e.Graphics.DrawLine(new Pen(Color.Black, 2)
            {
                Alignment = PenAlignment.Center,
            },
                new Point(areaLeft + marginX, marginY),
                new Point(areaLeft + marginX, marginY + bitmapSize.Height));

            e.Graphics.DrawLine(new Pen(Color.Black, 2)
            {
                Alignment = PenAlignment.Center,
            },
                new Point(areaRight + marginX, marginY),
                new Point(areaRight + marginX, marginY + bitmapSize.Height));
        }

        private void OnDispose(object sender, EventArgs e)
        {
            // TODO Called 2 times
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }
    }
}
