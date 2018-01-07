using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R3EHUDManager_wpf.background.view
{
    public class BackgroundPreviewView : FrameworkElement
    {
        private BitmapSource bitmap;
        private bool lineStyle;
        private bool drawRectOverlay;

        internal void SetBitmap(BitmapSource bitmap)
        {
            this.bitmap = bitmap;
            InvalidateVisual();
        }

        internal void DrawRectangle(bool lineStyle)
        {
            if (bitmap == null) return;

            drawRectOverlay = true;
            this.lineStyle = lineStyle;

            InvalidateVisual();
        }

        internal void ClearRectangle()
        {
            drawRectOverlay = false;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (bitmap == null) return;

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            double panelRatio = RenderSize.Width / RenderSize.Height;
            double bitmapRatio = (bitmap.PixelWidth / bitmap.PixelHeight);

            Size bitmapSize = new Size();
            if (bitmapRatio > panelRatio)
            {
                bitmapSize.Width = RenderSize.Width;
                bitmapSize.Height = RenderSize.Width / bitmapRatio;
            }
            else
            {
                bitmapSize.Height = RenderSize.Height;
                bitmapSize.Width = RenderSize.Height * bitmapRatio;
            }

            double marginX = (RenderSize.Width - bitmapSize.Width) / 2;
            double marginY = (RenderSize.Height - bitmapSize.Height) / 2;

            if (!drawRectOverlay)
            {
                drawingContext.DrawImage(bitmap, new Rect(marginX, marginY, bitmapSize.Width, bitmapSize.Height));
                return;
            }

            drawingContext.DrawImage(bitmap, new Rect(marginX, marginY, bitmapSize.Width, bitmapSize.Height));

            double rectOverlayWidth = bitmapSize.Width / 3;

            if (lineStyle)
            {
                drawingContext.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), 2),
                    new Point(marginX + rectOverlayWidth, marginY),
                    new Point(marginX + rectOverlayWidth, marginY + bitmapSize.Height));

                drawingContext.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), 2),
                    new Point(marginX + rectOverlayWidth * 2, marginY),
                    new Point(marginX + rectOverlayWidth * 2, marginY + bitmapSize.Height));
            }
            else
            {
                drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(140, 0, 0, 0)), null,
                    new Rect(
                        marginX,
                        marginY,
                        rectOverlayWidth,
                        bitmapSize.Height));

                drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(140, 0, 0, 0)), null,
                    new Rect(
                        marginX + rectOverlayWidth * 2,
                        marginY,
                        rectOverlayWidth,
                        bitmapSize.Height));
            }
        }
    }
}
