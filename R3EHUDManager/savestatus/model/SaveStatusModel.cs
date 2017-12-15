using da2mvc.core.events;
using R3EHUDManager.savestatus.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.savestatus.model
{
    class SaveStatusModel:EventDispatcher
    {
        public static readonly int EVENT_STATUS_CHANGED = EventId.New();

        private Dictionary<SaveType, bool> isSaved = new Dictionary<SaveType, bool>
        {
            { SaveType.PROFILE, true },
            { SaveType.R3E_HUD, true },
        };

        public void SetChanged(SaveType type)
        {
            isSaved[type] = false;
            DispatchEvent(new SaveStatusEventArgs(EVENT_STATUS_CHANGED, type, false));
        }

        public void SetSaved(SaveType type)
        {
            isSaved[type] = true;
            DispatchEvent(new SaveStatusEventArgs(EVENT_STATUS_CHANGED, type, true));
        }
    }
}
