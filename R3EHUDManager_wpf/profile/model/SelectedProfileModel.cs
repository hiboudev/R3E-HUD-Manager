using da2mvc.core.events;
using R3EHUDManager.profile.events;

namespace R3EHUDManager.profile.model
{
    class SelectedProfileModel : EventDispatcher
    {
        public ProfileModel Selection { get; private set; }
        public static readonly int EVENT_SELECTION_CHANGED = EventId.New();
        public static readonly int EVENT_SELECTION_CLEARED = EventId.New();

        public void SelectProfile(ProfileModel profile)
        {
            Selection = profile;
            DispatchEvent(new ProfileEventArgs(EVENT_SELECTION_CHANGED, profile));
        }

        internal void SelectNone()
        {
            Selection = null;
            DispatchEvent(new BaseEventArgs(EVENT_SELECTION_CLEARED));
        }
    }
}
