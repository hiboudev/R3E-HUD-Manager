using da2mvc.core.command;
using R3EHUDManager.database;
using R3EHUDManager.userpreferences.model;
using R3EHUDManager_wpf.apppresentation.view;
using System.Windows.Forms;

namespace R3EHUDManager.apppresentation.command
{
    class ShowPresentationCommand : ICommand
    {
        private readonly AppPresentationView presentation;
        private readonly UserPreferencesModel preferences;
        private readonly Database database;

        public ShowPresentationCommand(AppPresentationView presentation, UserPreferencesModel preferences, Database database)
        {
            this.presentation = presentation;
            this.preferences = preferences;
            this.database = database;
        }

        public void Execute()
        {
            if (presentation.ShowDialog() == true)
            {
                preferences.UserWatchedPresentation = true;
                database.SaveUserWatchedPresentationPref(true);
            }
            //presentation.Dispose(); // TODO check memory
        }
    }
}
