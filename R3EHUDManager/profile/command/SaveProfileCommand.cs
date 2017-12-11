using da2mvc.command;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.command
{
    class SaveProfileCommand : ICommand
    {
        private readonly SelectedProfileModel profileSelection;
        private readonly HudOptionsParser parser;
        private readonly PlaceHolderCollectionModel placeholderCollection;
        private readonly LocationModel location;
        private readonly ScreenModel screen;
        private readonly Database database;

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
            ProfileModel profile = profileSelection.Selection;
            string filePath = Path.Combine(location.LocalDirectoryProfiles, profile.HudFilePath);
            BackgroundModel background = screen.Background;

            profile.BackgroundId = background.Id;
            database.UpdateProfile(profile);
            parser.Write(filePath, placeholderCollection.Placeholders);
        }
    }
}
