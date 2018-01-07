using da2mvc.core.view;
using da2mvc.framework.collection.events;
using da2mvc.framework.collection.model;
using da2mvc.framework.collection.view;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;

namespace R3EHUDManager.background.view
{
    class BackgroundMenuMediator : CollectionMediator<CollectionModel<BackgroundModel>, BackgroundModel, BackgroundMenuView>
    {
        public BackgroundMenuMediator()
        {
            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
        }

        private void OnBackgroundChanged(ScreenModelEventArgs args)
        {
            View.SetSelectedItem(args.ScreenModel.Background.Id);
        }
    }
}
