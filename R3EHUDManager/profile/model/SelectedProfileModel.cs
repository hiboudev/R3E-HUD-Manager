using da2mvc.core.events;
using R3EHUDManager.profile.events;

namespace R3EHUDManager.profile.model
{
    class SelectedProfileModel:EventDispatcher
    {
        public ProfileModel Selection { get; private set; }
        public const string EVENT_SELECTION_CHANGED = "selectionChanged";
        public const string EVENT_SELECTION_CLEARED = "selectionCleared";

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
