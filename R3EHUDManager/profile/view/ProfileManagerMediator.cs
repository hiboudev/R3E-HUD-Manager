using da2mvc.core.events;
using da2mvc.core.view;
using da2mvc.framework.model;
using da2mvc.framework.model.events;
using R3EHUDManager.profile.events;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.view
{
    class ProfileManagerMediator : BaseMediator<ProfileManagerView>
    {
        public ProfileManagerMediator()
        {
            RegisterEventListener(typeof(CollectionModel<ProfileModel>), CollectionModel<ProfileModel>.EVENT_ITEMS_REMOVED, OnProfileRemoved);
        }

        private void OnProfileRemoved(BaseEventArgs args)
        {
            foreach(var profile in ((CollectionEventArgs<ProfileModel>)args).ChangedItems)
                View.RemoveProfile(profile);
        }
    }
}
