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
        private PlaceholderModel selection;
        public static readonly int EVENT_SELECTED = EventId.New();
        public static readonly int EVENT_UNSELECTED = EventId.New();

        internal PlaceholderModel Selection { get => selection; }

        public void Select(PlaceholderModel placeholder)
        {
            if (placeholder == Selection) return;

            if (Selection != null)
                DispatchEvent(new SelectionModelEventArgs(EVENT_UNSELECTED, Selection));

            selection = placeholder;
            DispatchEvent(new SelectionModelEventArgs(EVENT_SELECTED, Selection));
        }

        public void Unselect()
        {
            if (Selection == null) return;

            PlaceholderModel oldSelected = Selection;
            selection = null;

            DispatchEvent(new SelectionModelEventArgs(EVENT_UNSELECTED, oldSelected));
        }
    }
}
