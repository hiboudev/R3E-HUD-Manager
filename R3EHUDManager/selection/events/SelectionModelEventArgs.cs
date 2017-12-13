using da2mvc.core.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.selection.events
{
    class SelectionModelEventArgs : BaseEventArgs
    {
        public SelectionModelEventArgs(int eventId, PlaceholderModel placeholder) : base(eventId)
        {
            Placeholder = placeholder;
        }

        public PlaceholderModel Placeholder { get; }
    }
}
