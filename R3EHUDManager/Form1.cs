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
using Test_MVC.events;
using Test_MVC.injection;

namespace R3EHUDManager
{
    public partial class Form1 : Form, IEventDispatcher
    {
        public event EventHandler MyEventHandler;
        public const string EVENT_SAVE_CLICKED = "saveClicked";

        public Form1()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Mappings.InitializeMappings(this);

            InitializeComponent();
            InitializeUI();

            Injector.ExecuteCommand(typeof(StartApplicationCommand));
        }

        private void InitializeUI()
        {
            //Padding = new Padding(50);
            ScreenView screenView = (ScreenView)Injector.GetInstance(typeof(ScreenView));
            screenView.Dock = DockStyle.Fill;

            Button saveButton = new Button()
            {
                Text = "Save",
            };
            saveButton.Click += (sender, args) => dispatchEvent(new BaseEventArgs(EVENT_SAVE_CLICKED));

            Controls.Add(saveButton);
            Controls.Add(screenView);
        }

        public void dispatchEvent(BaseEventArgs args)
        {
            MyEventHandler?.Invoke(this, args);
        }

    }
}
