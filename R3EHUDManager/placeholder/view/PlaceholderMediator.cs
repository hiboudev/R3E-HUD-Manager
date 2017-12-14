using da2mvc.core.view;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.command;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager.selection.events;
using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private void OnValidationChanged(ValidationChangedEventArgs args)
        {
            if (args.Placeholder.Id != View.Model.Id)
                return;

            View.SetValidationResult(args.Result);
        }

        private void OnPlaceholderSelected(SelectionModelEventArgs args)
        {
            if(args.Placeholder.Id == View.Model.Id)
                View.SetSelected(true);
        }

        private void OnPlaceholderUnselected(SelectionModelEventArgs args)
        {
            if (args.Placeholder.Id == View.Model.Id)
                View.SetSelected(false);
        }

        private void OnPlaceholderUpdated(PlaceHolderUpdatedEventArgs args)
        {
            if (args.Placeholder.Id == View.Model.Id)
                View.Update(args.UpdateType);
        }
    }
}
