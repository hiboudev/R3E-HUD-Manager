using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.layout.view
{
    class LayoutToolbarView : TableLayoutPanel // TODO généraliser avec BackgroundToolbarView
    {
        private readonly LayoutMenuView menuView;

        public LayoutToolbarView(LayoutMenuView menuView)
        {
            this.menuView = menuView;
            InitializeUI();
        }

        private void InitializeUI()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            BackColor = Color.LightGray;

            menuView.Margin = new Padding();

            Controls.Add(menuView);
        }
    }
}
