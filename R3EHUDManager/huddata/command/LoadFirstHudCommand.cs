using da2mvc.core.command;
using da2mvc.core.injection;
using da2mvc.framework.collection.model;
using da2mvc.framework.menubutton.events;
using R3EHUDManager.database;
using R3EHUDManager.profile.command;
using R3EHUDManager.profile.model;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.command
{
    class LoadFirstHudCommand : ICommand
    {
        private readonly UserPreferencesModel preferences;
        private readonly CollectionModel<ProfileModel> profiles;

        public LoadFirstHudCommand(UserPreferencesModel preferences, CollectionModel<ProfileModel> profiles)
        {
            this.preferences = preferences;
            this.profiles = profiles;
        }

        public void Execute()
        {
            if (!LoadLastProfile())
                Injector.ExecuteCommand<LoadHudDataCommand>();
        }

        private bool LoadLastProfile()
        {
            if (preferences.LastProfileId == -1) return false;

            try
            {
                if (profiles.Get(preferences.LastProfileId) == null) return false;

                Injector.ExecuteCommand<SelectProfileCommand>(new MenuButtonEventArgs(1, preferences.LastProfileId, ""));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
