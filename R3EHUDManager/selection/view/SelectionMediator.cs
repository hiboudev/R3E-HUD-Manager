using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.selection.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.placeholder.events;
using da2mvc.core.view;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.events;

namespace R3EHUDManager.selection.view
{
    class SelectionMediator : BaseMediator<SelectionView>
    {
        public SelectionMediator()
        {
            RegisterEventListener<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            RegisterEventListener<SelectionModel, BaseEventArgs>(SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);

            RegisterEventListener<PlaceHolderCollectionModel, PlaceHolderUpdateEventArgs>(PlaceHolderCollectionModel.EVENT_PLACE_HOLDER_UPDATED, OnPlaceholderUpdated);

            RegisterEventListener<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
            RegisterEventListener<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);
        }

        private void OnTripleScreenChanged(ScreenModelEventArgs args)
        {
            View.TripleScreenChanged(args.ScreenModel.Layout);
        }

        private void OnBackgroundChanged(ScreenModelEventArgs args)
        {
            View.TripleScreenChanged(args.ScreenModel.Layout);
        }

        private void OnPlaceholderUpdated(PlaceHolderUpdateEventArgs args)
        {
            if (View.Selection == args.Placeholder)
                View.UpdateData();
        }

        private void OnPlaceholderSelected(SelectionModelEventArgs args)
        {
            View.SetSelected(args.Placeholder);
        }

        private void OnPlaceholderUnselected(BaseEventArgs args)
        {
            View.Unselect();
        }
    }
}
