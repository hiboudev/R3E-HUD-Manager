using da2mvc.core.command;
using R3EHUDManager.application.events;
using R3EHUDManager.huddata.model;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace R3EHUDManager.application.command
{
    class ApplicationExitCommand : ICommand
    {
        private readonly ApplicationExitEventArgs args;
        private readonly LayoutIOModel layoutIO;
        private readonly SelectedProfileModel selectedProfile;
        private readonly HudOptionsParser parser;
        private readonly LocationModel locationModel;
        private readonly PlaceHolderCollectionModel collectionModel;
        private readonly ScreenModel screenModel;

        public ApplicationExitCommand(ApplicationExitEventArgs args, LayoutIOModel layoutIO)
        {
            this.args = args;
            this.layoutIO = layoutIO;
        }

        public void Execute()
        {
            //if (saveStatus.IsSaved(SaveType.PROFILE) && saveStatus.IsSaved(SaveType.R3E_HUD)) return; // Here I prefer not to rely on it.

            string text = "There's unsaved changes:\n\n";
            bool promptUser = false;

            // TODO rename UnsavedChangeType to SavedChangeType ?
            UnsavedChangeType saved = layoutIO.GetSaveStatus();

            if (!saved.HasFlag(UnsavedChangeType.PROFILE))
            {
                // TODO check background
                text += $"* Current profile \"{selectedProfile.Selection.Name}\" has changed.\n";
                promptUser = true;
            }

            if (!saved.HasFlag(UnsavedChangeType.R3E))
            {
                text += "* Current layout is not applied to R3E.\n";
                promptUser = true;
            }

            text += "\nExit anyway?";

            if (promptUser && DialogResult.No == MessageBox.Show(text, "Unsaved changes", MessageBoxButtons.YesNo))
            {
                args.FormArgs.Cancel = true;
            }
        }
    }
}
