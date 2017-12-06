using da2mvc.events;
using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.events
{
    class BackgroundModelEventArgs : BaseEventArgs
    {
        public BackgroundModelEventArgs(string eventName, BackgroundModel background) : base(eventName)
        {
            Background = background;
        }

        public BackgroundModel Background { get; }
    }
}
