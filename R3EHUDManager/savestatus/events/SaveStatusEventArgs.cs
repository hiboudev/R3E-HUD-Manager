using da2mvc.core.events;
using R3EHUDManager.savestatus.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.savestatus.events
{
    class SaveStatusEventArgs : BaseEventArgs
    {
        public SaveStatusEventArgs(int eventId, SaveType type, bool isSaved) : base(eventId)
        {
            Type = type;
            IsSaved = isSaved;
        }

        public SaveType Type { get; }
        public bool IsSaved { get; }
    }
}
