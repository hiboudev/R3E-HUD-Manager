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
    class BackgroundManagerMediator : BaseMediator
    {
        public BackgroundManagerMediator()
        {
            RegisterEventListener(typeof(BackgroundCollectionModel), BackgroundCollectionModel.EVENT_BACKGROUND_REMOVED, OnBackgroundRemoved);
        }

        private void OnBackgroundRemoved(BaseEventArgs args)
        {
            ((BackgroundManagerView)View).RemoveBackground(((BackgroundModelEventArgs)args).Background);
        }
    }
}
