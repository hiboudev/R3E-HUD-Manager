using R3EHUDManager.data.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_MVC.command;

namespace R3EHUDManager.data.command
{
    class LoadHudDataCommand : ICommand
    {
        private readonly LocationModel locationModel;
        private readonly HudOptionsParser parser;
        private readonly PlaceHolderCollectionModel placeHolderCollection;

        public LoadHudDataCommand(LocationModel locationModel, HudOptionsParser parser, PlaceHolderCollectionModel placeHolderCollection)
        {
            this.locationModel = locationModel;
            this.parser = parser;
            this.placeHolderCollection = placeHolderCollection;
        }

        public void Execute()
        {
            // TODO Cause of a mistake in options file, where "Car Status" is also written "CarStatus".
            parser.FixFile(locationModel.HudOptionsPath);
            placeHolderCollection.AddRange(parser.Parse(locationModel.HudOptionsPath));
        }
    }
}
