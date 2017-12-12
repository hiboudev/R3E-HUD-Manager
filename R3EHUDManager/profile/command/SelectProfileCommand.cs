using da2mvc.core.command;
using da2mvc.framework.model;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.contextmenu.events;
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
    class SelectProfileCommand : ICommand
    {
        private readonly ContextMenuEventArgs args;
        private readonly ProfileCollectionModel profileCollection;
        private readonly SelectedProfileModel selectedProfile;
        private readonly CollectionModel<BackgroundModel> backgroundCollection;
        private readonly ScreenModel screen;
        private readonly HudOptionsParser parser;
        private readonly LocationModel location;
        private readonly PlaceHolderCollectionModel placeholderCollection;

        public SelectProfileCommand(ContextMenuEventArgs args, ProfileCollectionModel profileCollection, SelectedProfileModel selectedProfile,
                                    CollectionModel<BackgroundModel> backgroundCollection, ScreenModel screen, HudOptionsParser parser, LocationModel location,
                                    PlaceHolderCollectionModel placeholderCollection)
        {
            this.args = args;
            this.profileCollection = profileCollection;
            this.selectedProfile = selectedProfile;
            this.backgroundCollection = backgroundCollection;
            this.screen = screen;
            this.parser = parser;
            this.location = location;
            this.placeholderCollection = placeholderCollection;
        }

        public void Execute()
        {
            ProfileModel profile = profileCollection.Get(args.ItemId);
            BackgroundModel background = backgroundCollection.Get(profile.BackgroundId);
            List<PlaceholderModel> placeholders = parser.Parse(Path.Combine(location.LocalDirectoryProfiles, profile.fileName));

            screen.SetBackground(background);
            placeholderCollection.Clear();
            placeholderCollection.AddRange(placeholders);
            selectedProfile.SelectProfile(profileCollection.Get(args.ItemId));
        }
    }
}
