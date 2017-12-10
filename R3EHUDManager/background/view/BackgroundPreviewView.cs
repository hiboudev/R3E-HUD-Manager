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
        private float internalScale;
        private Bitmap bitmap;
        private Size bitmapSize;
        private float rectangleRightRatio = 0;
        private float rectangleXRatio;
        private Size originalBitmapSize;
        private const int MAX_BITMAP_SIZE = 1280;
        private bool lineStyle;

        public BackgroundPreviewView()
        {
            DoubleBuffered = true;
            Disposed += OnDispose;
            SizeChanged += (sender, args) => RefreshBackground();
        }

        internal void SetBitmap(Bitmap bitmap)
        {
            originalBitmapSize = new Size(bitmap.Width, bitmap.Height);

            Size newSize = new Size();
            float bitmapRatio = bitmap.PhysicalDimension.Width / bitmap.PhysicalDimension.Height;

            if(bitmapRatio > 1)
            {
                newSize.Width = MAX_BITMAP_SIZE;
                newSize.Height = (int)(MAX_BITMAP_SIZE / bitmapRatio);
            } 
            else
            {
                newSize.Height = MAX_BITMAP_SIZE;
                newSize.Width = (int)(MAX_BITMAP_SIZE * bitmapRatio);
            }

            internalScale = (float)newSize.Width / originalBitmapSize.Width;

            this.bitmap = new Bitmap(bitmap, newSize);
            bitmap.Dispose();

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

        internal void DrawRectangle(int x, int width, bool lineStyle)
        {
            if (bitmap == null) return;

            rectangleXRatio = (float)x / originalBitmapSize.Width;
            rectangleRightRatio = (float)(x + width) / originalBitmapSize.Width;

            this.lineStyle = lineStyle;

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

            if (bitmap == null || bitmapSize == null) return;

            int marginX = (Width - bitmapSize.Width) / 2;
            int marginY = (Height - bitmapSize.Height) / 2;

            if (rectangleRightRatio <= 0)
            {
                e.Graphics.DrawImage(bitmap, new Rectangle(marginX, marginY, bitmapSize.Width, bitmapSize.Height));
                return;
            }

            Bitmap paintBitmap = new Bitmap(bitmap);
            Graphics paintSurface = Graphics.FromImage(paintBitmap);

            int areaLeft = (int)Math.Round(rectangleXRatio * bitmap.Width);
            int areaRight = (int)Math.Round(rectangleRightRatio * bitmap.Width);

            if (lineStyle)
            {
                paintSurface.DrawLine(new Pen(Color.FromArgb(255, Color.Black), 3) { Alignment = PenAlignment.Center },
                    new Point(areaLeft, 0),
                    new Point(areaLeft, paintBitmap.Height));

                paintSurface.DrawLine(new Pen(Color.FromArgb(255, Color.Black), 3) { Alignment = PenAlignment.Center },
                    new Point(areaRight, 0),
                    new Point(areaRight, paintBitmap.Height));
            }
            else
            {
                paintSurface.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)),
                    new Rectangle(
                        0,
                        0,
                        areaLeft,
                        paintBitmap.Height));

                paintSurface.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)),
                    new Rectangle(
                        areaRight,
                        0,
                        paintBitmap.Width - areaRight,
                        paintBitmap.Height));
            }



            e.Graphics.DrawImage(paintBitmap, new Rectangle(marginX, marginY, bitmapSize.Width, bitmapSize.Height));

            paintSurface.Dispose();
            paintBitmap.Dispose();
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
