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
        public const string EVENT_COLLECTION_FILLED = "collectionFilled";

        internal List<R3eDirectoryModel> Directories { get; private set; }

        public void SetDirectories(List<R3eDirectoryModel> directories)
        {
            this.Directories = directories;
            DispatchEvent(new R3eDirectoryCollectionEventArgs(EVENT_COLLECTION_FILLED, this));
        }

        public R3eDirectoryModel Get(int id)
        {
            return Directories.Find(x => x.Id == id);
        }
    }
}
