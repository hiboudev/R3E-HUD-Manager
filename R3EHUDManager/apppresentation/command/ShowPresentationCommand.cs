using da2mvc.core.command;
using R3EHUDManager.apppresentation.view;
using R3EHUDManager.database;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (presentation.ShowDialog() == DialogResult.OK)
            {
                preferences.UserWatchedPresentation = true;
                database.SaveUserWatchedPresentationPref(true);
            }
            presentation.Dispose();
        }
    }
}
