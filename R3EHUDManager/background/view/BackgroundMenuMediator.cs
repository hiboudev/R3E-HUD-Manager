using da2mvc.core.events;
using da2mvc.core.view;
using da2mvc.framework.model;
using da2mvc.framework.model.events;
using R3EHUDManager.background.events;
using R3EHUDManager.background.model;
using R3EHUDManager.contextmenu.view;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
