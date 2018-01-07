using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.screen.model;
using System.Collections.Generic;

namespace R3EHUDManager.background.command
{
    class InitializeBackgroundsCommand : ICommand
    {
        private readonly Database database;
        private readonly CollectionModel<BackgroundModel> backgroundCollection;
        private readonly ScreenModel screenModel;

        public InitializeBackgroundsCommand(Database database, CollectionModel<BackgroundModel> backgroundCollection, ScreenModel screenModel)
        {
            this.database = database;
            this.backgroundCollection = backgroundCollection;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            List<BackgroundModel> backgrounds = database.GetAllBackgrounds();

            if (backgrounds.Count == 0)
            {
                BackgroundModel defaultBackground = BackgroundFactory.NewBackgroundModel("Default", "background.png", BaseDirectoryType.GRAPHICAL_ASSETS, true, ScreenLayoutType.SINGLE);
                backgroundCollection.AddRange(new List<BackgroundModel>(new BackgroundModel[] { defaultBackground }));
                database.AddBackground(defaultBackground);
                screenModel.SetBackground(defaultBackground);
            }
            else
            {
                backgroundCollection.AddRange(backgrounds);
                screenModel.SetBackground(backgrounds[0]);
            }
        }
    }
}
