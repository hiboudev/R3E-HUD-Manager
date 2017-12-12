using da2mvc.core.command;
using da2mvc.core.injection;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.database;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.command
{
    class InitializeProfilesCommand : ICommand
    {
        private readonly Database database;
        private readonly ProfileCollectionModel collection;
        private readonly SelectedProfileModel selection;

        public InitializeProfilesCommand(Database database, ProfileCollectionModel collection, SelectedProfileModel selection)
        {
            this.database = database;
            this.collection = collection;
            this.selection = selection;
        }

        public void Execute()
        {
            List<ProfileModel> profiles = database.GetAllProfiles();
            collection.AddRange(profiles);

            //if(profiles.Count > 0)
            //{
            //    Injector.ExecuteCommand(typeof(SelectProfileCommand), new ContextMenuEventArgs("", profiles[0].Id, ""));
            //}
        }
    }
}
