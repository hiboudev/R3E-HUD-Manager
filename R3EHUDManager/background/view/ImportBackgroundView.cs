using da2mvc.events;
using da2mvc.injection;
using R3EHUDManager.application.events;
using R3EHUDManager.background.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class ImportBackgroundView : Button,IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public const string EVENT_IMPORT_BACKGROUND = "importBackground";

        public ImportBackgroundView()
        {
            Text = "Import";
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var nameDialog = (PromptBackgroundNameView)Injector.GetInstance(typeof(PromptBackgroundNameView));

                if(nameDialog.ShowDialog() == DialogResult.OK)
                {
                    DispatchEvent(new ImportBackgroundEventArgs(EVENT_IMPORT_BACKGROUND, nameDialog.BackgroundName, fileDialog.FileName));
                }
                nameDialog.Dispose();
            }

            fileDialog.Dispose();
        }
    }
}
