using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.huddata.model;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace R3EHUDManager.huddata.command
{
    class ReloadDefaultHudDataCommand : ICommand
    {
        private readonly LocationModel locationModel;
        private readonly PlaceHolderCollectionModel placeHolderCollection;
        private readonly SelectionModel selectionModel;
        private readonly LayoutIOModel layoutIO;
        private readonly SelectedProfileModel selectedProfile;

        public ReloadDefaultHudDataCommand(LocationModel locationModel, PlaceHolderCollectionModel placeHolderCollection,
            SelectionModel selectionModel, LayoutIOModel layoutIO, SelectedProfileModel selectedProfile)
        {
            this.locationModel = locationModel;
            this.placeHolderCollection = placeHolderCollection;
            this.selectionModel = selectionModel;
            this.layoutIO = layoutIO;
            this.selectedProfile = selectedProfile;
        }

        public void Execute()
        {
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
                selectionModel.Unselect();
                selectedProfile.SelectNone();
                placeHolderCollection.Clear();
                placeHolderCollection.AddRange(placeholders);
                layoutIO.DispatchSaveStatus();
            }
        }
    }
}
