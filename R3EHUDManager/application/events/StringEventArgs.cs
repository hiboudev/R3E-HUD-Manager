using da2mvc.core.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.application.events
{
    class StringEventArgs : BaseEventArgs
    {
        public StringEventArgs(int eventId, string value) : base(eventId)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
