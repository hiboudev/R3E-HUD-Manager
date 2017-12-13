using da2mvc.core.events;
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
        public PlaceHolderUpdateEventArgs(int eventId, PlaceholderModel placeholder, UpdateType updateType) : base(eventId)
        {
            Placeholder = placeholder;
            UpdateType = updateType;
        }

        public PlaceholderModel Placeholder { get; }
        public UpdateType UpdateType { get; }
    }
}
