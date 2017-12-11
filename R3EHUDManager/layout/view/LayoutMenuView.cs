using R3EHUDManager.contextmenu.view;
using R3EHUDManager.graphics;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.layout.view
{
    class LayoutMenuView : AbstractContextMenuView
    {

        public LayoutMenuView() : base("Layout")
        {
            AddItem(new ContextMenuViewItem((int)ScreenLayoutType.SINGLE, "Single screen", GraphicalAsset.GetLayoutIcon(ScreenLayoutType.SINGLE)));
            AddItem(new ContextMenuViewItem((int)ScreenLayoutType.TRIPLE, "Triple screen", GraphicalAsset.GetLayoutIcon(ScreenLayoutType.TRIPLE)));
        }

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            return new List<ToolStripMenuItem>();
        }
    }
}
