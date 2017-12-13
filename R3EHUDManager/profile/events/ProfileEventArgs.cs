using da2mvc.core.events;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.events
{
    class ProfileEventArgs : BaseEventArgs
    {
        public ProfileEventArgs(int  eventId, ProfileModel profile) : base(eventId)
        {
            Profile = profile;
        }

        public ProfileModel Profile { get; }
    }
}
