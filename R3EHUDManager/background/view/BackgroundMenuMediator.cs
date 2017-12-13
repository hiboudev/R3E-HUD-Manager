using da2mvc.core.view;
using da2mvc.framework.collection.events;
using da2mvc.framework.collection.model;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;

namespace R3EHUDManager.background.view
{
    class BackgroundMenuMediator : BaseMediator<BackgroundMenuView>
    {
        public BackgroundMenuMediator()
        {
            HandleEvent<CollectionModel<BackgroundModel>, CollectionEventArgs<BackgroundModel>>(CollectionModel<BackgroundModel>.EVENT_ITEMS_ADDED, OnBackgroundAdded);
            HandleEvent<CollectionModel<BackgroundModel>, CollectionEventArgs<BackgroundModel>>(CollectionModel<BackgroundModel>.EVENT_ITEMS_REMOVED, OnBackgroundRemoved);
            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
        }

        private void OnBackgroundAdded(CollectionEventArgs<BackgroundModel> args)
        {
            var typedArgs = args;

            View.AddBackgrounds(typedArgs.ChangedItems);
        }

        private void OnBackgroundRemoved(CollectionEventArgs<BackgroundModel> args)
        {
            var typedArgs = args;

            foreach (var background in typedArgs.ChangedItems)
                View.RemoveItem(background.Id);
        }

        private void OnBackgroundChanged(ScreenModelEventArgs args)
        {
            View.SetSelectedItem(args.ScreenModel.Background.Id);
        }
    }
}
