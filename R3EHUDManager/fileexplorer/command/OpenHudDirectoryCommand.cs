using da2mvc.command;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.fileexplorer.command
{
    class OpenHudDirectoryCommand : AbstractOpenFileExplorerCommand
    {
        public OpenHudDirectoryCommand(LocationModel locationModel) : base(locationModel)
        {
        }

        public override void Execute()
        {
            OpenFileExplorer(locationModel.HudOptionsDirectory, locationModel.HudOptionsFileName);
        }
    }
}
