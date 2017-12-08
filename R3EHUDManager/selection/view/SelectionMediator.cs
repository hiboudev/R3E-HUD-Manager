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
using da2mvc.view;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.events;

namespace R3EHUDManager.selection.view
{
    class SelectionMediator : BaseMediator
    {
        public SelectionMediator()
        {
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);

            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_PLACE_HOLDER_UPDATED, OnPlaceholderUpdated);

            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);
        }

        private void OnTripleScreenChanged(BaseEventArgs args)
        {
            ((SelectionView)View).SetTripleScreen(((ScreenModelEventArgs)args).ScreenModel.IsTripleScreen);
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
