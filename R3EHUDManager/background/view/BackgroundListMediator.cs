using MVC_Framework.view;
using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using R3EHUDManager.background.events;

namespace R3EHUDManager.background.view
{
    class BackgroundListMediator : BaseMediator
    {
        public BackgroundListMediator()
        {
            RegisterEventListener(typeof(BackgroundCollectionModel), BackgroundCollectionModel.EVENT_BACKGROUND_LIST_ADDED, OnListAdded);
            RegisterEventListener(typeof(BackgroundCollectionModel), BackgroundCollectionModel.EVENT_BACKGROUND_ADDED, OnBackgroundAdded);
            RegisterEventListener(typeof(SelectedBackgroundModel), SelectedBackgroundModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
        }

        private void OnBackgroundChanged(BaseEventArgs args)
        {
            ((BackgroundListView)View).SelectBackground(((SelectedBackgroundEventArgs)args).Model.Selection);
        }

        private void OnBackgroundAdded(BaseEventArgs args)
        {
            ((BackgroundListView)View).AddBackground(((BackgroundModelEventArgs)args).Background);
        }

        private void OnListAdded(BaseEventArgs args)
        {
            ((BackgroundListView)View).SetBackgrounds(((BackgroundCollectionEventArgs)args).Collection.Backgrounds);
        }
    }
}
