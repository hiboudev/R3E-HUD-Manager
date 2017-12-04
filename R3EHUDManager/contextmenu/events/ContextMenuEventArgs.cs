using da2mvc.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.contextmenu.events
{
    class ContextMenuEventArgs : BaseEventArgs
    {
        public ContextMenuEventArgs(string eventName, int itemId, string itemName) : base(eventName)
        {
            ItemId = itemId;
            ItemName = itemName;
        }

        public int ItemId { get; }
        public string ItemName { get; }
    }
}