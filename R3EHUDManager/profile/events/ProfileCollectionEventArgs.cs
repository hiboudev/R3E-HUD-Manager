using da2mvc.events;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.events
{
    class ProfileCollectionEventArgs : BaseEventArgs
    {
        public ProfileCollectionEventArgs(string eventName, ProfileCollectionModel collection, ProfileModel[] modifiedProfiles) : base(eventName)
        {
            Collection = collection;
            ModifiedProfiles = modifiedProfiles;
        }

        public ProfileCollectionModel Collection { get; }
        public ProfileModel[] ModifiedProfiles { get; }
    }
}
