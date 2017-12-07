﻿using da2mvc.command;
using R3EHUDManager.huddata.parser;
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
    class ReloadDefaultHudDataCommand : ICommand
    {
        private readonly LocationModel locationModel;
        private readonly HudOptionsParser parser;
        private readonly PlaceHolderCollectionModel placeHolderCollection;
        private readonly SelectionModel selectionModel;

        public ReloadDefaultHudDataCommand(LocationModel locationModel, HudOptionsParser parser, PlaceHolderCollectionModel placeHolderCollection, SelectionModel selectionModel)
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
                placeholders = parser.Parse(locationModel.HudOptionsBackupFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while parsing HUD xml file, application will exit.\n" + e.Message, "Error");
                Environment.Exit(0);
            }

            if (placeholders != null)
                placeHolderCollection.AddRange(placeholders);
        }
    }
}