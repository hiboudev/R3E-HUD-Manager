using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.events;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using System.IO;

namespace R3EHUDManager.profile.command
{
    class SaveProfileCommand : EventDispatcher, ICommand
    {
        private readonly SelectedProfileModel profileSelection;
        private readonly HudOptionsParser parser;
        private readonly PlaceHolderCollectionModel placeholderCollection;
        private readonly LocationModel location;
        private readonly ScreenModel screen;
        private readonly Database database;
        public const string EVENT_PROFILE_CHANGES_SAVED = "profileChangesSaved";

        public SaveProfileCommand(SelectedProfileModel profileSelection, HudOptionsParser parser, PlaceHolderCollectionModel placeholderCollection,
            LocationModel location, ScreenModel screen, Database database)
        {
            this.profileSelection = profileSelection;
            this.parser = parser;
            this.placeholderCollection = placeholderCollection;
            this.location = location;
            this.screen = screen;
            this.database = database;
        }

        public void Execute()
        {
            if (profileSelection.Selection == null) return;

            ProfileModel profile = profileSelection.Selection;
            string filePath = Path.Combine(location.LocalDirectoryProfiles, profile.fileName);
            BackgroundModel background = screen.Background;

            profile.BackgroundId = background.Id;
            database.UpdateProfile(profile);
            parser.Write(filePath, placeholderCollection.Placeholders);

            DispatchEvent(new ProfileEventArgs(EVENT_PROFILE_CHANGES_SAVED, profile));
        }
    }
}
