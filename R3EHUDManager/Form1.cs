﻿using R3EHUDManager.application.command;
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

namespace R3EHUDManager
{
    public partial class Form1 : Form, IEventDispatcher
    {
        public event EventHandler MyEventHandler;
        public const string EVENT_SAVE_CLICKED = "saveClicked";
        public const string EVENT_RELOAD_CLICKED = "reloadClicked";
        public const string EVENT_RELOAD_DEFAULT_CLICKED = "reloadDefaultClicked";

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

            FlowLayoutPanel buttonsPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Top,
                Height = 30,
            };

            buttonsPanel.Controls.Add(GetButton("Save", EVENT_SAVE_CLICKED));
            buttonsPanel.Controls.Add(GetButton("Reload", EVENT_RELOAD_CLICKED));
            buttonsPanel.Controls.Add(GetButton("Default", EVENT_RELOAD_DEFAULT_CLICKED));

            Controls.Add(buttonsPanel);
            Controls.Add(screenView);
        }

        private Button GetButton(string text, string eventType)
        {
            Button button = new Button()
            {
                Text = text,
            };
            button.Click += (sender, args) => dispatchEvent(new BaseEventArgs(eventType));
            return button;
        }

        public void dispatchEvent(BaseEventArgs args)
        {
            MyEventHandler?.Invoke(this, args);
        }

    }
}
