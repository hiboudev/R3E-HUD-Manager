using da2mvc.core.events;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.userpreferences.events
{
    class PromptPrefsEventArgs : BaseEventArgs
    {
        public PromptPrefsEventArgs(int eventId, Dictionary<PreferenceType, bool> preferences) : base(eventId)
        {
            Preferences = preferences;
        }

        public Dictionary<PreferenceType, bool> Preferences { get; }
    }
}
