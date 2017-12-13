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
            Injector.ExecuteCommand<StartApplicationCommand>();
        }

        private void InitializeUI()
        {
            MinimumSize = new Size(400, 400);
            Size = new Size(1020, 620);

            // LayoutMenuView removed since backgrounds are linked with a screen layout, this option is not useful and confusing, maybe could be added back as a layout override in profile.
            Panel topBarPanel = NewHDockPanel(DockStyle.Top, new Control[] {
                NewHToolBar( Injector.GetInstance<BackgroundMenuView>() ),
                NewHToolBar( Injector.GetInstance<ProfileMenuView>() )
                /*,NewHToolBar( (Control)Injector.GetInstance(typeof(LayoutMenuView)) })*/
            });

            Panel bottomBarPanel = NewHDockPanel(DockStyle.Bottom, new Control[] {
                NewHToolBar( Injector.GetInstance<ZoomView>() )
            });

            SelectionView selectionView = Injector.GetInstance< SelectionView>();
            selectionView.FlowDirection = FlowDirection.TopDown;
            selectionView.Margin = new Padding(selectionView.Margin.Left, selectionView.Margin.Top, selectionView.Margin.Right, 10);
            selectionView.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            PlaceholdersListView listView = Injector.GetInstance<PlaceholdersListView>();
            listView.Height = 160;
            listView.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            SettingsMenuView prefsButton = Injector.GetInstance< SettingsMenuView>();
            prefsButton.Anchor = AnchorStyles.Left;

            var directoryMenu = Injector.GetInstance<R3eDirectoryMenuView>();
            directoryMenu.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            FlowLayoutPanel leftBarPanel = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Left,
                AutoSize = true,
                WrapContents = false,
            };

            leftBarPanel.Controls.AddRange(new Control[] {
                selectionView, listView, NewButton("Apply to R3E", EVENT_SAVE_CLICKED), NewButton("Reload from R3E", EVENT_RELOAD_CLICKED),
                NewButton("Reload original", EVENT_RELOAD_DEFAULT_CLICKED), directoryMenu, prefsButton} );

            ScreenView screenView = Injector.GetInstance<ScreenView>();
            screenView.Dock = DockStyle.Fill;

            Controls.AddRange(new Control[] { screenView, topBarPanel, bottomBarPanel, leftBarPanel });
        }

        private Panel NewHDockPanel(DockStyle dock, Control[] controls)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = dock,
                AutoSize = true
            };

            foreach (var control in controls)
                panel.Controls.Add(control);

            return panel;
        }

        private Panel NewHToolBar(Control control)
        {
            Panel toolBar = new TableLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.LightGray,
            };

            control.Margin = new Padding();
            toolBar.Controls.Add(control);

            return toolBar;
        }

        private Button NewButton(string text, string eventType)
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
