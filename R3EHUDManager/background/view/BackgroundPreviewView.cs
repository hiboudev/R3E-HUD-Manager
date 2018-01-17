using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R3EHUDManager.background.view
{
    public class BackgroundPreviewView : FrameworkElement
    {
        private BitmapSource bitmap;
        private CuttingType cuttingType = CuttingType.NONE;

        internal void SetBitmap(BitmapSource bitmap)
        {
            this.bitmap = bitmap;
            InvalidateVisual();
        }

        internal void SetCuttingType(CuttingType cutting)
        {
            cuttingType = cutting;

            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (bitmap == null) return;

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);

            double panelRatio = RenderSize.Width / RenderSize.Height;
            double bitmapRatio = (double)bitmap.PixelWidth / bitmap.PixelHeight;

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

            drawingContext.DrawImage(bitmap, new Rect(marginX, marginY, bitmapSize.Width, bitmapSize.Height));

            double rectOverlayWidth = bitmapSize.Width / 3;

            if (cuttingType == CuttingType.TRIPLE_SCREEN)
            {
                drawingContext.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), 2),
                    new Point(marginX + rectOverlayWidth, marginY),
                    new Point(marginX + rectOverlayWidth, marginY + bitmapSize.Height));

                drawingContext.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), 2),
                    new Point(marginX + rectOverlayWidth * 2, marginY),
                    new Point(marginX + rectOverlayWidth * 2, marginY + bitmapSize.Height));
            }
            else if (cuttingType == CuttingType.CROP_TO_SINGLE)
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

    enum CuttingType
    {
        NONE,
        TRIPLE_SCREEN,
        CROP_TO_SINGLE
    }
}
