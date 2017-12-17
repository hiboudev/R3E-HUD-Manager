using da2mvc.core.command;
using da2mvc.core.injection;
using R3EHUDManager.userpreferences.model;

namespace R3EHUDManager.apppresentation.command
{
    class CheckUserWatchedPresentationCommand : ICommand
    {
        private readonly UserPreferencesModel preferences;

        public CheckUserWatchedPresentationCommand(UserPreferencesModel preferences)
        {
            this.preferences = preferences;
        }

        public void Execute()
        {
            if (!preferences.UserWatchedPresentation)
                Injector.ExecuteCommand<ShowPresentationCommand>();
        }
    }
}
