using da2mvc.events;
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
        public ProfileEventArgs(string eventName, ProfileModel profile) : base(eventName)
        {
            Profile = profile;
        }

        public ProfileModel Profile { get; }
    }
}
