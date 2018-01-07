using da2mvc.core.events;
using R3EHUDManager.huddata.model;

namespace R3EHUDManager.huddata.events
{
    class LayoutSourceEventArgs : BaseEventArgs
    {
        public LayoutSourceEventArgs(int eventId, LayoutSourceType sourceType, string sourceName) : base(eventId)
        {
            SourceType = sourceType;
            SourceName = sourceName;
        }

        public LayoutSourceType SourceType { get; }
        public string SourceName { get; }
    }
}
