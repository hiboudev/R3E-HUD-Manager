using da2mvc.core.command;
using da2mvc.framework.menubutton.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;

namespace R3EHUDManager.screen.command
{
    class ChangeScreenLayoutCommand : ICommand
    {
        private readonly MenuButtonEventArgs args;
        private readonly ScreenModel screenModel;
        private readonly PlaceHolderCollectionModel collectionModel;

        public ChangeScreenLayoutCommand(MenuButtonEventArgs args, ScreenModel screenModel, PlaceHolderCollectionModel collectionModel)
        {
            this.args = args;
            this.screenModel = screenModel;
            this.collectionModel = collectionModel;
        }

        public void Execute()
        {
            ScreenLayoutType layout = (ScreenLayoutType)args.ItemId;
            
            if (screenModel.Layout == layout) return;

            if (layout == ScreenLayoutType.SINGLE)
                ScreenUtils.PromptUserIfOutsideOfCenterScreenPlaceholders(collectionModel);
            
            screenModel.SetLayout(layout);
        }
    }
}
