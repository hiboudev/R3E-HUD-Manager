using da2mvc.core.events;
using R3EHUDManager.location.events;
using System.Collections.Generic;

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
