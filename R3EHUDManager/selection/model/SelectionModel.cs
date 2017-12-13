using da2mvc.core.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.selection.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.selection.model
{
    class SelectionModel:EventDispatcher
    {
        private PlaceholderModel selected;
        public static readonly int EVENT_SELECTED = EventId.New();
        public static readonly int EVENT_UNSELECTED = EventId.New();

        public void Select(PlaceholderModel placeholder)
        {
            if (placeholder == selected) return;

            if (selected != null)
                DispatchEvent(new SelectionModelEventArgs(EVENT_UNSELECTED, selected));

            selected = placeholder;
            DispatchEvent(new SelectionModelEventArgs(EVENT_SELECTED, selected));
        }

        public void Unselect()
        {
            if (selected == null) return;

            PlaceholderModel oldSelected = selected;
            selected = null;

            DispatchEvent(new SelectionModelEventArgs(EVENT_UNSELECTED, oldSelected));
        }
    }
}
