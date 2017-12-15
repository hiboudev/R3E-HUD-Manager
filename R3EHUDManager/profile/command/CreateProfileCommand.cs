using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.savestatus.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.utils;
using System.IO;

namespace R3EHUDManager.profile.command
{
    class CreateProfileCommand : ICommand
    {
        private readonly StringEventArgs args;
        private readonly CollectionModel<ProfileModel> profileCollection;
        private readonly Database database;
        private readonly HudOptionsParser parser;
        private readonly LocationModel location;
        private readonly ScreenModel screen;
        private readonly PlaceHolderCollectionModel placeholderCollection;
        private readonly SelectedProfileModel selectedProfile;

        public CreateProfileCommand(StringEventArgs args, CollectionModel<ProfileModel> profileCollection, Database database, 
            HudOptionsParser parser, LocationModel location, ScreenModel screen, PlaceHolderCollectionModel placeholderCollection,
            SelectedProfileModel selectedProfile)
        {
            this.args = args;
            this.profileCollection = profileCollection;
            this.database = database;
            this.parser = parser;
            this.location = location;
            this.screen = screen;
            this.placeholderCollection = placeholderCollection;
            this.selectedProfile = selectedProfile;
        }

        public void Execute()
        {
            string fileName = ToFileName(args.Value);
            string filePath = Path.Combine(location.LocalDirectoryProfiles, fileName); // TODO tester les noms de fichier
            File.Copy(location.HudTemplateFile, filePath);

            parser.Write(filePath, placeholderCollection.Items);

            BackgroundModel background = screen.Background;

            ProfileModel newProfile = ProfileFactory.NewProfileModel(args.Value, background.Id, fileName);

            database.AddProfile(newProfile);
            profileCollection.Add(newProfile);

            selectedProfile.SelectProfile(newProfile);
        }

        public static string ToFileName(string profileName)
        {
            return $"profile_{StringUtils.ToValidFileName(profileName)}.xml";
        }
    }
}
