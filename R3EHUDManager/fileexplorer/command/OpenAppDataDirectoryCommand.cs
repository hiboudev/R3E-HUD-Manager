using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.fileexplorer.command
{
    class OpenAppDataDirectoryCommand : AbstractOpenFileExplorerCommand
    {
        public OpenAppDataDirectoryCommand(LocationModel locationModel) : base(locationModel)
        {
        }

        public override void Execute()
        {
            OpenFileExplorer(locationModel.AppDataDirectory);
        }
    }
}
