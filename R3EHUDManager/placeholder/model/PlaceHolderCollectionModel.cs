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
        public const string EVENT_PLACE_HOLDERS_ADDED = "placeHoldersAdded";
        public const string EVENT_PLACE_HOLDER_UPDATED = "placeHolderUpdated";
        public List<PlaceholderModel> Placeholders { get => placeHolders.Values.ToList(); }

        public void AddRange(List<PlaceholderModel> placeHolders)
        {
            this.placeHolders = placeHolders.ToDictionary(x => x.Name, x => x);
            DispatchEvent(new PlaceHolderCollectionEventArgs(EVENT_PLACE_HOLDERS_ADDED, this.placeHolders.Values.ToList()));
        }

        internal void UpdatePlaceholder(string placeholderName, R3ePoint position)
        {
            placeHolders[placeholderName].Position = position.Clone();
            DispatchEvent(new PlaceHolderModelEventArgs(EVENT_PLACE_HOLDER_UPDATED, placeHolders[placeholderName], UpdateType.POSITION));
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
