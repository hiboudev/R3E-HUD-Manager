﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.screen.model;
using da2mvc.core.events;
using R3EHUDManager.application.events;

namespace R3EHUDManager.screen.view
{
    class ZoomView : FlowLayoutPanel, IEventDispatcher
    {
        private RadioButton fitWindow;
        private RadioButton fitHeight;

        public event EventHandler MvcEventHandler;

        public static readonly int EVENT_ZOOM_LEVEL_CHANGED = EventId.New();

        private bool holdChangeEvents = false;

        public ZoomView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            AutoSize = true;
            BackColor = Color.LightGray;
            WrapContents = false;

            Label label = new Label()
            {
                AutoSize = true,
                Text = "Zoom",
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
        };

            fitWindow = new RadioButton()
            {
                AutoSize = true,
                Text = "Fit to window",
                Margin = new Padding(0),
            };

            fitHeight = new RadioButton()
            {
                AutoSize = true,
                Text = "Fit to height",
                Margin = new Padding(0),
            };

            fitWindow.CheckedChanged += OnRadioChange;
            fitHeight.CheckedChanged += OnRadioChange;

            Controls.AddRange(new Control[] { label, fitWindow, fitHeight });
        }

        private void OnRadioChange(object sender, EventArgs e)
        {
            if (holdChangeEvents || !((RadioButton)sender).Checked) return;

            if (fitWindow.Checked)
                DispatchEvent(new IntEventArgs(EVENT_ZOOM_LEVEL_CHANGED, (int)ZoomLevel.FIT_TO_WINDOW));
            else
                DispatchEvent(new IntEventArgs(EVENT_ZOOM_LEVEL_CHANGED, (int)ZoomLevel.FIT_TO_HEIGHT));
        }

        internal void SetZoomLevel(ZoomLevel zoomLevel)
        {
            holdChangeEvents = true;

            if (zoomLevel == ZoomLevel.FIT_TO_HEIGHT)
                fitHeight.Checked = true;
            else
                fitWindow.Checked = true;

            holdChangeEvents = false;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
