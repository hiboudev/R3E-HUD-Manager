using da2mvc.core.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.events
{
    class PlaceholderScreenEventArgs : BaseEventArgs
    {
        public PlaceholderScreenEventArgs(int eventId, string placeholderName, ScreenPositionType screenType) : base(eventId)
        {
            PlaceholderName = placeholderName;
            ScreenType = screenType;
        }

        public string PlaceholderName { get; }
        public ScreenPositionType ScreenType { get; }
    }
}
