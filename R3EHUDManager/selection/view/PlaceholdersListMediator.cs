using R3EHUDManager.placeholder.model;
using da2mvc.core.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;
using da2mvc.framework.collection.view;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListMediator : CollectionMediator<PlaceHolderCollectionModel, PlaceholderModel, PlaceholdersListView>
    {
        public PlaceholdersListMediator()
        {
            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            HandleEvent<SelectionModel, BaseEventArgs>(SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);
        }

        private void OnPlaceholderSelected(SelectionModelEventArgs args)
        {
            View.SelectPlaceholder(args.Placeholder.Name);
        }

        private void OnPlaceholderUnselected(BaseEventArgs args)
        {
            View.UnselectPlaceholder();
        }
    }
}
