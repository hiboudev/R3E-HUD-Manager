using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.events;
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
    class ImportBackgroundCommand : ICommand
    {
        private readonly ImportBackgroundEventArgs args;
        private readonly SelectedBackgroundModel selection;
        private readonly LocationModel locationModel;
        private readonly Database database;
        private readonly BackgroundCollectionModel collection;

        public ImportBackgroundCommand(ImportBackgroundEventArgs args, SelectedBackgroundModel selection, LocationModel locationModel, Database database, BackgroundCollectionModel collection)
        {
            this.args = args;
            this.selection = selection;
            this.locationModel = locationModel;
            this.database = database;
            this.collection = collection;
        }

        public void Execute()
        {
            string fileName = Path.GetFileName(args.FilePath);

            if (File.Exists(Path.Combine(locationModel.LocalDirectoryBackgrounds, fileName)))
            {
                fileName = GetUnusedName(locationModel.LocalDirectoryBackgrounds, fileName);
            }


            File.Copy(args.FilePath, Path.Combine(locationModel.LocalDirectoryBackgrounds, fileName));
            BackgroundModel background = BackgroundFactory.NewBackgroundModel(args.Name, fileName, BaseDirectoryType.BACKGROUNDS_DIRECTORY);
            database.AddBackground(background);
            collection.AddBackground(background);
            selection.SelectBackground(background);
        }

        private string GetUnusedName(string path, string fileName)
        {
            int counter = 0;
            string nameOnly = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            do { ++counter; }
            while (File.Exists(Path.Combine(path, $"{nameOnly}({counter}){extension}")));

            return $"{nameOnly}({counter}){extension}";
        }
    }
}
