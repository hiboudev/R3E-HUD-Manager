using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3EHUDManager.location.model;

namespace R3EHUDManager.fileexplorer.command
{
    class OpenAppInstallDirectoryCommand : AbstractOpenFileExplorerCommand
    {
        public OpenAppInstallDirectoryCommand(LocationModel locationModel) : base(locationModel)
        {
        }

        public override void Execute()
        {
            OpenFileExplorer(locationModel.AppInstallDirectory);
        }
    }
}
