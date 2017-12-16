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
        private static readonly Dictionary<int, SetStatus> actions;
        private delegate void SetStatus(SaveStatusModel model);

        static UpdateSaveStatusCommand()
        {
            actions = new Dictionary<int, SetStatus>
            {
                {PlaceholderModel.EVENT_UPDATED, (model) => model.SetChanged(SaveType.PROFILE | SaveType.R3E_HUD) },
                {SaveProfileCommand.EVENT_PROFILE_CHANGES_SAVED, (model) => model.SetSaved(SaveType.PROFILE) },
                {SaveHudCommand.EVENT_HUD_LAYOUT_APPLIED, (model) => model.SetSaved(SaveType.R3E_HUD) },
                {LoadHudDataCommand.EVENT_HUD_LAYOUT_LOADED, (model) => model.SetSaved(SaveType.R3E_HUD) },
                {CreateProfileCommand.EVENT_PROFILE_CREATED, (model) => model.SetSaved(SaveType.PROFILE) },
                {ScreenModel.EVENT_BACKGROUND_CHANGED, (model) => model.SetChanged(SaveType.PROFILE) },
                {SelectedProfileModel.EVENT_SELECTION_CHANGED,  (model) => {model.SetSaved(SaveType.PROFILE); model.SetChanged(SaveType.R3E_HUD);}  },
                {SelectedProfileModel.EVENT_SELECTION_CLEARED,  (model) => model.SetSaved(SaveType.PROFILE)},
            };
        }

        public UpdateSaveStatusCommand(BaseEventArgs args, SaveStatusModel saveStatus)
        {
            this.args = args;
            this.saveStatus = saveStatus;
        }

        public void Execute()
        {
            actions[args.EventId].Invoke(saveStatus);
        }
    }
}
