using da2mvc.core.events;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class PlaceholderViewEventArgs : BaseEventArgs
    {
        public PlaceholderViewEventArgs(int eventId, PlaceholderView view, Point point) : base(eventId)
        {
            View = view;
            Point = point;
        }

        public PlaceholderView View { get; }
        public Point Point { get; }
    }
}
