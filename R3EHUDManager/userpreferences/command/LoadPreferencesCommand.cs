using da2mvc.core.command;
using R3EHUDManager.database;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.userpreferences.command
{
    class LoadPreferencesCommand : ICommand
    {
        private readonly UserPreferencesModel preferences;
        private readonly Database database;

        public LoadPreferencesCommand(UserPreferencesModel preferences, Database database)
        {
            this.preferences = preferences;
            this.database = database;
        }

        public void Execute()
        {
            database.GetPreferences(preferences);
        }
    }
}
