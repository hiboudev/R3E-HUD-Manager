using da2mvc.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class BackgroundModelEventArgs : BaseEventArgs
    {
        public BackgroundModelEventArgs(string eventName, BackgroundModel model) : base(eventName)
        {
            Model = model;
        }

        public BackgroundModel Model { get; }
    }
}
