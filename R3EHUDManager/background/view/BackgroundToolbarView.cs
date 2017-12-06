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
    class BackgroundToolbarView : FlowLayoutPanel
    {
        public BackgroundToolbarView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            
            BackColor = Color.LightGray;

            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;

            FlowLayoutPanel hPanel = new FlowLayoutPanel()
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                //BackColor = Color.Blue,
                Margin = new Padding(Margin.Left, 0, Margin.Right, 0),
            };

            Label title = new Label()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Text = "Background",
                Font = new Font(Font.FontFamily, 7),
                TextAlign = ContentAlignment.MiddleCenter,
                //BackColor = Color.Green,
            };

            ImportBackgroundView button = (ImportBackgroundView)Injector.GetInstance(typeof(ImportBackgroundView));
            button.Margin = new Padding(button.Margin.Left, 2, button.Margin.Right, 0);

            BackgroundListView list = (BackgroundListView)Injector.GetInstance(typeof(BackgroundListView));
            list.Margin = new Padding(list.Margin.Left, list.Margin.Top, list.Margin.Right, 0);


            hPanel.Controls.Add(button);
            hPanel.Controls.Add(list);

            Controls.Add(title);
            Controls.Add(hPanel);
        }
    }
}
