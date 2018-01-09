using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.location.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.application.view;
using System.IO;
using System.Windows.Forms;

namespace R3EHUDManager.background.command
{
    class DeleteBackgroundCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly CollectionModel<BackgroundModel> backgroundCollection;
        private readonly ScreenModel screenModel;
        private readonly Database database;
        private readonly LocationModel locationModel;
        private readonly CollectionModel<ProfileModel> profileCollection;

        public DeleteBackgroundCommand(IntEventArgs args, CollectionModel<BackgroundModel> backgroundCollection, ScreenModel screenModel, Database database, LocationModel locationModel,
                                        CollectionModel<ProfileModel> profileCollection)
        {
            this.args = args;
            this.backgroundCollection = backgroundCollection;
            this.screenModel = screenModel;
            this.database = database;
            this.locationModel = locationModel;
            this.profileCollection = profileCollection;
        }

        public void Execute()
        {
            BackgroundModel background = backgroundCollection.Get(args.Value);

            string parentProfileNames = GetParentProfileNames(background);
            if (parentProfileNames != null)
            {
                MessageBoxView.Show("Can't delete this background", $"This background is used by one or more profiles ({parentProfileNames}), you can't delete it.");
                return;
            }

            backgroundCollection.Remove(background);

            if (screenModel.Background == background)
                // Default background can't be deleted so there's always at least 1 item in the list.
                screenModel.SetBackground(backgroundCollection.Items[0]);

            database.DeleteBackground(background);

            string dirPath = locationModel.GetGraphicBasePath(background.DirectoryType);
            File.Delete(Path.Combine(dirPath, background.FileName));
        }

        private string GetParentProfileNames(BackgroundModel background)
        {
            string names = "";

            foreach (var profile in profileCollection.Items)
                if (profile.BackgroundId == background.Id)
                    names += $"{profile.Name}, ";

            if (names.Length > 0) return names.Substring(0, names.Length - 2);
            return null;
        }
    }
}
