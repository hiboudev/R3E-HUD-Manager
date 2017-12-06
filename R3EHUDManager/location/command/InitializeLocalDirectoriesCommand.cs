using da2mvc.command;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.command
{
    class InitializeLocalDirectoriesCommand : ICommand
    {
        private readonly LocationModel locationModel;

        public InitializeLocalDirectoriesCommand(LocationModel locationModel)
        {
            this.locationModel = locationModel;
        }

        public void Execute()
        {
            if (!Directory.Exists(locationModel.LocalDirectoryDatabase))
                Directory.CreateDirectory(locationModel.LocalDirectoryDatabase);

            if (!Directory.Exists(locationModel.LocalDirectoryBackgrounds))
                Directory.CreateDirectory(locationModel.LocalDirectoryBackgrounds);

            if (!Directory.Exists(locationModel.LocalDirectoryOldBackups))
                Directory.CreateDirectory(locationModel.LocalDirectoryOldBackups);
        }
    }
}
