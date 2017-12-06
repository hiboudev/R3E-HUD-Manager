using da2mvc.events;
using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.events
{
    class BackgroundCollectionEventArgs : BaseEventArgs
    {
        public BackgroundCollectionEventArgs(string eventName, BackgroundCollectionModel collection) : base(eventName)
        {
            Collection = collection;
        }

        public BackgroundCollectionModel Collection { get; }
    }
}
