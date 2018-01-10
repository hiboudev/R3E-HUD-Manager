using da2mvc.core.command;
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
            foreach (var dir in locationModel.LocalDirectories)
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
        }
    }
}
