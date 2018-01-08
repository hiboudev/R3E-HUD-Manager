using da2mvc.core.events;
using R3EHUDManager_wpf.placeholder.view;
using System.Windows;

namespace R3EHUDManager.placeholder.events
{
    class PlaceholderViewEventArgs : BaseEventArgs
    {
        public PlaceholderViewEventArgs(int eventId, PlaceholderView view, Point point) : base(eventId)
        {
            View = view;
            Point = point;
        }

        public PlaceholderView View { get; }
        public Point Point { get; }
    }
}
