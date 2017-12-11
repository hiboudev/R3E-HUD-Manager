using da2MVC.events;
using R3EHUDManager.profile.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.model
{
    class ProfileCollectionModel:EventDispatcher // TODO generic model for collections
    {
        public List<ProfileModel> Profiles { get; } = new List<ProfileModel>();

        public const string EVENT_PROFILE_ADDED = "profileAdded";
        public const string EVENT_PROFILE_REMOVED = "profileRemoved";

        internal void Add (ProfileModel profile)
        {
            Profiles.Add(profile);

            DispatchEvent(new ProfileCollectionEventArgs(EVENT_PROFILE_ADDED, this, new ProfileModel[] { profile }));
        }

        internal void AddRange(List<ProfileModel> profiles)
        {
            this.Profiles.AddRange(profiles);

            DispatchEvent(new ProfileCollectionEventArgs(EVENT_PROFILE_ADDED, this, profiles.ToArray()));
        }

        internal void Remove(ProfileModel profile)
        {
            Profiles.Remove(profile);

            DispatchEvent(new ProfileEventArgs(EVENT_PROFILE_REMOVED, profile));
        }

        internal ProfileModel Get(int id)
        {
            foreach (ProfileModel profile in Profiles)
                if (profile.Id == id)
                    return profile;

            throw new Exception("Profile not found.");
        }
    }
}
