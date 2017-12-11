using da2MVC.events;
using R3EHUDManager.profile.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.model
{
    class SelectedProfileModel:EventDispatcher
    {
        public ProfileModel Selection { get; private set; }
        public const string EVENT_SELECTION_CHANGED = "selectionChanged";

        public void SelectProfile(ProfileModel profile)
        {
            Selection = profile;
            DispatchEvent(new ProfileEventArgs(EVENT_SELECTION_CHANGED, profile));
        }
    }
}
