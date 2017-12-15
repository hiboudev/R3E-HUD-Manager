using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.application.events;
using R3EHUDManager.database;
using R3EHUDManager.location.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.savestatus.model;
using System;
using System.IO;

namespace R3EHUDManager.profile.command
{
    class DeleteProfileCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly CollectionModel<ProfileModel> profileCollection;
        private readonly SelectedProfileModel profileSelection;
        private readonly Database database;
        private readonly LocationModel location;

        public DeleteProfileCommand(IntEventArgs args, CollectionModel<ProfileModel> profileCollection, SelectedProfileModel profileSelection,
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

            if (profileSelection.Selection == profile)
            {
                profileSelection.SelectNone();
            }

            profileCollection.Remove(profile);
            database.DeleteProfile(profile);
            File.Delete(filePath);
        }
    }
}
