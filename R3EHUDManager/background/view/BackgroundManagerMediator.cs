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
            RegisterEventListener(typeof(CollectionModel<BackgroundModel>), CollectionModel<BackgroundModel>.EVENT_ITEMS_REMOVED, OnBackgroundRemoved);
        }

        private void OnBackgroundRemoved(BaseEventArgs args)
        {
            var typedArgs = ((CollectionEventArgs<BackgroundModel>)args);
            foreach(var background in typedArgs.ChangedItems)
                View.RemoveBackground(background);

        }
    }
}
