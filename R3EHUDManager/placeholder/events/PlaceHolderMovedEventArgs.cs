using R3EHUDManager.coordinates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;

namespace R3EHUDManager.placeholder.events
{
    class PlaceHolderMovedEventArgs : BaseEventArgs
    {
        public PlaceHolderMovedEventArgs(int eventId, string placeholderName, R3ePoint position) : base(eventId)
        {
            PlaceholderName = placeholderName;
            Position = position;
        }

        public string PlaceholderName { get; }
        public R3ePoint Position { get; }
    }
}
