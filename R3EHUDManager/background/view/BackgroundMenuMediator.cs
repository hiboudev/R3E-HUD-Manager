﻿using da2mvc.events;
using da2mvc.view;
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
            RegisterEventListener(typeof(BackgroundCollectionModel), BackgroundCollectionModel.EVENT_BACKGROUND_LIST_ADDED, OnListAdded);
            RegisterEventListener(typeof(BackgroundCollectionModel), BackgroundCollectionModel.EVENT_BACKGROUND_ADDED, OnBackgroundAdded);
            RegisterEventListener(typeof(BackgroundCollectionModel), BackgroundCollectionModel.EVENT_BACKGROUND_REMOVED, OnBackgroundRemoved);
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
        }

        private void OnListAdded(BaseEventArgs args)
        {
            var typedArgs = (BackgroundCollectionEventArgs)args;

            ((BackgroundMenuView)View).AddBackgrounds(typedArgs.Collection.Backgrounds);
        }

        private void OnBackgroundAdded(BaseEventArgs args)
        {
            var typedArgs = (BackgroundModelEventArgs)args;

            ((BackgroundMenuView)View).AddBackground(typedArgs.Background);
        }

        private void OnBackgroundChanged(BaseEventArgs args)
        {
            ((BackgroundMenuView)View).SetSelectedItem(((ScreenModelEventArgs)args).ScreenModel.Background.Id);
        }

        private void OnBackgroundRemoved(BaseEventArgs args)
        {
            var typedArgs = (BackgroundModelEventArgs)args;

            ((BackgroundMenuView)View).RemoveItem(typedArgs.Background.Id);
        }
    }
}
