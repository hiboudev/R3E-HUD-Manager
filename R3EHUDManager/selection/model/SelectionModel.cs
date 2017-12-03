using da2MVC.events;
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
        public const string EVENT_SELECTED = "selected";
        public const string EVENT_UNSELECTED = "unselected";

        public void Select(PlaceholderModel placeholder)
        {
            if (placeholder == selected) return;

            if (selected != null)
                DispatchEvent(new SelectionModelEventArgs(EVENT_UNSELECTED, selected));

            selected = placeholder;
            DispatchEvent(new SelectionModelEventArgs(EVENT_SELECTED, selected));
        }
    }
}
