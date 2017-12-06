using da2mvc.command;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.database.command
{
    class InitializeDatabaseCommand : ICommand
    {
        private readonly Database database;
        private readonly LocationModel locationModel;

        public InitializeDatabaseCommand(Database database, LocationModel locationModel)
        {
            this.database = database;
            this.locationModel = locationModel;
        }

        public void Execute()
        {
            database.Initialize(locationModel.DatabaseFile);
        }
    }
}
