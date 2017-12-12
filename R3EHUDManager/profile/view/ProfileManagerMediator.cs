using da2mvc.core.events;
using da2mvc.core.view;
using R3EHUDManager.profile.events;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.view
{
    class ProfileManagerMediator : BaseMediator
    {
        public ProfileManagerMediator()
        {
            RegisterEventListener(typeof(ProfileCollectionModel), ProfileCollectionModel.EVENT_PROFILE_REMOVED, OnProfileRemoved);
        }

        private void OnProfileRemoved(BaseEventArgs args)
        {
            ((ProfileManagerView)View).RemoveProfile(((ProfileEventArgs)args).Profile);
        }
    }
}
