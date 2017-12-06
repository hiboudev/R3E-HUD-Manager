using da2mvc.events;
using R3EHUDManager.application.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class LoadBackgroundView : Button,IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public const string EVENT_LOAD_BACKGROUND = "loadBackground";

        public LoadBackgroundView()
        {
            Text = "Background";
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                DispatchEvent(new StringEventArgs(EVENT_LOAD_BACKGROUND, dialog.FileName));
        }
    }
}
