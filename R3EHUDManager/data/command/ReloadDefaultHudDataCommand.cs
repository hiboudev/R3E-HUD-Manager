using da2mvc.command;
using R3EHUDManager.data.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.data.command
{
    class ReloadDefaultHudDataCommand : ICommand
    {
        private readonly LocationModel locationModel;
        private readonly HudOptionsParser parser;
        private readonly PlaceHolderCollectionModel placeHolderCollection;

        public ReloadDefaultHudDataCommand(LocationModel locationModel, HudOptionsParser parser, PlaceHolderCollectionModel placeHolderCollection)
        {
            this.locationModel = locationModel;
            this.parser = parser;
            this.placeHolderCollection = placeHolderCollection;
        }

        public void Execute()
        {
            placeHolderCollection.AddRange(parser.Parse(locationModel.HudOptionsBackupPath));
        }
    }
}
