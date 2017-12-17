using da2mvc.core.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.events
{
    class FiltersChangedEventArgs : BaseEventArgs
    {
        public FiltersChangedEventArgs(int eventId, Dictionary<string, bool> filters) : base(eventId)
        {
            Filters = filters;
        }

        public Dictionary<string, bool> Filters { get; }
    }
}
