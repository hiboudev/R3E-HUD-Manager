using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.screen.model;
using da2mvc.events;
using R3EHUDManager.application.events;

namespace R3EHUDManager.screen.view
{
    class ZoomView : FlowLayoutPanel, IEventDispatcher
    {
        private RadioButton fitWindow;
        private RadioButton fitHeight;

        public event EventHandler MvcEventHandler;
        public const string EVENT_ZOOM_LEVEL_CHANGED = "zoomLevelChanged";

        private bool holdChangeEvents = false;

        public ZoomView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            AutoSize = true;
            BackColor = Color.LightGray;

            Label label = new Label()
            {
                AutoSize = true,
                Text = "Zoom",
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            };

            fitWindow = new RadioButton()
            {
                AutoSize = true,
                Text = "Fit window",
                Margin = new Padding(0),
        };

            fitHeight = new RadioButton()
            {
                AutoSize = true,
                Text = "Fit height",
                Margin = new Padding(0),
            };

            fitWindow.CheckedChanged += OnRadioChange;
            fitHeight.CheckedChanged += OnRadioChange;

            Controls.AddRange(new Control[] {label, fitWindow, fitHeight });
        }

        private void OnRadioChange(object sender, EventArgs e)
        {
            if (holdChangeEvents) return;

            if (fitWindow.Checked)
                DispatchEvent(new IntEventArgs(EVENT_ZOOM_LEVEL_CHANGED, (int)ZoomLevel.FIT_WINDOW));
            else
                DispatchEvent(new IntEventArgs(EVENT_ZOOM_LEVEL_CHANGED, (int)ZoomLevel.FIT_HEIGHT));
        }

        internal void SetZoomLevel(ZoomLevel zoomLevel)
        {
            holdChangeEvents = true;

            if (zoomLevel == ZoomLevel.FIT_HEIGHT)
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
