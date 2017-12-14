﻿using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using da2mvc.core.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;
using da2mvc.core.view;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;
using da2mvc.framework.collection.events;

namespace R3EHUDManager.screen.view
{
    class ScreenMediator : BaseMediator<ScreenView>
    {
        public ScreenMediator()
        {
            HandleEvent<PlaceHolderCollectionModel, CollectionEventArgs<PlaceholderModel>>(PlaceHolderCollectionModel.EVENT_ITEMS_ADDED, OnPlaceHoldersAdded);
            HandleEvent<PlaceHolderCollectionModel, BaseEventArgs>(PlaceHolderCollectionModel.EVENT_CLEARED, OnPlaceholderCleared);

            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);
            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_ZOOM_LEVEL_CHANGED, OnZoomLevelChanged);
        }

        private void OnZoomLevelChanged(ScreenModelEventArgs args)
        {
            View.SetZoomLevel(args.ScreenModel.ZoomLevel);
        }

        private void OnTripleScreenChanged(ScreenModelEventArgs args)
        {
            View.TripleScreenChanged(args.ScreenModel);
        }

        private void OnBackgroundChanged(ScreenModelEventArgs args)
        {
            View.BackgroundChanged(args.ScreenModel);
        }

        private void OnPlaceHoldersAdded(CollectionEventArgs<PlaceholderModel> args)
        {
            View.DisplayPlaceHolders(args.ChangedItems);
        }

        private void OnPlaceholderCleared(BaseEventArgs args)
        {
            View.RemovePlaceholders();
        }
    }
}
