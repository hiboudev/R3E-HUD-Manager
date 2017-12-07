using da2mvc.command;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.log.command
{
    class InitializeLoggerCommand : ICommand
    {
        private readonly LocationModel location;

        public InitializeLoggerCommand(LocationModel location)
        {
            this.location = location;
        }

        public void Execute()
        {
            Logger.Initialize(location.LogFile);
            Logger.Clear();
        }
    }
}
