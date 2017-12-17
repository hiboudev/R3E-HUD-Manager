using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.huddata.command;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.command;
using R3EHUDManager.profile.model;
using R3EHUDManager.savestatus.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.savestatus.command
{
    class UpdateSaveStatusCommand : ICommand
    {
        private readonly BaseEventArgs args;
        private readonly SaveStatusModel saveStatus;
        private readonly SelectedProfileModel selectedProfile;
        private static readonly Dictionary<int, SetStatus> actions;
        private delegate void SetStatus(SaveStatusModel model, SelectedProfileModel selectedProfile);

        static UpdateSaveStatusCommand()
        {
            actions = new Dictionary<int, SetStatus>
            {
                {PlaceholderModel.EVENT_UPDATED, (model, selectedProfile) => model.SetChanged(SaveType.PROFILE | SaveType.R3E_HUD) },
                {SaveProfileCommand.EVENT_PROFILE_CHANGES_SAVED, (model, selectedProfile) => model.SetSaved(SaveType.PROFILE) },
                {SaveHudCommand.EVENT_HUD_LAYOUT_APPLIED, (model, selectedProfile) => model.SetSaved(SaveType.R3E_HUD) },
                {LoadHudDataCommand.EVENT_HUD_LAYOUT_LOADED, (model, selectedProfile) => {model.SetSaved(SaveType.R3E_HUD); if(selectedProfile.Selection != null) model.SetChanged(SaveType.PROFILE); } },
                {CreateProfileCommand.EVENT_PROFILE_CREATED, (model, selectedProfile) => model.SetSaved(SaveType.PROFILE) },
                {ScreenModel.EVENT_BACKGROUND_CHANGED, (model, selectedProfile) => { if(selectedProfile.Selection != null) model.SetChanged(SaveType.PROFILE); } },
                {SelectedProfileModel.EVENT_SELECTION_CHANGED,  (model, selectedProfile) => {model.SetSaved(SaveType.PROFILE); model.SetChanged(SaveType.R3E_HUD); }  },
                {SelectedProfileModel.EVENT_SELECTION_CLEARED,  (model, selectedProfile) => model.SetSaved(SaveType.PROFILE)},
                {ReloadDefaultHudDataCommand.EVENT_DEFAULT_HUD_LAYOUT_LOADED,  (model, selectedProfile) => {if(selectedProfile.Selection != null) model.SetChanged(SaveType.PROFILE); model.SetChanged(SaveType.R3E_HUD); } },
            };
        }

        public UpdateSaveStatusCommand(BaseEventArgs args, SaveStatusModel saveStatus, SelectedProfileModel selectedProfile)
        {
            this.args = args;
            this.saveStatus = saveStatus;
            this.selectedProfile = selectedProfile;
        }

        public void Execute()
        {
            actions[args.EventId].Invoke(saveStatus, selectedProfile);
        }
    }
}
