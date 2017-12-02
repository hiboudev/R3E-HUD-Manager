using MVC_Framework.view;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;

namespace R3EHUDManager.placeholder.view
{
    class ScreenMediator : BaseMediator
    {
        public ScreenMediator()
        {
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDERS_ADDED, OnPlaceHoldersAdded);
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDER_UPDATED, OnPlaceHolderUpdated);
        }

        private void OnPlaceHolderUpdated(BaseEventArgs args)
        {
            PlaceHolderModelEventArgs typedArgs = (PlaceHolderModelEventArgs)args;
            ((ScreenView)View).UpdatePlaceholder(typedArgs.Placeholder, typedArgs.UpdateType);
        }

        private void OnPlaceHoldersAdded(BaseEventArgs args)
        {
            ((ScreenView)View).DisplayPlaceHolders(((PlaceHolderCollectionEventArgs)args).PlaceHolders);
        }
    }
}
