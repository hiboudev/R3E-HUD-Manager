using da2mvc.events;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.events
{
    class R3eDirectoryCollectionEventArgs : BaseEventArgs
    {
        public R3eDirectoryCollectionEventArgs(string eventName, R3eDirectoryCollectionModel collection) : base(eventName)
        {
            Collection = collection;
        }

        public R3eDirectoryCollectionModel Collection { get; }
    }
}
