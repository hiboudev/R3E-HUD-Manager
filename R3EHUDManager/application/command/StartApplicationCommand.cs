using R3EHUDManager.huddata.command;
using R3EHUDManager.location.command;
using R3EHUDManager.background.command;
using R3EHUDManager.database.command;
using R3EHUDManager.log.command;
using R3EHUDManager.profile.command;
using da2mvc.core.injection;
using da2mvc.core.command;

namespace R3EHUDManager.application.command
{
    class StartApplicationCommand : ICommand
    {
        public void Execute()
        {
            Injector.ExecuteCommand(typeof(InitializeLocalDirectoriesCommand)); 
            Injector.ExecuteCommand(typeof(InitializeLoggerCommand)); 
            Injector.ExecuteCommand(typeof(InitializeDatabaseCommand));
            Injector.ExecuteCommand(typeof(FindR3eHomeDirectoryCommand));
            Injector.ExecuteCommand(typeof(SaveOriginalFileCommand));
            Injector.ExecuteCommand(typeof(LoadHudDataCommand));
            Injector.ExecuteCommand(typeof(InitializeBackgroundsCommand)); 
            Injector.ExecuteCommand(typeof(InitializeProfilesCommand));
        }
    }
}
