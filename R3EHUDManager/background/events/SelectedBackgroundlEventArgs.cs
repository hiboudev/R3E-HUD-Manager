using da2mvc.events;
using R3EHUDManager.background.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.events
{
    class SelectedBackgroundEventArgs : BaseEventArgs
    {
        public SelectedBackgroundEventArgs(string eventName, SelectedBackgroundModel model) : base(eventName)
        {
            Model = model;
        }

        public SelectedBackgroundModel Model { get; }
    }
}
