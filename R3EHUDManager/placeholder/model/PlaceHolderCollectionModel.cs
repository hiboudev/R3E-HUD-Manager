using R3EHUDManager.placeholder.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using System.Drawing;
using R3EHUDManager.coordinates;
using da2mvc.framework.model;

namespace R3EHUDManager.placeholder.model
{
    class PlaceHolderCollectionModel : CollectionModel<PlaceholderModel>
    {
        private Dictionary<string, PlaceholderModel> placeHolders = new Dictionary<string, PlaceholderModel>();
        public const string EVENT_PLACE_HOLDER_UPDATED = "placeHolderUpdated";

        public override void Add(PlaceholderModel model)
        {
            PrivateAdd(model);
            base.Add(model);
        }

        public override void AddRange(List<PlaceholderModel> models)
        {
            foreach (var model in models)
                PrivateAdd(model);

            base.AddRange(models);
        }

        public override void Remove(PlaceholderModel model)
        {
            placeHolders.Remove(model.Name);
            base.Remove(model);
        }

        public override void Clear()
        {
            placeHolders.Clear();
            base.Clear();
        }

        private void PrivateAdd(PlaceholderModel model)
        {
            placeHolders.Add(model.Name, model);
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
    }
}
