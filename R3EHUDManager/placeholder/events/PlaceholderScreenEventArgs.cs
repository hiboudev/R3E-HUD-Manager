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
        public PlaceholderScreenEventArgs(int eventId, int placeholderId, ScreenPositionType screenType) : base(eventId)
        {
            PlaceholderId = placeholderId;
            ScreenType = screenType;
        }

        public int PlaceholderId { get; }
        public ScreenPositionType ScreenType { get; }
    }
}
