using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;
using da2mvc.view;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListMediator : BaseMediator
    {
        public PlaceholdersListMediator()
        {
            RegisterEventListener(typeof(PlaceHolderCollectionModel), PlaceHolderCollectionModel.EVENT_NEW_LAYOUT, OnPlaceholdersAdded);
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_SELECTED, OnPlaceholdersSelected);
            RegisterEventListener(typeof(SelectionModel), SelectionModel.EVENT_UNSELECTED, OnPlaceholdersUnselected);
        }

        private void OnPlaceholdersSelected(BaseEventArgs args)
        {
            ((PlaceholdersListView)View).SelectPlaceholder(((SelectionModelEventArgs)args).Placeholder.Name);
        }

        private void OnPlaceholdersUnselected(BaseEventArgs args)
        {
            ((PlaceholdersListView)View).UnselectPlaceholder();
        }

        private void OnPlaceholdersAdded(BaseEventArgs args)
        {
            ((PlaceholdersListView)View).SetPlaceholders(((PlaceHolderCollectionEventArgs)args).PlaceHolders);
        }
    }
}
