using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;
using R3EHUDManager.background.model;
using R3EHUDManager.background.events;
using da2mvc.core.view;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;
using da2mvc.framework.model.events;

namespace R3EHUDManager.screen.view
{
    class ScreenMediator : BaseMediator<ScreenView>
    {
        public ScreenMediator()
        {
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_ITEMS_ADDED, OnPlaceHoldersAdded);
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_CLEARED, OnPlaceholderCleared);
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDER_UPDATED, OnPlaceHolderUpdated);

            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_SELECTED, OnPlaceHolderSelected);
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_UNSELECTED, OnPlaceHolderUnselected);

            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_ZOOM_LEVEL_CHANGED, OnZoomLevelChanged);
        }

        private void OnZoomLevelChanged(BaseEventArgs args)
        {
            View.SetZoomLevel(((ScreenModelEventArgs)args).ScreenModel.ZoomLevel);
        }

        private void OnTripleScreenChanged(BaseEventArgs args)
        {
            View.TripleScreenChanged(((ScreenModelEventArgs)args).ScreenModel);
        }

        private void OnBackgroundChanged(BaseEventArgs args)
        {
            View.BackgroundChanged(((ScreenModelEventArgs)args).ScreenModel);
        }

        private void OnPlaceHolderSelected(BaseEventArgs args)
        {
            View.SelectPlaceholder(((SelectionModelEventArgs)args).Placeholder, true);
        }

        private void OnPlaceHolderUnselected(BaseEventArgs args)
        {
            View.SelectPlaceholder(((SelectionModelEventArgs)args).Placeholder, false);
        }

        private void OnPlaceHolderUpdated(BaseEventArgs args)
        {
            PlaceHolderUpdateEventArgs typedArgs = (PlaceHolderUpdateEventArgs)args;
            View.UpdatePlaceholder(typedArgs.Placeholder, typedArgs.UpdateType);
        }

        private void OnPlaceHoldersAdded(BaseEventArgs args)
        {
            View.DisplayPlaceHolders(((CollectionEventArgs<PlaceholderModel>)args).ChangedItems);
        }

        private void OnPlaceholderCleared(BaseEventArgs args)
        {
            View.RemovePlaceholders();
        }
    }
}
