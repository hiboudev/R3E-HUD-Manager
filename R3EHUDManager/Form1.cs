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
using da2mvc.events;
using da2mvc.injection;
using R3EHUDManager.selection.view;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

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

            FlowLayoutPanel toolBarPanel = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Left,
                AutoSize = true,
                WrapContents = false,
            };

            SelectionView selectionView = (SelectionView)Injector.GetInstance(typeof(SelectionView));
            selectionView.FlowDirection = FlowDirection.TopDown;
            selectionView.Margin = new Padding(selectionView.Margin.Left, selectionView.Margin.Top, selectionView.Margin.Right, 10);

            PlaceholdersListView listView = (PlaceholdersListView)Injector.GetInstance(typeof(PlaceholdersListView));
            listView.Height = 200;
            listView.Anchor = AnchorStyles.Left | AnchorStyles.Right;


            toolBarPanel.Controls.Add(selectionView);
            toolBarPanel.Controls.Add(listView);

            toolBarPanel.Controls.Add(GetButton("Reload", EVENT_RELOAD_CLICKED));
            toolBarPanel.Controls.Add(GetButton("Original", EVENT_RELOAD_DEFAULT_CLICKED));
            toolBarPanel.Controls.Add(GetButton("Save", EVENT_SAVE_CLICKED));

            Controls.Add(screenView);
            Controls.Add(toolBarPanel);
            
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
