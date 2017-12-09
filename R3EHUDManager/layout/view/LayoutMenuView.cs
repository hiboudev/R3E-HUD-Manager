using R3EHUDManager.contextmenu.view;
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

        //public const string EVENT_TRIPLE_LAYOUT_SELECTED = "tripleLayoutSelected";
        //public const string EVENT_SINGLE_LAYOUT_SELECTED = "singleLayoutSelected";

        public LayoutMenuView() : base("Layout")
        {
            AddItem(new ContextMenuViewItem((int)ScreenLayoutType.SINGLE, "Single screen"));
            AddItem(new ContextMenuViewItem((int)ScreenLayoutType.TRIPLE, "Triple screen"));
        }

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            return new List<ToolStripMenuItem>();
        }

        //    isTripleBox = new CheckBox()
        //    {
        //        Text = "Triple screen (testing purpose)",
        //            AutoSize = true,
        //        };

        //    internal void SetTripleScreen(bool isTriple)
        //    {
        //        holdIsTripleEvent = true;
        //        isTripleBox.Checked = isTriple;
        //        holdIsTripleEvent = false;
        //    }

        //    isTripleBox.CheckedChanged += OnTripleChecked;
        //    }

        //private void OnTripleChecked(object sender, EventArgs e)
        //{
        //    if (holdIsTripleEvent) return;

        //    DispatchEvent(new BooleanEventArgs(EVENT_TRIPLE_SCREEN_CHANGED, isTripleBox.Checked));
        //}

        //private void OnTripleScreenChanged(BaseEventArgs args)
        //{
        //    ((SelectionView)View).SetTripleScreen(((ScreenModelEventArgs)args).ScreenModel.IsTripleScreen);
        //}

        //    RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);
        //public const string EVENT_TRIPLE_SCREEN_CHANGED = "tripleScreenChanged";
    }
}
