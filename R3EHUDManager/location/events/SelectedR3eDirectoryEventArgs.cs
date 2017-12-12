using da2mvc.events;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.events
{
    class SelectedR3eDirectoryEventArgs : BaseEventArgs
    {
        public SelectedR3eDirectoryEventArgs(string eventName, SelectedR3eDirectoryModel selection) : base(eventName)
        {
            Selection = selection;
        }

        public SelectedR3eDirectoryModel Selection { get; }
    }
}
