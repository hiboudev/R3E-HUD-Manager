using da2mvc.core.events;
using R3EHUDManager.location.events;

namespace R3EHUDManager.location.model
{
    class SelectedR3eDirectoryModel:EventDispatcher
    {
        public static readonly int EVENT_DIRECTORY_CHANGED = EventId.New();

        public R3eDirectoryModel Directory { get; private set; }

        public void SelectDirectory(R3eDirectoryModel directory)
        {
            Directory = directory;
            DispatchEvent(new SelectedR3eDirectoryEventArgs(EVENT_DIRECTORY_CHANGED, this));
        }
    }
}
