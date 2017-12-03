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
            //MessageBox.Show(Application.UserAppDataPath);
            //FormBorderStyle = FormBorderStyle.FixedSingle;
            //MaximizeBox = false;

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
            //Padding = new Padding(50);
            ScreenView screenView = (ScreenView)Injector.GetInstance(typeof(ScreenView));
            screenView.Dock = DockStyle.Fill;

            FlowLayoutPanel buttonsPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 30,
            };

            buttonsPanel.Controls.Add(GetButton("Reload", EVENT_RELOAD_CLICKED));
            buttonsPanel.Controls.Add(GetButton("Original", EVENT_RELOAD_DEFAULT_CLICKED));
            buttonsPanel.Controls.Add(GetButton("Save", EVENT_SAVE_CLICKED));

            SelectionView selectionView = (SelectionView)Injector.GetInstance(typeof(SelectionView));
            buttonsPanel.Controls.Add(selectionView);

            Controls.Add(screenView);
            Controls.Add(buttonsPanel);
        }

        private Button GetButton(string text, string eventType)
        {
            Button button = new Button()
            {
                Text = text,
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
