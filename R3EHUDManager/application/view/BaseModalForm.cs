using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.application.view
{
    class BaseModalForm : Form
    {
        public BaseModalForm(string title)
        {
            InitializeUI(title);
        }

        private void InitializeUI(string title)
        {
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;

            Text = title;

            MinimumSize = new Size(100, 100);
            Size = new Size(300, 200);
            Padding = new Padding(6);
        }
    }
}
