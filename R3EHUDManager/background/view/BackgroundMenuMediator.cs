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
    class BackgroundMenuMediator : BaseMediator
    {
        public BackgroundMenuMediator()
        {
            RegisterEventListener(typeof(CollectionModel<BackgroundModel>), CollectionModel<BackgroundModel>.EVENT_ITEMS_ADDED, OnBackgroundAdded);
            RegisterEventListener(typeof(CollectionModel<BackgroundModel>), CollectionModel<BackgroundModel>.EVENT_ITEMS_REMOVED, OnBackgroundRemoved);
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
        }

        private void OnBackgroundAdded(BaseEventArgs args)
        {
            var typedArgs = (CollectionEventArgs<BackgroundModel>)args;

            ((BackgroundMenuView)View).AddBackgrounds(typedArgs.ChangedItems);
        }

        private void OnBackgroundRemoved(BaseEventArgs args)
        {
            var typedArgs = (CollectionEventArgs<BackgroundModel>)args;

            foreach(var background in typedArgs.ChangedItems)
                ((BackgroundMenuView)View).RemoveItem(background.Id);
        }

        private void OnBackgroundChanged(BaseEventArgs args)
        {
            ((BackgroundMenuView)View).SetSelectedItem(((ScreenModelEventArgs)args).ScreenModel.Background.Id);
        }
    }
}
