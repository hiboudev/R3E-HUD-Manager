using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.savestatus.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly HudOptionsParser parser;
        private readonly LocationModel locationModel;
        private readonly PlaceHolderCollectionModel collectionModel;
        private readonly ScreenModel screenModel;

        public ApplicationExitCommand(ApplicationExitEventArgs args, SaveStatusModel saveStatus, SelectedProfileModel selectedProfile,
            HudOptionsParser parser, LocationModel locationModel, PlaceHolderCollectionModel collectionModel, ScreenModel screenModel)
        {
            this.args = args;
            this.saveStatus = saveStatus;
            this.selectedProfile = selectedProfile;
            this.parser = parser;
            this.locationModel = locationModel;
            this.collectionModel = collectionModel;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            //if (saveStatus.IsSaved(SaveType.PROFILE) && saveStatus.IsSaved(SaveType.R3E_HUD)) return; // Here I prefer not to rely on it.

            string text = "There's unsaved changes:\n\n";
            bool promptUser = false;

            if (!IsProfileSaved())
            {
                // TODO check background
                text += $"* Current profile \"{selectedProfile.Selection.Name}\" has changed.\n";
                promptUser = true;
            }

            if (!IsLayoutAppliedToR3E())
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

        private bool IsProfileSaved()
        {
            if (selectedProfile.Selection == null) return true;

            return screenModel.Background.Id == selectedProfile.Selection.BackgroundId &&
                AreLayoutEquals(parser.Parse(Path.Combine(locationModel.LocalDirectoryProfiles, selectedProfile.Selection.FileName)), collectionModel.Items);
        }

        private bool IsLayoutAppliedToR3E()
        {
            return AreLayoutEquals(parser.Parse(locationModel.HudOptionsFile), collectionModel.Items);
        }

        private bool AreLayoutEquals(List<PlaceholderModel> layout1, List<PlaceholderModel> layout2)
        {
            // Actually possible if we filtered placeholders but didn't reload layout.
            if (layout1.Count != layout2.Count) return false;

            Dictionary<string, PlaceholderModel> placeholders1 = layout1.ToDictionary(x => x.Name, x => x);
            Dictionary<string, PlaceholderModel> placeholders2 = layout2.ToDictionary(x => x.Name, x => x);



            foreach (KeyValuePair<string, PlaceholderModel> keyValue in placeholders1)
            {
                if (!keyValue.Value.Position.Equals(placeholders2[keyValue.Key].Position)) return false;
                if (!keyValue.Value.Anchor.Equals(placeholders2[keyValue.Key].Anchor)) return false;
                if (!keyValue.Value.Size.Equals(placeholders2[keyValue.Key].Size)) return false;
            }

            return true;
        }
    }
}
