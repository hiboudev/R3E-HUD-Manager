using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.background.events;
using da2mvc.core.view;
using da2mvc.framework.model;
using da2mvc.framework.model.events;

namespace R3EHUDManager.background.view
{
    class BackgroundManagerMediator : BaseMediator<BackgroundManagerView>
    {
        public BackgroundManagerMediator()
        {
            RegisterEventListener<CollectionEventArgs<BackgroundModel>>(typeof(CollectionModel<BackgroundModel>), CollectionModel<BackgroundModel>.EVENT_ITEMS_REMOVED, OnBackgroundRemoved);
        }

        private void OnBackgroundRemoved(CollectionEventArgs<BackgroundModel> args)
        {
            foreach(var background in args.ChangedItems)
                View.RemoveBackground(background);

        }
    }
}
