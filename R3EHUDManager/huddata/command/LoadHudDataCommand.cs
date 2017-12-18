using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using da2mvc.core.command;
using System.Windows.Forms;
using R3EHUDManager.selection.model;
using da2mvc.core.events;
using R3EHUDManager.huddata.model;

namespace R3EHUDManager.huddata.command
{
    class LoadHudDataCommand : ICommand, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_HUD_LAYOUT_LOADED = EventId.New();

        private readonly LocationModel locationModel;
        private readonly PlaceHolderCollectionModel placeHolderCollection;
        private readonly SelectionModel selectionModel;
        private readonly LayoutIOModel layoutIO;

        public LoadHudDataCommand(LocationModel locationModel,  PlaceHolderCollectionModel placeHolderCollection, SelectionModel selectionModel,
            LayoutIOModel layoutIO)
        {
            this.locationModel = locationModel;
            this.placeHolderCollection = placeHolderCollection;
            this.selectionModel = selectionModel;
            this.layoutIO = layoutIO;
        }
        
        public void Execute()
        {
            List<PlaceholderModel> placeholders = null;
            try
            {
                placeholders = layoutIO.LoadR3eLayout();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while parsing HUD xml file, application will exit.\n" + e.Message, "Error");
                Environment.Exit(0);
            }

            if (placeholders != null)
            {
                selectionModel.Unselect();
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
