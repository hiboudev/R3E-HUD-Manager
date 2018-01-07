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
using R3EHUDManager_wpf.selection.view;
using R3EHUDManager.selection.model;
using R3EHUDManager.userpreferences.model;

namespace R3EHUDManager.selection.view
{
    class SelectionMediator : BaseMediator<SelectionView>
    {
        public SelectionMediator()
        {
            HandleEvent<SelectionModel, SelectionModelEventArgs>(SelectionModel.EVENT_SELECTED, OnPlaceholderSelected);
            HandleEvent<SelectionModel, BaseEventArgs>(SelectionModel.EVENT_UNSELECTED, OnPlaceholderUnselected);

            HandleEvent<PlaceholderModel, PlaceHolderUpdatedEventArgs>(PlaceholderModel.EVENT_UPDATED, OnPlaceholderUpdated);

            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
            //HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnTripleScreenChanged);

            HandleEvent<UserPreferencesModel, BaseEventArgs>(UserPreferencesModel.EVENT_CULTURE_CHANGED, OnCultureChanged);
        }

        private void OnCultureChanged(BaseEventArgs args)
        {
            View.RefreshCulture();
        }

        //private void OnTripleScreenChanged(ScreenModelEventArgs args)
        //{
        //    View.TripleScreenChanged(args.ScreenModel.Layout);
        //}

        private void OnBackgroundChanged(ScreenModelEventArgs args)
        {
            View.TripleScreenChanged(args.ScreenModel.Layout);
        }

        private void OnPlaceholderUpdated(PlaceHolderUpdatedEventArgs args)
        {
            if (View.Selection == null)
                return;

            if (View.Selection.Id == args.Placeholder.Id)
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
