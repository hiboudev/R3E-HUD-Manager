using R3EHUDManager.huddata.command;
using R3EHUDManager.location.command;
using R3EHUDManager.database.command;
using R3EHUDManager.profile.command;
using da2mvc.core.injection;
using da2mvc.core.command;
using R3EHUDManager.userpreferences.command;
using R3EHUDManager.background.command;
using R3EHUDManager.r3esupport.command;
using R3EHUDManager.apppresentation.command;
using R3EHUDManager.log.command;
using R3EHUDManager.database;
using R3EHUDManager.motec.command;

namespace R3EHUDManager.application.command
{
    class StartApplicationCommand : ICommand
    {
        public void Execute()
        {
            Injector.ExecuteCommand<InitializeLocalDirectoriesCommand>();
            Injector.ExecuteCommand<InitializeLoggerCommand>();
            Injector.ExecuteCommand<InitializeDatabaseCommand>();
            Injector.ExecuteCommand<LoadPreferencesCommand>();
            Injector.ExecuteCommand<LoadFiltersCommand>();
            Injector.ExecuteCommand<FindR3eHomeDirectoryCommand>();
            Injector.ExecuteCommand<SaveOriginalFileCommand>();
            Injector.ExecuteCommand<InitializeSupportRulesCommand>();
            Injector.ExecuteCommand<InitializeBackgroundsCommand>();
            Injector.ExecuteCommand<InitializeMotecsCommand>();
            Injector.ExecuteCommand<InitializeProfilesCommand>();
            Injector.ExecuteCommand<LoadFirstHudCommand>();
            Injector.ExecuteCommand<CheckUserWatchedPresentationCommand>();
        }
    }
}
