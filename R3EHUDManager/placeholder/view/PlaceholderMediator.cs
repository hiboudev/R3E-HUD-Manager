using System;
using da2mvc.core.view;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.selection.events;
using R3EHUDManager.selection.model;
using R3EHUDManager.placeholder.view;
using R3EHUDManager.graphics;
using R3EHUDManager.application.events;

namespace R3EHUDManager.placeholder.view
{
    class PlaceholderMediator : BaseMediator<PlaceholderView>
    {
        public PlaceholderMediator()
        {
            HandleEvent<PlaceholderModel, PlaceHolderUpdatedEventArgs>(PlaceholderModel.EVENT_UPDATED, OnPlaceholderUpdated);

            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);

            HandleEvent<PlaceholderModel, ValidationChangedEventArgs>(PlaceholderModel.EVENT_VALIDATION_CHANGED, OnValidationChanged);

            HandleEvent<GraphicalAssetFactory, IntEventArgs>(GraphicalAssetFactory.EVENT_MOTEC_CHANGED, OnMotecChanged);
        }

        private void OnMotecChanged(IntEventArgs args)
        {
            if (View.Model.Name == PlaceholderName.MOTEC)
                View.Render();
        }

        private void OnValidationChanged(ValidationChangedEventArgs args)
        {
            if (args.Placeholder.Id != View.Model.Id)
                return;

            View.SetValidationResult(args.Result);
        }

        private void OnPlaceholderSelected(SelectionModelEventArgs args)
        {
            if (args.Placeholder.Id == View.Model.Id)
                View.IsSelected = true;
        }

        private void OnPlaceholderUnselected(SelectionModelEventArgs args)
        {
            if (args.Placeholder.Id == View.Model.Id)
                View.IsSelected = false;
        }

        private void OnPlaceholderUpdated(PlaceHolderUpdatedEventArgs args)
        {
            if (args.Placeholder.Id == View.Model.Id)
                //View.Update(args.UpdateType); // TODO WPF encore besoin de l'update type?
                View.Render();
        }
    }
}
