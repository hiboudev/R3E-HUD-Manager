using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.database;
using R3EHUDManager.location.model;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.command
{
    class DeleteProfileCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly ProfileCollectionModel profileCollection;
        private readonly SelectedProfileModel profileSelection;
        private readonly Database database;
        private readonly LocationModel location;

        public DeleteProfileCommand(IntEventArgs args, ProfileCollectionModel profileCollection, SelectedProfileModel profileSelection,
                                    Database database, LocationModel location)
        {
            this.args = args;
            this.profileCollection = profileCollection;
            this.profileSelection = profileSelection;
            this.database = database;
            this.location = location;
        }

        public void Execute()
        {
            ProfileModel profile = profileCollection.Get(args.Value);
            String filePath = Path.Combine(location.LocalDirectoryProfiles, profile.fileName);

            if(profileSelection.Selection == profile)
                profileSelection.SelectNone();

            profileCollection.Remove(profile);
            database.DeleteProfile(profile);
            File.Delete(filePath);
        }
    }
}
