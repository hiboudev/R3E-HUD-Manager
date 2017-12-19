using da2mvc.core.command;
using da2mvc.core.injection;
using R3EHUDManager.application.view;
using R3EHUDManager.database;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.model;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.huddata.command
{
    class PromptUnsavedChangesCommand : ICommand
    {
        private readonly UnsavedChangesEventArgs args;
        private readonly UserPreferencesModel preferences;
        private readonly Database database;

        public PromptUnsavedChangesCommand(UnsavedChangesEventArgs args, UserPreferencesModel preferences, Database database)
        {
            this.args = args;
            this.preferences = preferences;
            this.database = database;
        }

        public void Execute()
        {
            

            switch (args.ChangeType)
            {
                case UnsavedChangeType.PROFILE:
                    DisplayPrompt(PreferenceType.PROMPT_SAVE_PROFILE_LAYOUT_CHANGE, 
                        "Unsaved profile", $"There's unsaved changes in profile \"{args.SourceName}\", continue anyway?",
                        "Don't ask for unsaved profile when loading a new layout");
                    break;

                case UnsavedChangeType.R3E:
                    DisplayPrompt(PreferenceType.PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE,
                        "Unsaved layout", $"There's unsaved changes in this layout, continue anyway?",
                        "Don't ask for unsaved layout when loading a new one");
                    break;
            }
        }

        private void DisplayPrompt(PreferenceType preferenceType, string title, string text, string checkText)
        {
            if (!preferences.GetPromptPreference(preferenceType)) return;

            PromptView prompt = Injector.GetInstance<PromptView>();
            CheckBoxData checkData = new CheckBoxData(preferenceType, checkText);
            prompt.Initialize(title, text, new CheckBoxData[] { checkData });

            if (DialogResult.No == prompt.ShowDialog())
                args.CancelLoading();

            if (prompt.GetChecked(preferenceType))
            {
                preferences.SetPromptPreference(preferenceType, false);
                database.SavePromptSavePref(preferenceType, false);
            }

            prompt.Dispose();
        }
    }
}
