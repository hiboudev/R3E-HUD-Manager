using R3EHUDManager.placeholder.model;
using da2mvc.core.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.events;
using da2mvc.framework.collection.view;
using System;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.selection.view;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListMediator : CollectionMediator<PlaceHolderCollectionModel, PlaceholderModel, PlaceholderListView>
    {
        public PlaceholdersListMediator()
        {
            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);

            HandleEvent<PlaceholderModel, ValidationChangedEventArgs>(PlaceholderModel.EVENT_VALIDATION_CHANGED, OnValidationChanged);
        }

        private void OnValidationChanged(ValidationChangedEventArgs args)
        {
            View.SetValidationResult(args.Placeholder, args.Result);
        }

        private void OnPlaceholderSelected(SelectionModelEventArgs args)
        {
            View.SelectPlaceholder(args.Placeholder.Id);
        }

        private void OnPlaceholderUnselected(SelectionModelEventArgs args)
        {
            View.UnselectPlaceholder(args.Placeholder.Id);
        }
    }
}
