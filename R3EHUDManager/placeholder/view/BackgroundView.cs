using R3EHUDManager.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.placeholder.view
{
    class BackgroundView:Control
    {
        private Bitmap bitmap;

        public BackgroundView()
        {
            Disposed += OnDispose;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            if (bitmap != null) bitmap.Dispose();
        }

        public void SetSize(Size size)
        {
            if (bitmap != null) bitmap.Dispose();

            bitmap = new Bitmap(GraphicalAsset.GetBackground(), size);
            Size = size;
            BackgroundImage = bitmap;
        }
    }
}
