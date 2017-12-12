using R3EHUDManager.application.command;
using R3EHUDManager.placeholder.view;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using da2mvc.core.events;
using da2mvc.core.injection;
using R3EHUDManager.selection.view;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using R3EHUDManager.graphics;
using R3EHUDManager.contextmenu.view;
using R3EHUDManager.screen.view;
using R3EHUDManager.settings.view;
using R3EHUDManager.background.view;
using R3EHUDManager.settings;
using R3EHUDManager.layout.view;
using R3EHUDManager.profile.view;
using R3EHUDManager.location.view;

namespace R3EHUDManager
{
    public partial class Form1 : Form, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;

        public const string EVENT_SAVE_CLICKED = "saveClicked";
        public const string EVENT_RELOAD_CLICKED = "reloadClicked";
        public const string EVENT_RELOAD_DEFAULT_CLICKED = "reloadDefaultClicked";

        public Form1()
        {
            //CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            Mappings.InitializeMappings(this);

            InitializeComponent();
            InitializeUI();

            Shown += OnFormShown;
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            Injector.ExecuteCommand(typeof(StartApplicationCommand));
        }

        private void InitializeUI()
        {
            MinimumSize = new Size(400, 400);

            ScreenView screenView = (ScreenView)Injector.GetInstance(typeof(ScreenView));
            screenView.Dock = DockStyle.Fill;


            FlowLayoutPanel topBarPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                AutoSize = true
            };

            FlowLayoutPanel leftBarPanel = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Left,
                AutoSize = true,
                WrapContents = false,
            };

            Panel bottomBarPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Bottom,
                AutoSize = true
            };

            SelectionView selectionView = (SelectionView)Injector.GetInstance(typeof(SelectionView));
            selectionView.FlowDirection = FlowDirection.TopDown;
            selectionView.Margin = new Padding(selectionView.Margin.Left, selectionView.Margin.Top, selectionView.Margin.Right, 10);
            selectionView.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            PlaceholdersListView listView = (PlaceholdersListView)Injector.GetInstance(typeof(PlaceholdersListView));
            listView.Height = 160;
            listView.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            SettingsMenuView prefsButton = (SettingsMenuView)Injector.GetInstance(typeof(SettingsMenuView));
            prefsButton.Anchor = AnchorStyles.Left;

            leftBarPanel.Controls.Add(selectionView);
            leftBarPanel.Controls.Add(listView);

            leftBarPanel.Controls.Add(GetButton("Apply to R3E", EVENT_SAVE_CLICKED));
            leftBarPanel.Controls.Add(GetButton("Reload from R3E", EVENT_RELOAD_CLICKED));
            leftBarPanel.Controls.Add(GetButton("Reload original", EVENT_RELOAD_DEFAULT_CLICKED));

            var directoryMenu = (Control)Injector.GetInstance(typeof(R3eDirectoryMenuView));
            directoryMenu.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            leftBarPanel.Controls.Add(directoryMenu);

            leftBarPanel.Controls.Add(prefsButton);

            topBarPanel.Controls.Add((Control)Injector.GetInstance(typeof(BackgroundToolbarView)));
            // Since backgrounds are linked with a screen layout, this option is not useful and confusing, maybe could be added back as a layout override in profile.
            //topBarPanel.Controls.Add((Control)Injector.GetInstance(typeof(LayoutToolbarView)));
            topBarPanel.Controls.Add((Control)Injector.GetInstance(typeof(ProfileToolbarView)));

            bottomBarPanel.Controls.Add((Control)Injector.GetInstance(typeof(ZoomView)));

            Controls.Add(screenView);
            Controls.Add(topBarPanel);
            Controls.Add(bottomBarPanel);
            Controls.Add(leftBarPanel);

        }

        private Button GetButton(string text, string eventType)
        {
            Button button = new Button()
            {
                Text = text,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };
            button.Click += (sender, args) => DispatchEvent(new BaseEventArgs(eventType));
            return button;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

    }
}
