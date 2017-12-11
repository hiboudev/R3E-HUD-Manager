using da2mvc.command;
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
        private readonly ProfileCollectionModel collection;
        private readonly SelectedProfileModel selection;

        public InitializeProfilesCommand(ProfileCollectionModel collection, SelectedProfileModel selection)
        {
            this.collection = collection;
            this.selection = selection;
        }

        public void Execute()
        {
            ProfileModel profile1 = new ProfileModel(1, "profile 1", 1, "");
            collection.Add(profile1);
            collection.Add(new ProfileModel(2, "profile 2", 1, ""));

            selection.SelectProfile(profile1);
        }
    }
}
