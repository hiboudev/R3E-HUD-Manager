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
    class BackgroundPreviewView : Panel
    {
        private Bitmap bitmap;
        private Size bitmapSize;
        private float rectangleRightRatio = 0;
        private float rectangleXRatio;

        public BackgroundPreviewView()
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

            if (rectangleRightRatio <= 0 || bitmapSize == null)
            {
                return;
            }

            int x = (int)Math.Round((rectangleXRatio * bitmapSize.Width + ((double)Width - bitmapSize.Width) / 2));
            int y = (Height - bitmapSize.Height) / 2;
            int width = (int)Math.Round(rectangleRightRatio * bitmapSize.Width - x + ((double)Width - bitmapSize.Width) / 2);
            int height = bitmapSize.Height;

            e.Graphics.DrawRectangle(new Pen(Color.Yellow, 2)
            {
                Alignment = PenAlignment.Inset
            }, new Rectangle(x,y,width,height));
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
