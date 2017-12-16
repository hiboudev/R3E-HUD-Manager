using da2mvc.core.events;
using R3EHUDManager.layout.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.layout.events
{
    class LayoutSourceEventArgs : BaseEventArgs
    {
        public LayoutSourceEventArgs(int eventId, LayoutSourceModel model) : base(eventId)
        {
            Model = model;
        }

        public LayoutSourceModel Model { get; }
    }
}
