using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;

namespace R3EHUDManager.placeholder.events
{
    class PlaceHolderCollectionEventArgs : BaseEventArgs
    {
        public PlaceHolderCollectionEventArgs(string eventName, List<PlaceholderModel> placeHolders) : base(eventName)
        {
            PlaceHolders = placeHolders;
        }

        public List<PlaceholderModel> PlaceHolders { get; }
    }
}
