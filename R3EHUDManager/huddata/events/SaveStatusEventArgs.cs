using da2mvc.core.events;
using R3EHUDManager.huddata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.events
{
    class SaveStatusEventArgs : BaseEventArgs
    {
        public SaveStatusEventArgs(int eventId, UnsavedChangeType savedTypes) : base(eventId)
        {
            SavedTypes = savedTypes;
        }

        public UnsavedChangeType SavedTypes { get; }
    }
}
