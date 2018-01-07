using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using da2mvc.core.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;
using da2mvc.core.view;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;
using da2mvc.framework.collection.events;
using System;
using R3EHUDManager_wpf.screen.view;

namespace R3EHUDManager.screen.view
{
    class ScreenMediator : BaseMediator<ScreenView>
    {
        public ScreenMediator()
        {
            HandleEvent<PlaceHolderCollectionModel, CollectionEventArgs<PlaceholderModel>>(PlaceHolderCollectionModel.EVENT_ITEMS_ADDED, OnPlaceHoldersAdded);
            HandleEvent<PlaceHolderCollectionModel, BaseEventArgs>(PlaceHolderCollectionModel.EVENT_CLEARED, OnPlaceholderCleared);

            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
            //HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);
            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_ZOOM_LEVEL_CHANGED, OnZoomLevelChanged);

            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);

            //HandleEvent<PlaceholderModel, PlaceHolderUpdatedEventArgs>(PlaceholderModel.EVENT_UPDATED, OnPlaceholderUpdated);
        }

        private void OnPlaceHoldersAdded(CollectionEventArgs<PlaceholderModel> args)
        {
            View.DisplayPlaceHolders(args.ChangedItems);
        }

        private void OnPlaceholderCleared(BaseEventArgs args)
        {
            View.RemovePlaceholders();
        }

        //private void OnPlaceholderUpdated(PlaceHolderUpdatedEventArgs args)
        //{
        //    View.PlaceHolderUpdated();
        //}

        private void OnPlaceholderSelected(SelectionModelEventArgs args)
        {
            View.PlaceholderSelected(args.Placeholder);
        }

        private void OnPlaceholderUnselected(SelectionModelEventArgs args)
        {
            View.PlaceholderUnselected();
        }

        private void OnZoomLevelChanged(ScreenModelEventArgs args)
        {
            View.SetZoomLevel(args.ScreenModel.ZoomLevel);
        }

        //private void OnTripleScreenChanged(ScreenModelEventArgs args)
        //{
        //    View.TripleScreenChanged(args.ScreenModel);
        //}

        private void OnBackgroundChanged(ScreenModelEventArgs args)
        {
            View.BackgroundChanged(args.ScreenModel);
        }
    }
}
