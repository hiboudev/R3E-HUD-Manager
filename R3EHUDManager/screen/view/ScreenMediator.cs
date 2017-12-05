using MVC_Framework.view;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;

namespace R3EHUDManager.screen.view
{
    class ScreenMediator : BaseMediator
    {
        public ScreenMediator()
        {
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDERS_ADDED, OnPlaceHoldersAdded);
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDER_UPDATED, OnPlaceHolderUpdated);
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_SELECTED, OnPlaceHolderSelected);
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_UNSELECTED, OnPlaceHolderUnselected);
        }

        private void OnPlaceHolderSelected(BaseEventArgs args)
        {
            ((ScreenView)View).SelectPlaceholder(((SelectionModelEventArgs)args).Placeholder, true);
        }

        private void OnPlaceHolderUnselected(BaseEventArgs args)
        {
            ((ScreenView)View).SelectPlaceholder(((SelectionModelEventArgs)args).Placeholder, false);
        }

        private void OnPlaceHolderUpdated(BaseEventArgs args)
        {
            PlaceHolderUpdateEventArgs typedArgs = (PlaceHolderUpdateEventArgs)args;
            ((ScreenView)View).UpdatePlaceholder(typedArgs.Placeholder, typedArgs.UpdateType);
        }

        private void OnPlaceHoldersAdded(BaseEventArgs args)
        {
            ((ScreenView)View).DisplayPlaceHolders(((PlaceHolderCollectionEventArgs)args).PlaceHolders);
        }
    }
}
