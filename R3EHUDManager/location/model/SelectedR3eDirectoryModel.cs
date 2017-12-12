using da2MVC.events;
using R3EHUDManager.location.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.model
{
    class SelectedR3eDirectoryModel:EventDispatcher
    {
        public const string EVENT_DIRECTORY_CHANGED = "directoryChanged";

        public R3eDirectoryModel Directory { get; private set; }

        public void SelectDirectory(R3eDirectoryModel directory)
        {
            Directory = directory;
            DispatchEvent(new SelectedR3eDirectoryEventArgs(EVENT_DIRECTORY_CHANGED, this));
        }
    }
}
