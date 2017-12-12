using da2mvc.core.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.screen.events
{
    class ScreenModelEventArgs : BaseEventArgs
    {
        public ScreenModelEventArgs(string eventName, ScreenModel screenModel) : base(eventName)
        {
            ScreenModel = screenModel;
        }

        public ScreenModel ScreenModel { get; }
    }
}
