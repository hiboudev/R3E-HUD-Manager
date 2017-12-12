using da2MVC.events;
using R3EHUDManager.location.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.model
{
    class R3eDirectoryCollectionModel: EventDispatcher
    {
        private List<R3eDirectoryModel> directories = new List<R3eDirectoryModel>();
        private const string EVENT_COLLECTION_FILLED = "collectionFilled";

        public void SetDirectories(List<R3eDirectoryModel> directories)
        {
            this.directories = directories;
            DispatchEvent(new R3eDirectoryCollectionEventArgs(EVENT_COLLECTION_FILLED, this));
        }
    }
}
