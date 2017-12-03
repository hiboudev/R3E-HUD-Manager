using da2mvc.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.application.events
{
    class StringEventArgs : BaseEventArgs
    {
        public StringEventArgs(string eventName,string text) : base(eventName)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
