using da2mvc.core.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.location.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.command
{
    class DeleteBackgroundCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly BackgroundCollectionModel collectionModel;
        private readonly ScreenModel screenModel;
        private readonly Database database;
        private readonly LocationModel locationModel;
        private readonly ProfileCollectionModel profileCollection;

        public DeleteBackgroundCommand(IntEventArgs args, BackgroundCollectionModel collectionModel, ScreenModel screenModel, Database database, LocationModel locationModel,
                                        ProfileCollectionModel profileCollection)
        {
            this.args = args;
            this.collectionModel = collectionModel;
            this.screenModel = screenModel;
            this.database = database;
            this.locationModel = locationModel;
            this.profileCollection = profileCollection;
        }

        public void Execute()
        {
            BackgroundModel background = collectionModel.Get(args.Value);

            string parentProfileNames = GetParentProfileNames(background);
            if (parentProfileNames != null)
            {
                MessageBox.Show($"This background is used by one or more profiles ({parentProfileNames}), you can't delete it.", "Can't delete this background");
                return;
            }

            collectionModel.RemoveBackground(background);

            if (screenModel.Background == background)
                // Default background can't be deleted so there's always at least 1 item in the list.
                screenModel.SetBackground(collectionModel.Backgrounds[0]);

            database.DeleteBackground(background);

            string dirPath = locationModel.GetGraphicBasePath(background.DirectoryType);
            File.Delete(Path.Combine(dirPath, background.FileName));
        }

        private string GetParentProfileNames(BackgroundModel background)
        {
            string names = "";

            foreach (var profile in profileCollection.Profiles)
                if (profile.BackgroundId == background.Id)
                    names += $"{profile.Name}, ";

            if (names.Length > 0) return names.Substring(0, names.Length - 2);
            return null;
        }
    }
}
