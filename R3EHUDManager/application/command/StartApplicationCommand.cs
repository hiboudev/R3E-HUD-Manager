using R3EHUDManager.huddata.command;
using R3EHUDManager.location.command;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.command;
using da2mvc.injection;
using R3EHUDManager.background.command;
using R3EHUDManager.application.events;
using R3EHUDManager.database.command;
using R3EHUDManager.background.events;

namespace R3EHUDManager.application.command
{
    class StartApplicationCommand : ICommand
    {
        public void Execute()
        {
            Injector.ExecuteCommand(typeof(InitializeLocalDirectoriesCommand));
            Injector.ExecuteCommand(typeof(InitializeDatabaseCommand));
            Injector.ExecuteCommand(typeof(FindR3eHomeDirectoryCommand));
            Injector.ExecuteCommand(typeof(SaveOriginalFileCommand));
            Injector.ExecuteCommand(typeof(LoadHudDataCommand));
            Injector.ExecuteCommand(typeof(InitializeBackgroundsCommand));
        }
    }
}
