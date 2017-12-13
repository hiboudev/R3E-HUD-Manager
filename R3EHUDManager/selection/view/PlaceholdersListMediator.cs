using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;
using da2mvc.core.view;
using da2mvc.framework.model.events;
using da2mvc.framework.view;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListMediator : CollectionMediator<PlaceHolderCollectionModel, PlaceholderModel, PlaceholdersListView>
    {
        public PlaceholdersListMediator()
        {
            RegisterEventListener<SelectionModelEventArgs>(typeof(SelectionModel), SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            RegisterEventListener<BaseEventArgs>(typeof(SelectionModel), SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);
        }

        private void OnPlaceholderSelected(BaseEventArgs args)
        {
            View.SelectPlaceholder(((SelectionModelEventArgs)args).Placeholder.Name);
        }

        private void OnPlaceholderUnselected(BaseEventArgs args)
        {
            View.UnselectPlaceholder();
        }
    }
}
