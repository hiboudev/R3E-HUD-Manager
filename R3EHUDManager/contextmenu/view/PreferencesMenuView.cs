using da2mvc.events;
using R3EHUDManager.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.contextmenu.view
{
    class PreferencesMenuView : AbstractContextMenuView
    {
        public const string EVENT_OPEN_APP_DATA_DIRECTORY = "openAppDataDirectory";
        public const string EVENT_OPEN_APP_INSTALL_DIRECTORY = "openAppInstallDirectory";
        public const string EVENT_OPEN_HUD_DIRECTORY = "openHudDirectory";

        public PreferencesMenuView() : base("")
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            DrawArrow = false;
            Image = new Bitmap(GraphicalAsset.GetPreferencesIcon(), 16, 16);
            Size = new Size(24, 24);

            Disposed += OnDispose;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            Image image = Image;
            Image = null;
            image.Dispose();
        }

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            ToolStripMenuItem openDataDirItem = new ToolStripMenuItem("<Open application data directory>");
            ToolStripMenuItem openInstallDirItem = new ToolStripMenuItem("<Open application install directory>");
            ToolStripMenuItem openHudDirItem = new ToolStripMenuItem("<Open HUD directory>");

            openHudDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_HUD_DIRECTORY));
            openDataDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_DATA_DIRECTORY));
            openInstallDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_INSTALL_DIRECTORY));

            return new List<ToolStripMenuItem>(new ToolStripMenuItem[] { openDataDirItem, openInstallDirItem, openHudDirItem });
        }
    }
}
