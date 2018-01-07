//using da2mvc.core.command;
//using da2mvc.framework.menubutton.events;
//using R3EHUDManager.database;
//using R3EHUDManager.placeholder.model;
//using R3EHUDManager.screen.model;
//using R3EHUDManager.screen.utils;
//using R3EHUDManager.userpreferences.model;

//namespace R3EHUDManager.screen.command
//{
//    class ChangeScreenLayoutCommand : ICommand
//    {
//        private readonly MenuButtonEventArgs args;
//        private readonly ScreenModel screenModel;
//        private readonly PlaceHolderCollectionModel collectionModel;
//        private readonly UserPreferencesModel preferences;
//        private readonly Database database;

//        public ChangeScreenLayoutCommand(MenuButtonEventArgs args, ScreenModel screenModel, PlaceHolderCollectionModel collectionModel, UserPreferencesModel preferences,
//            Database database)
//        {
//            this.args = args;
//            this.screenModel = screenModel;
//            this.collectionModel = collectionModel;
//            this.preferences = preferences;
//            this.database = database;
//        }

//        public void Execute()
//        {
//            ScreenLayoutType layout = (ScreenLayoutType)args.ItemId;

//            if (screenModel.Layout == layout) return;

//            if (layout == ScreenLayoutType.SINGLE)
//                ScreenUtils.PromptUserIfOutsideOfCenterScreenPlaceholders(collectionModel, preferences, database);

//            screenModel.SetLayout(layout);
//        }
//    }
//}
