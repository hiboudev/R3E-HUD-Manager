
using da2mvc.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.application.events
{
    class IntEventArgs : BaseEventArgs
    {
        public IntEventArgs(string eventName, int value) : base(eventName)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
