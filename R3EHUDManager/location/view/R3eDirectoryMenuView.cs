using da2mvc.framework.menubutton.view;
using R3EHUDManager.location.model;
using System.Drawing;

namespace R3EHUDManager.location.view
{
    class R3eDirectoryMenuView : MenuButtonView<R3eDirectoryModel>
    {
        public R3eDirectoryMenuView()
        {
            Font = new Font(Font.FontFamily, 7);
            AutoSize = false;
        }

        protected override string Title => "Dir";
    }
}
