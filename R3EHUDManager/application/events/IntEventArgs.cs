
using da2mvc.core.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.application.events
{
    class IntEventArgs : BaseEventArgs
    {
        public IntEventArgs(int eventId, int value) : base(eventId)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
