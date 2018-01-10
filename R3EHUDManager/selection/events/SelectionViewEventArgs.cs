using da2mvc.core.events;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.selection.events
{
    class SelectionViewEventArgs : BaseEventArgs
    {

        public SelectionViewEventArgs(int eventId,R3ePoint point) : base(eventId)
        {
            Point = point;
        }
        
        public R3ePoint Point { get; }
    }
}
