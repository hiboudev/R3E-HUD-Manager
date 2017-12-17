using da2mvc.core.command;
using R3EHUDManager.application.events;
using R3EHUDManager.profile.model;
using R3EHUDManager.savestatus.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.application.command
{
    class ApplicationExitCommand : ICommand
    {
        private readonly ApplicationExitEventArgs args;
        private readonly SaveStatusModel saveStatus;
        private readonly SelectedProfileModel selectedProfile;

        public ApplicationExitCommand(ApplicationExitEventArgs args, SaveStatusModel saveStatus, SelectedProfileModel selectedProfile)
        {
            this.args = args;
            this.saveStatus = saveStatus;
            this.selectedProfile = selectedProfile;
        }

        public void Execute()
        {
            if (saveStatus.IsSaved(SaveType.PROFILE) && saveStatus.IsSaved(SaveType.R3E_HUD)) return;

            string text = "Possibly unsaved changes:\n\n";

            if (!saveStatus.IsSaved(SaveType.PROFILE))
                text += $"* Current profile \"{selectedProfile.Selection.Name}\".\n";

            if (!saveStatus.IsSaved(SaveType.R3E_HUD))
                text += "* Layout not applied to R3E.\n";

            text += "\nExit anyway?";

            if (DialogResult.No == MessageBox.Show(text, "Unsaved changes", MessageBoxButtons.YesNo))
            {
                args.FormArgs.Cancel = true;
            }
        }
    }
}
