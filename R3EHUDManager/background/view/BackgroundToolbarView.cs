using da2mvc.injection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class BackgroundToolbarView : Panel
    {
        private readonly BackgroundMenuView menuView;

        public BackgroundToolbarView(BackgroundMenuView menuView)
        {
            this.menuView = menuView;
            InitializeUI();
        }

        private void InitializeUI()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            BackColor = Color.LightGray;

            Controls.Add(menuView);
        }
    }
}
