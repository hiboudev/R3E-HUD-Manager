using da2mvc.core.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class PlaceHolderResizedEventArgs : BaseEventArgs
    {
        public PlaceHolderResizedEventArgs(int eventId, string placeholderName, decimal size) : base(eventId)
        {
            PlaceholderName = placeholderName;
            Size = size;
        }

        public string PlaceholderName { get; }
        public decimal Size { get; }
    }
}
