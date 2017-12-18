using da2mvc.core.command;
using da2mvc.framework.collection.model;
using da2mvc.framework.menubutton.events;
using R3EHUDManager.background.model;
using R3EHUDManager.huddata.model;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.layout.model;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.savestatus.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.selection.model;
using System.Collections.Generic;
using System.IO;

namespace R3EHUDManager.profile.command
{
    class SelectProfileCommand : ICommand
    {
        private readonly MenuButtonEventArgs args;
        private readonly CollectionModel<ProfileModel> profileCollection;
        private readonly SelectedProfileModel selectedProfile;
        private readonly CollectionModel<BackgroundModel> backgroundCollection;
        private readonly ScreenModel screen;
        private readonly LocationModel location;
        private readonly PlaceHolderCollectionModel placeholderCollection;
        private readonly SelectionModel selectionModel;
        private readonly LayoutSourceModel layoutSource;
        private readonly LayoutIOModel layoutIO;

        public SelectProfileCommand(MenuButtonEventArgs args, CollectionModel<ProfileModel> profileCollection, SelectedProfileModel selectedProfile,
                                    CollectionModel<BackgroundModel> backgroundCollection, ScreenModel screen, LocationModel location,
                                    PlaceHolderCollectionModel placeholderCollection, SelectionModel selectionModel, LayoutSourceModel layoutSource,
                                    LayoutIOModel layoutIO)
        {
            this.args = args;
            this.profileCollection = profileCollection;
            this.selectedProfile = selectedProfile;
            this.backgroundCollection = backgroundCollection;
            this.screen = screen;
            this.location = location;
            this.placeholderCollection = placeholderCollection;
            this.selectionModel = selectionModel;
            this.layoutSource = layoutSource;
            this.layoutIO = layoutIO;
        }

        public void Execute()
        {
            ProfileModel profile = profileCollection.Get(args.ItemId);
            BackgroundModel background = backgroundCollection.Get(profile.BackgroundId);
            List<PlaceholderModel> placeholders = layoutIO.LoadProfileLayout(profile);

            selectionModel.Unselect();

            screen.SetBackground(background);
            placeholderCollection.Clear();
            placeholderCollection.AddRange(placeholders);
            selectedProfile.SelectProfile(profile);
            layoutSource.SetSource(LayoutSourceType.PROFILE, profile.Name);
        }
    }
}
