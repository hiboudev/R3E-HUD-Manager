using R3EHUDManager.application.command;
using System;
using System.Drawing;
using System.Windows.Forms;
using da2mvc.core.events;
using da2mvc.core.injection;
using R3EHUDManager.selection.view;
using R3EHUDManager.screen.view;
using R3EHUDManager.settings.view;
using R3EHUDManager.background.view;
using R3EHUDManager.profile.view;
using R3EHUDManager.location.view;
using R3EHUDManager.huddata.view;
using R3EHUDManager.application.events;

namespace R3EHUDManager
{
    public partial class Form1 : Form, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_PROCESS_EXIT = EventId.New();

        public Form1()
        {
            //CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            Mappings.InitializeMappings(this);

            InitializeComponent();
            InitializeUI();

            Shown += OnFormShown;
            FormClosing += (sender, args) => DispatchEvent(new ApplicationExitEventArgs(EVENT_PROCESS_EXIT, args));
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            Injector.ExecuteCommand<StartApplicationCommand>();
        }

        private void InitializeUI()
        {
            MinimumSize = new Size(620, 520);
            Size = new Size(1020, 620);

            Panel topBarPanel = NewHDockPanel(DockStyle.Top, new Control[] {
                NewHToolBar( Injector.GetInstance<BackgroundMenuView>() ),
                NewHToolBar( Injector.GetInstance<ProfileMenuView>() )
            });

            Panel bottomBarPanel = NewHDockPanel(DockStyle.Bottom, new Control[] {
                NewHToolBar( Injector.GetInstance<ZoomView>() ),
                Injector.GetInstance<LayoutSourceView>(),
            });

            SelectionView selectionView = Injector.GetInstance< SelectionView>();
            selectionView.FlowDirection = FlowDirection.TopDown;
            selectionView.Margin = new Padding(selectionView.Margin.Left, selectionView.Margin.Top, selectionView.Margin.Right, 10);

            PlaceholdersListView listView = Injector.GetInstance<PlaceholdersListView>();

            ReloadLayoutView reloadView = Injector.GetInstance<ReloadLayoutView>();
            reloadView.Margin = new Padding(reloadView.Margin.Left, 10, reloadView.Margin.Right, reloadView.Margin.Bottom);

            SettingsMenuView prefsButton = Injector.GetInstance< SettingsMenuView>();
            prefsButton.Anchor = AnchorStyles.Left;

            var directoryMenu = Injector.GetInstance<R3eDirectoryMenuView>();

            TableLayoutPanel leftBarPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                ColumnCount = 1,
            };

            AddToTable(leftBarPanel, selectionView, SizeType.AutoSize, AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            AddToTable(leftBarPanel, listView, SizeType.Percent, 100, AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            AddToTable(leftBarPanel, reloadView, SizeType.AutoSize, AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            AddToTable(leftBarPanel, directoryMenu, SizeType.AutoSize, AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            AddToTable(leftBarPanel, prefsButton, SizeType.AutoSize, AnchorStyles.Left);

            ScreenView screenView = Injector.GetInstance<ScreenView>();
            screenView.Dock = DockStyle.Fill;

            Controls.AddRange(new Control[] { screenView, topBarPanel, bottomBarPanel, leftBarPanel });
        }

        private void AddToTable(TableLayoutPanel table, Control control, SizeType sizeType, AnchorStyles anchor)
        {
            control.Anchor = anchor;
            table.RowStyles.Add(new RowStyle(sizeType));
            table.Controls.Add(control);
        }

        private void AddToTable(TableLayoutPanel table, Control control, SizeType sizeType, int height, AnchorStyles anchor)
        {
            control.Anchor = anchor;
            table.RowStyles.Add(new RowStyle(sizeType, height));
            table.Controls.Add(control);
        }

        private Panel NewHDockPanel(DockStyle dock, Control[] controls)
        {
            TableLayoutPanel panel = new TableLayoutPanel()
            {
                Dock = dock,
                AutoSize = true,
                RowCount = 1,
                ColumnCount = controls.Length,
            };

            int count = 0;
            foreach (var control in controls)
            {
                ColumnStyle columnStyle = count++ == controls.Length - 1 ? new ColumnStyle(SizeType.Percent, 100) : new ColumnStyle(SizeType.AutoSize);

                control.Anchor |= AnchorStyles.Left;

                panel.ColumnStyles.Add(columnStyle);
                panel.Controls.Add(control);
            }

            return panel;
        }

        private Panel NewHToolBar(Control control)
        {
            Panel toolBar = new TableLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.LightGray,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
        };

            control.Margin = new Padding();
            toolBar.Controls.Add(control);

            return toolBar;
        }

        private Button NewButton(string text, int eventId)
        {
            Button button = new Button()
            {
                Text = text,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };
            button.Click += (sender, args) => DispatchEvent(new BaseEventArgs(eventId));
            return button;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

    }
}
