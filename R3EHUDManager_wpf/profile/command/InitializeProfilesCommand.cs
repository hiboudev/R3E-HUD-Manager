using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.database;
using R3EHUDManager.profile.model;
using System.Collections.Generic;

namespace R3EHUDManager.profile.command
{
    class InitializeProfilesCommand : ICommand
    {
        private readonly Database database;
        private readonly CollectionModel<ProfileModel> collection;
        private readonly SelectedProfileModel selection;

        public InitializeProfilesCommand(Database database, CollectionModel<ProfileModel> collection, SelectedProfileModel selection)
        {
            this.database = database;
            this.collection = collection;
            this.selection = selection;
        }

        public void Execute()
        {
            List<ProfileModel> profiles = database.GetAllProfiles();
            collection.Clear();
            collection.AddRange(profiles);

            //if(profiles.Count > 0)
            //{
            //    Injector.ExecuteCommand(typeof(SelectProfileCommand), new ContextMenuEventArgs("", profiles[0].Id, ""));
            //}
        }
    }
}
