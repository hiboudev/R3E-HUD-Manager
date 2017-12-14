using da2mvc.core.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.events
{
    class InvalidLayoutEventArgs : BaseEventArgs
    {
        public InvalidLayoutEventArgs(int eventId, PlaceholderModel placeholder, string description) : base(eventId)
        {
            Placeholder = placeholder;
            Description = description;
        }

        public PlaceholderModel Placeholder { get; }
        public string Description { get; }
    }
}
