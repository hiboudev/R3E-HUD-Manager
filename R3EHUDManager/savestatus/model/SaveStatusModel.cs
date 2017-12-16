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

        private SaveType isSaved = SaveType.PROFILE | SaveType.R3E_HUD;

        public void SetChanged(SaveType type)
        {
            isSaved &= ~type;
            DispatchEvent(new SaveStatusEventArgs(EVENT_STATUS_CHANGED, type, false));
        }

        public void SetSaved(SaveType type)
        {
            isSaved |= type;
            DispatchEvent(new SaveStatusEventArgs(EVENT_STATUS_CHANGED, type, true));
        }
    }
}
