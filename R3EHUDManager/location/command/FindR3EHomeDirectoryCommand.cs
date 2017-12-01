using R3EHUDManager.location.finder;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using da2mvc.command;
using System.Windows.Forms;

namespace R3EHUDManager.location.command
{
    class FindR3eHomeDirectoryCommand:ICommand
    {
        private readonly LocationModel locationModel;
        private readonly R3eHomeDirectoryFinder finder;

        public FindR3eHomeDirectoryCommand(LocationModel locationModel, R3eHomeDirectoryFinder finder)
        {
            this.locationModel = locationModel;
            this.finder = finder;
        }

        public void Execute()
        {
            locationModel.R3eHomeBasePath = finder.GetPath();
        }
    }
}
