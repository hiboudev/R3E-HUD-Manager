using da2MVC.events;
using R3EHUDManager.background.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.model
{
    class BackgroundCollectionModel:EventDispatcher
    {
        private Dictionary<int, BackgroundModel> backgrounds = new Dictionary<int, BackgroundModel>();
        public List<BackgroundModel> Backgrounds { get => backgrounds.Values.ToList(); }

        public const string EVENT_BACKGROUND_LIST_ADDED = "backgroundListAdded";
        public const string EVENT_BACKGROUND_ADDED = "backgroundAdded";

        public void SetBackgrounds(List<BackgroundModel> backgrounds)
        {
            this.backgrounds = backgrounds.ToDictionary(x => x.Id, x => x);
            DispatchEvent(new BackgroundCollectionEventArgs(EVENT_BACKGROUND_LIST_ADDED, this));
        }

        public void AddBackground(BackgroundModel background)
        {
            backgrounds.Add(background.Id, background);
            DispatchEvent(new BackgroundModelEventArgs(EVENT_BACKGROUND_ADDED, background));
        }

        internal BackgroundModel Get(int id)
        {
            return backgrounds[id];
        }
    }
}
