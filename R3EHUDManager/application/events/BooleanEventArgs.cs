using da2mvc.core.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.application.events
{
    class BooleanEventArgs : BaseEventArgs
    {
        public BooleanEventArgs(int eventId, bool value) : base(eventId)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
