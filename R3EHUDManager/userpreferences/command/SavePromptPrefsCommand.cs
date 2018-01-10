using da2mvc.core.command;
using R3EHUDManager.database;
using R3EHUDManager.userpreferences.events;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.userpreferences.command
{
    class SavePromptPrefsCommand : ICommand
    {
        private readonly PromptPrefsEventArgs args;
        private readonly UserPreferencesModel preferences;
        private readonly Database database;

        public SavePromptPrefsCommand(PromptPrefsEventArgs args, UserPreferencesModel preferences, Database database)
        {
            this.args = args;
            this.preferences = preferences;
            this.database = database;
        }

        public void Execute()
        {
            foreach (KeyValuePair<PreferenceType, bool> keyValue in args.Preferences)
            {
                preferences.SetPromptPreference(keyValue.Key, keyValue.Value);
                database.SavePromptSavePref(keyValue.Key, keyValue.Value);
            }
        }
    }
}
