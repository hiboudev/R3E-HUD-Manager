using da2mvc.core.command;
using da2mvc.core.events;
using da2mvc.framework.collection.model;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.graphics;
using R3EHUDManager.huddata.model;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.utils;
using System;
using System.IO;

namespace R3EHUDManager.profile.command
{
    class CreateProfileCommand : ICommand
    {
        private readonly StringEventArgs args;
        private readonly CollectionModel<ProfileModel> profileCollection;
        private readonly Database database;
        private readonly LocationModel location;
        private readonly ScreenModel screen;
        private readonly PlaceHolderCollectionModel placeholderCollection;
        private readonly SelectedProfileModel selectedProfile;
        private readonly LayoutIOModel layoutIO;
        private readonly GraphicalAssetFactory assetsFactory;

        public CreateProfileCommand(StringEventArgs args, CollectionModel<ProfileModel> profileCollection, Database database,
            LocationModel location, ScreenModel screen, PlaceHolderCollectionModel placeholderCollection,
            SelectedProfileModel selectedProfile, LayoutIOModel layoutIO, GraphicalAssetFactory assetsFactory)
        {
            this.args = args;
            this.profileCollection = profileCollection;
            this.database = database;
            this.location = location;
            this.screen = screen;
            this.placeholderCollection = placeholderCollection;
            this.selectedProfile = selectedProfile;
            this.layoutIO = layoutIO;
            this.assetsFactory = assetsFactory;
        }

        public void Execute()
        {
            string fileName = ToFileName(args.Value);
            string filePath = Path.Combine(location.LocalDirectoryProfiles, fileName); // TODO tester les noms de fichier
            File.Copy(location.HudTemplateFile, filePath);

            BackgroundModel background = screen.Background;

            ProfileModel newProfile = ProfileFactory.NewProfileModel(args.Value, background.Id, fileName, assetsFactory.SelectedMotec.Id);

            layoutIO.WriteProfileLayout(newProfile, placeholderCollection.Items);

            database.AddProfile(newProfile);
            profileCollection.Add(newProfile);

            selectedProfile.SelectProfile(newProfile);
            layoutIO.DispatchSaveStatus();
        }

        public static string ToFileName(string profileName)
        {
            return $"profile_{StringUtils.ToValidFileName(profileName)}.xml";
        }
    }
}
