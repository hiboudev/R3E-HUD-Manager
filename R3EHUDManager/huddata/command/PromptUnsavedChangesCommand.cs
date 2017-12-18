using da2mvc.core.command;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.huddata.command
{
    class PromptUnsavedChangesCommand : ICommand
    {
        private readonly UnsavedChangesEventArgs args;

        public PromptUnsavedChangesCommand(UnsavedChangesEventArgs args)
        {
            this.args = args;
        }

        public void Execute()
        {
            switch (args.ChangeType)
            {
                case UnsavedChangeType.PROFILE:
                    if (DialogResult.No == MessageBox.Show($"There's unsaved changes in profile \"{args.SourceName}\", continue anyway?", "Unsaved changes", MessageBoxButtons.YesNo))
                        args.CancelLoading();
                    break;

                case UnsavedChangeType.R3E:
                    if (DialogResult.No == MessageBox.Show($"Current layout is not saved, continue anyway?", "Unsaved changes", MessageBoxButtons.YesNo))
                        args.CancelLoading();
                    break;
            }
        }
    }
}
