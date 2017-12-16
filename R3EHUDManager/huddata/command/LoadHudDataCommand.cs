using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.command;
using System.Windows.Forms;
using R3EHUDManager.selection.model;
using R3EHUDManager.background.model;
using da2mvc.core.events;

namespace R3EHUDManager.huddata.command
{
    class LoadHudDataCommand : ICommand, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_HUD_LAYOUT_LOADED = EventId.New();

        private readonly LocationModel locationModel;
        private readonly HudOptionsParser parser;
        private readonly PlaceHolderCollectionModel placeHolderCollection;
        private readonly SelectionModel selectionModel;

        public LoadHudDataCommand(LocationModel locationModel, HudOptionsParser parser, PlaceHolderCollectionModel placeHolderCollection, SelectionModel selectionModel)
        {
            this.locationModel = locationModel;
            this.parser = parser;
            this.placeHolderCollection = placeHolderCollection;
            this.selectionModel = selectionModel;
        }
        
        public void Execute()
        {
            selectionModel.Unselect();

            List<PlaceholderModel> placeholders = null;
            try
            {
                placeholders = parser.Parse(locationModel.HudOptionsFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while parsing HUD xml file, application will exit.\n" + e.Message, "Error");
                Environment.Exit(0);
            }

            if (placeholders != null)
            {
                placeHolderCollection.Clear();
                placeHolderCollection.AddRange(placeholders);

                DispatchEvent(new BaseEventArgs(EVENT_HUD_LAYOUT_LOADED));
            }
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
