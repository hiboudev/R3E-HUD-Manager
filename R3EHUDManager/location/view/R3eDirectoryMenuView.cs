using R3EHUDManager.contextmenu.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.location.view
{
    class R3eDirectoryMenuView : AbstractContextMenuView
    {
        public R3eDirectoryMenuView() : base("Dir")
        {
            //TitleMaxLength = 40;
            //Width = 230;
            Font = new Font(Font.FontFamily, 7);
            AutoSize = false;
        }

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            return new List<ToolStripMenuItem>();
        }
    }
}
