using da2mvc.core.command;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.screen.model;
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
        private readonly ScreenModel screenModel;

        public InitializeBackgroundsCommand(Database database, BackgroundCollectionModel backgroundCollection, ScreenModel screenModel)
        {
            this.database = database;
            this.backgroundCollection = backgroundCollection;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            List<BackgroundModel> backgrounds = database.GetAllBackgrounds();

            if(backgrounds.Count == 0)
            {
                BackgroundModel defaultBackground = BackgroundFactory.NewBackgroundModel("Default", "background.png", BaseDirectoryType.GRAPHICAL_ASSETS, true, ScreenLayoutType.SINGLE); // TODO temp
                backgroundCollection.SetBackgrounds(new List<BackgroundModel>(new BackgroundModel[] { defaultBackground }));
                database.AddBackground(defaultBackground);
                screenModel.SetBackground(defaultBackground);
            }
            else
            {
                backgroundCollection.SetBackgrounds(backgrounds);
                screenModel.SetBackground(backgrounds[0]);
            }
        }
    }
}
