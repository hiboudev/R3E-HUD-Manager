using da2mvc.core.events;
using da2mvc.framework.menubutton.view;
using R3EHUDManager.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace R3EHUDManager.settings.view
{
    class SettingsMenuView : SettingsMenuButtonView
    {
        public static readonly int EVENT_OPEN_APP_DATA_DIRECTORY = EventId.New();
        public static readonly int EVENT_OPEN_APP_INSTALL_DIRECTORY = EventId.New();
        public static readonly int EVENT_OPEN_HUD_DIRECTORY = EventId.New();

        public SettingsMenuView()
        {
            InitializeUI();
        }

        protected override string Title => "";

        private void InitializeUI()
        {
            DrawArrow = false;
            Image = new Bitmap(GraphicalAsset.GetPreferencesIcon(), 16, 16);
            Size = new Size(24, 24);

            Disposed += OnDispose;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            Image.Dispose();
        }

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            ToolStripMenuItem openDataDirItem = new ToolStripMenuItem("<Open application data directory>");
            ToolStripMenuItem openInstallDirItem = new ToolStripMenuItem("<Open application install directory>");
            ToolStripMenuItem openHudDirItem = new ToolStripMenuItem("<Open HUD directory>");

            openHudDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_HUD_DIRECTORY));
            openDataDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_DATA_DIRECTORY));
            openInstallDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_INSTALL_DIRECTORY));

            return new List<ToolStripMenuItem>(new ToolStripMenuItem[] { openHudDirItem, openDataDirItem, openInstallDirItem });
        }
    }
}
