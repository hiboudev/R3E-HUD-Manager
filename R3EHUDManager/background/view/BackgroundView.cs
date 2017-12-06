using R3EHUDManager.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.background.model;

namespace R3EHUDManager.background.view
{
    class BackgroundView:Control
    {
        private Bitmap bitmap;
        private Image baseBitmap;

        public BackgroundView()
        {
            DoubleBuffered = true;
            Disposed += OnDispose;
        }

        public void SetSize(Size size)
        {
            Size = size;
            RedrawBackground();
        }

        private void RedrawBackground()
        {
            if (baseBitmap == null) return;

            DisposeBitmap();

            bitmap = new Bitmap(baseBitmap, Size);
            BackgroundImage = bitmap;
        }

        internal void SetBackground(BackgroundModel model)
        {
            DisposeBaseBitmap();

            baseBitmap = model.GetBackground();
            RedrawBackground();
        }

        private void OnDispose(object sender, EventArgs e)
        {
            DisposeBitmap();
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

        private void DisposeBitmap()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }
    }
}
