using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.huddata.model;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.layout.model;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.huddata.command
{
    class ReloadDefaultHudDataCommand : ICommand, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_DEFAULT_HUD_LAYOUT_LOADED = EventId.New();

        private readonly LocationModel locationModel;
        private readonly PlaceHolderCollectionModel placeHolderCollection;
        private readonly SelectionModel selectionModel;
        private readonly LayoutSourceModel layoutSource;
        private readonly LayoutIOModel layoutIO;

        public ReloadDefaultHudDataCommand(LocationModel locationModel, PlaceHolderCollectionModel placeHolderCollection,
            SelectionModel selectionModel, LayoutSourceModel layoutSource, LayoutIOModel layoutIO)
        {
            this.locationModel = locationModel;
            this.placeHolderCollection = placeHolderCollection;
            this.selectionModel = selectionModel;
            this.layoutSource = layoutSource;
            this.layoutIO = layoutIO;
        }

        public void Execute()
        {
            selectionModel.Unselect();

            List<PlaceholderModel> placeholders = null;
            try
            {
                placeholders = layoutIO.LoadDefaultR3eLayout();
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

                DispatchEvent(new BaseEventArgs(EVENT_DEFAULT_HUD_LAYOUT_LOADED));
                layoutSource.SetSource(LayoutSourceType.BACKUP, locationModel.HudOptionsBackupFile);
            }
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
