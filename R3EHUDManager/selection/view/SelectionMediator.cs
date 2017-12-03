using MVC_Framework.view;
using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using R3EHUDManager.selection.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.placeholder.events;

namespace R3EHUDManager.selection.view
{
    class SelectionMediator : BaseMediator
    {
        public SelectionMediator()
        {
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);

            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDER_UPDATED, OnPlaceholderUpdated);
        }

        private void OnPlaceholderUpdated(BaseEventArgs args)
        {
            if (((SelectionView)View).Selection == ((PlaceHolderUpdateEventArgs)args).Placeholder)
            {
                ((SelectionView)View).UpdateData();
            }
        }

        private void OnPlaceholderSelected(BaseEventArgs args)
        {
            ((SelectionView)View).SetSelected(((SelectionModelEventArgs)args).Placeholder);
        }

        private void OnPlaceholderUnselected(BaseEventArgs args)
        {
            ((SelectionView)View).Unselect();
        }
    }
}
