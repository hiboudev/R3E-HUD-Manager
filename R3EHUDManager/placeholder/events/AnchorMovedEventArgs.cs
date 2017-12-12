using da2mvc.core.events;
using R3EHUDManager.coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class AnchorMovedEventArgs : BaseEventArgs
    {
        public AnchorMovedEventArgs(string eventName, string placeholderName, R3ePoint anchor) : base(eventName)
        {
            PlaceholderName = placeholderName;
            Anchor = anchor;
        }

        public string PlaceholderName { get; }
        public R3ePoint Anchor { get; }
    }
}
