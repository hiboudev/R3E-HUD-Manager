using da2mvc.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class PlaceHolderUpdateEventArgs : BaseEventArgs
    {
        public PlaceHolderUpdateEventArgs(string eventName, PlaceholderModel placeholder, UpdateType updateType) : base(eventName)
        {
            Placeholder = placeholder;
            UpdateType = updateType;
        }

        public PlaceholderModel Placeholder { get; }
        public UpdateType UpdateType { get; }
    }
}
