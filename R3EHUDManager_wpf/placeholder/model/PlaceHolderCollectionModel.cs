using R3EHUDManager.placeholder.events;
using System.Collections.Generic;
using R3EHUDManager.coordinates;
using da2mvc.framework.collection.model;
using da2mvc.core.events;

namespace R3EHUDManager.placeholder.model
{
    class PlaceHolderCollectionModel : CollectionModel<PlaceholderModel> // TODO delete this class and use generic collection only, edit the view API.
    {
        private Dictionary<string, PlaceholderModel> placeHolders = new Dictionary<string, PlaceholderModel>();

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

        public PlaceholderModel Get(string placeholderName)
        {
            return placeHolders[placeholderName];
        }
    }
}
