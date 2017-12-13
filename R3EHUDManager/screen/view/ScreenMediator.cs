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
            RegisterEventListener<CollectionEventArgs<PlaceholderModel>>(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_ITEMS_ADDED, OnPlaceHoldersAdded);
            RegisterEventListener<BaseEventArgs>(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_CLEARED, OnPlaceholderCleared);
            RegisterEventListener<PlaceHolderUpdateEventArgs>(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDER_UPDATED, OnPlaceHolderUpdated);

            RegisterEventListener<SelectionModelEventArgs>(typeof(SelectionModel), SelectionModel.EVENT_SELECTED, OnPlaceHolderSelected);
            RegisterEventListener<SelectionModelEventArgs>(typeof(SelectionModel), SelectionModel.EVENT_UNSELECTED, OnPlaceHolderUnselected);

            RegisterEventListener<ScreenModelEventArgs>(typeof(ScreenModel), ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
            RegisterEventListener<ScreenModelEventArgs>(typeof(ScreenModel), ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);
            RegisterEventListener<ScreenModelEventArgs>(typeof(ScreenModel), ScreenModel.EVENT_ZOOM_LEVEL_CHANGED, OnZoomLevelChanged);
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

        private void OnPlaceHolderSelected(SelectionModelEventArgs args)
        {
            View.SelectPlaceholder(args.Placeholder, true);
        }

        private void OnPlaceHolderUnselected(SelectionModelEventArgs args)
        {
            View.SelectPlaceholder(args.Placeholder, false);
        }

        private void OnPlaceHolderUpdated(PlaceHolderUpdateEventArgs args)
        {
            View.UpdatePlaceholder(args.Placeholder, args.UpdateType);
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
