using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class BabckgroundPreviewView : Panel
    {
        private Bitmap bitmap;
        private Size bitmapSize;
        private float rectangleWidthRatio = 0;
        private float rectangleXRatio;

        public BabckgroundPreviewView()
        {
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

            if (BackgroundImage != null)
                BackgroundImage.Dispose();

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

            BackgroundImage = new Bitmap(bitmap, bitmapSize);
            BackgroundImageLayout = ImageLayout.Center;

            Invalidate();
        }

        internal void DrawRectangle(int x, int width)
        {
            if (bitmap == null) return;

            rectangleWidthRatio = width / bitmap.PhysicalDimension.Width;
            rectangleXRatio = x / bitmap.PhysicalDimension.Width;

            Invalidate();
        }

        internal void ClearRectangle()
        {
            rectangleWidthRatio = 0;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (rectangleWidthRatio <= 0 || bitmapSize == null)
            {
                return;
            }

            e.Graphics.DrawRectangle(new Pen(Color.Yellow, 2)
            {
                Alignment = PenAlignment.Inset
            }, new Rectangle((int)Math.Round((rectangleXRatio * bitmapSize.Width + ((double)Width - bitmapSize.Width)/2)), (Height - bitmapSize.Height) / 2, (int)Math.Round(rectangleWidthRatio * bitmapSize.Width), bitmapSize.Height));
        }

        private void OnDispose(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
                BackgroundImage = null;
            }
        }
    }
}
