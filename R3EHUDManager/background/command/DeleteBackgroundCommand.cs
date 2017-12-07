using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.command
{
    class DeleteBackgroundCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly BackgroundCollectionModel collectionModel;
        private readonly SelectedBackgroundModel selectionModel;
        private readonly Database database;
        private readonly LocationModel locationModel;

        public DeleteBackgroundCommand(IntEventArgs args, BackgroundCollectionModel collectionModel, SelectedBackgroundModel selectionModel, Database database, LocationModel locationModel)
        {
            this.args = args;
            this.collectionModel = collectionModel;
            this.selectionModel = selectionModel;
            this.database = database;
            this.locationModel = locationModel;
        }

        public void Execute()
        {
            BackgroundModel background = collectionModel.Get(args.Value);


            collectionModel.RemoveBackground(background);

            if (selectionModel.Selection == background)
                // Default background can't be deleted so there's always at least 1 item in the list.
                selectionModel.SelectBackground(collectionModel.Backgrounds[0]);

            database.DeleteBackground(background);

            string dirPath = locationModel.GetGraphicBasePath(background.DirectoryType);
            File.Delete(Path.Combine(dirPath, background.FileName));
        }
    }
}
