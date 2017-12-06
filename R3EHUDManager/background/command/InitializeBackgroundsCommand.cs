using da2mvc.command;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.command
{
    class InitializeBackgroundsCommand : ICommand
    {
        private readonly Database database;
        private readonly BackgroundCollectionModel backgroundCollection;
        private readonly SelectedBackgroundModel selectedBackground;

        public InitializeBackgroundsCommand(Database database, BackgroundCollectionModel backgroundCollection, SelectedBackgroundModel selectedBackground)
        {
            this.database = database;
            this.backgroundCollection = backgroundCollection;
            this.selectedBackground = selectedBackground;
        }

        public void Execute()
        {
            List<BackgroundModel> backgrounds = database.GetAllBackgrounds();

            if(backgrounds.Count == 0)
            {
                //selectedBackground
                BackgroundModel defaultBackground = BackgroundFactory.NewBackgroundModel("Default", "background.png", BaseDirectoryType.GRAPHICAL_ASSETS);
                backgroundCollection.SetBackgrounds(new List<BackgroundModel>(new BackgroundModel[] { defaultBackground }));
                database.AddBackground(defaultBackground);
                selectedBackground.SelectBackground(defaultBackground);
            }
            else
            {
                backgroundCollection.SetBackgrounds(backgrounds);
                selectedBackground.SelectBackground(backgrounds[0]);
            }
        }
    }
}
