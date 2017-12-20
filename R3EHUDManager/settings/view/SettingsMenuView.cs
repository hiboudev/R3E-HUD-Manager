using da2mvc.core.events;
using da2mvc.core.injection;
using da2mvc.framework.menubutton.view;
using R3EHUDManager.apppresentation.view;
using R3EHUDManager.graphics;
using R3EHUDManager.huddata.view;
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
        public static readonly int EVENT_SHOW_PRESENTATION = EventId.New();

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

        protected override List<ToolStripItem> GetBuiltInItems()
        {
            ToolStripMenuItem openDataDirItem = new ToolStripMenuItem("Open application data directory");
            ToolStripMenuItem openInstallDirItem = new ToolStripMenuItem("Open application install directory");
            ToolStripMenuItem openHudDirItem = new ToolStripMenuItem("Open HUD directory");
            
            ToolStripMenuItem openFilteredPlaceholders = new ToolStripMenuItem("Manage filtered placeholders...");
            ToolStripMenuItem showPresentation = new ToolStripMenuItem("Quick presentation");
            ToolStripMenuItem openSettings = new ToolStripMenuItem("Settings...");

            openHudDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_HUD_DIRECTORY));
            openDataDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_DATA_DIRECTORY));
            openInstallDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_INSTALL_DIRECTORY));
            openFilteredPlaceholders.Click += OpenFiltersManager;
            showPresentation.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_SHOW_PRESENTATION));
            openSettings.Click += OpenSettings;

            return new List<ToolStripItem>(new ToolStripItem[] {
                openHudDirItem,
                openDataDirItem,
                openInstallDirItem,
                new ToolStripSeparator(),
                showPresentation,
                new ToolStripSeparator(),
                openFilteredPlaceholders,
                openSettings
            });
        }

        private void OpenSettings(object sender, EventArgs e)
        {
            var dialog = Injector.GetInstance<SettingsView>();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
            }
            dialog.Dispose();
        }

        private void OpenFiltersManager(object sender, EventArgs e)
        {
            var dialog = Injector.GetInstance<PlaceholderBlackListView>();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
            }
            dialog.Dispose();
        }
    }
}
