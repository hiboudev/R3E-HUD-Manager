﻿using da2mvc.core.command;
using da2mvc.framework.collection.model;
using da2mvc.framework.menubutton.events;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;
using R3EHUDManager.huddata.model;
using R3EHUDManager.location.model;
using R3EHUDManager.motec.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.selection.model;
using System.Collections.Generic;

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
        private readonly LayoutIOModel layoutIO;
        private readonly GraphicalAssetFactory assetFactory;
        private readonly CollectionModel<MotecModel> motecCollection;

        public SelectProfileCommand(MenuButtonEventArgs args, CollectionModel<ProfileModel> profileCollection, SelectedProfileModel selectedProfile,
                                    CollectionModel<BackgroundModel> backgroundCollection, ScreenModel screen, LocationModel location,
                                    PlaceHolderCollectionModel placeholderCollection, SelectionModel selectionModel,
                                    LayoutIOModel layoutIO, GraphicalAssetFactory assetFactory, CollectionModel<MotecModel> motecCollection)
        {
            this.args = args;
            this.profileCollection = profileCollection;
            this.selectedProfile = selectedProfile;
            this.backgroundCollection = backgroundCollection;
            this.screen = screen;
            this.location = location;
            this.placeholderCollection = placeholderCollection;
            this.selectionModel = selectionModel;
            this.layoutIO = layoutIO;
            this.assetFactory = assetFactory;
            this.motecCollection = motecCollection;
        }

        public void Execute()
        {
            ProfileModel profile = profileCollection.Get(args.ItemId);
            List<PlaceholderModel> placeholders = layoutIO.LoadProfileLayout(profile);

            if (placeholders == null)
                return;
            // TODO gestion des erreurs, chargemetn de fichiers, bitmaps...
            BackgroundModel background = backgroundCollection.Get(profile.BackgroundId);

            selectionModel.Unselect();
            screen.SetBackground(background);
            placeholderCollection.Clear();
            placeholderCollection.AddRange(placeholders);
            assetFactory.SetMotec(motecCollection.Get(profile.MotecId));
            selectedProfile.SelectProfile(profile);
            layoutIO.DispatchSaveStatus();
        }
    }
}
