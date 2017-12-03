using da2mvc.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class PlaceHolderResizedEventArgs : BaseEventArgs
    {
        public PlaceHolderResizedEventArgs(string eventName, string placeholderName, decimal size) : base(eventName)
        {
            PlaceholderName = placeholderName;
            Size = size;
        }

        public string PlaceholderName { get; }
        public decimal Size { get; }
    }
}
