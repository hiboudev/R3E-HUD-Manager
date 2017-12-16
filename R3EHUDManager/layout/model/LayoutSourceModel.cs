using da2mvc.core.events;
using R3EHUDManager.layout.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.layout.model
{
    class LayoutSourceModel:EventDispatcher
    {
        public static readonly int EVENT_SOURCE_CHANGED = EventId.New();

        public LayoutSourceType SourceType { get; private set; }
        public string Name { get; private set; }

        public void SetSource(LayoutSourceType sourceType, string name)
        {
            SourceType = sourceType;
            Name = name;

            DispatchEvent(new LayoutSourceEventArgs(EVENT_SOURCE_CHANGED, this));
        }
    }
}
