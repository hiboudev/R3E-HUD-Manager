using da2mvc.core.command;
using R3EHUDManager.application.events;
using R3EHUDManager.database;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.userpreferences.command
{
    class SaveOutsidePlaceholdersPrefCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly UserPreferencesModel preferences;
        private readonly Database database;

        public SaveOutsidePlaceholdersPrefCommand(IntEventArgs args, UserPreferencesModel preferences, Database database)
        {
            this.args = args;
            this.preferences = preferences;
            this.database = database;
        }

        public void Execute()
        {
            preferences.PromptOutsidePlaceholders = (OutsidePlaceholdersPrefType)args.Value;
            database.SaveOutsidePlaceholdersPref(preferences.PromptOutsidePlaceholders);
        }
    }
}
