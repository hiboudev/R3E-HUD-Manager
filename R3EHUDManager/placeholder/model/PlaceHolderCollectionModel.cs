using R3EHUDManager.placeholder.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using System.Drawing;
using R3EHUDManager.coordinates;

namespace R3EHUDManager.placeholder.model
{
    class PlaceHolderCollectionModel :IEventDispatcher
    {
        private Dictionary<string, PlaceholderModel> placeHolders = new Dictionary<string, PlaceholderModel>();
        public event EventHandler MvcEventHandler;
        public const string EVENT_NEW_LAYOUT = "newLayout";
        public const string EVENT_PLACE_HOLDER_UPDATED = "placeHolderUpdated";
        public List<PlaceholderModel> Placeholders { get => placeHolders.Values.ToList(); }

        public void SetPlaceholders(List<PlaceholderModel> placeHolders)
        {
            this.placeHolders = placeHolders.ToDictionary(x => x.Name, x => x);
            DispatchEvent(new PlaceHolderCollectionEventArgs(EVENT_NEW_LAYOUT, this.placeHolders.Values.ToList()));
        }

        internal void UpdatePlaceholderPosition(string placeholderName, R3ePoint position)
        {
            placeHolders[placeholderName].Position = position.Clone();
            DispatchEvent(new PlaceHolderUpdateEventArgs(EVENT_PLACE_HOLDER_UPDATED, placeHolders[placeholderName], UpdateType.POSITION));
        }

        internal void UpdatePlaceholderAnchor(string placeholderName, R3ePoint anchor)
        {
            placeHolders[placeholderName].Anchor = anchor.Clone();
            DispatchEvent(new PlaceHolderUpdateEventArgs(EVENT_PLACE_HOLDER_UPDATED, placeHolders[placeholderName], UpdateType.ANCHOR));
        }

        internal void UpdatePlaceholderSize(string placeholderName, decimal size)
        {
            placeHolders[placeholderName].Size = new R3ePoint((double)size, (double)size);
            DispatchEvent(new PlaceHolderUpdateEventArgs(EVENT_PLACE_HOLDER_UPDATED, placeHolders[placeholderName], UpdateType.SIZE));
        }

        public PlaceholderModel Get(string placeholderName)
        {
            return placeHolders[placeholderName];
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
