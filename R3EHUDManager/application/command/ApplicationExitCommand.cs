﻿using da2mvc.core.command;
using da2mvc.core.injection;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
using R3EHUDManager.database;
using R3EHUDManager.huddata.model;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.userpreferences.model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace R3EHUDManager.application.command
{
    class ApplicationExitCommand : ICommand
    {
        private readonly ApplicationExitEventArgs args;
        private readonly LayoutIOModel layoutIO;
        private readonly SelectedProfileModel selectedProfile;
        private readonly Database database;
        private readonly UserPreferencesModel preferences;

        public ApplicationExitCommand(ApplicationExitEventArgs args, LayoutIOModel layoutIO, SelectedProfileModel selectedProfile,
            Database database, UserPreferencesModel preferences)
        {
            this.args = args;
            this.layoutIO = layoutIO;
            this.selectedProfile = selectedProfile;
            this.database = database;
            this.preferences = preferences;
        }

        public void Execute()
        {
            if (!CheckUnsavedChanges()) return;

            if (selectedProfile.Selection != null)
                database.SaveLastProfilePref(selectedProfile.Selection.Id);
            else
                database.SaveLastProfilePref(-1);
        }

        private bool CheckUnsavedChanges()
        {
            string text = "";
            bool promptUser = false;
            List<CheckBoxData> checkData = new List<CheckBoxData>();

            // TODO rename UnsavedChangeType to SavedChangeType ?
            UnsavedChangeType saved = layoutIO.GetSaveStatus();

            if (!saved.HasFlag(UnsavedChangeType.PROFILE) && preferences.GetPromptPreference(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT))
            {
                // TODO check background
                checkData.Add(new CheckBoxData(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT, "Don't ask for unsaved profile when exiting application"));

                text += $"Profile \"{selectedProfile.Selection.Name}\" has unsaved changes.\n";
                promptUser = true;
            }

            if (!saved.HasFlag(UnsavedChangeType.R3E) && preferences.GetPromptPreference(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT))
            {
                checkData.Add(new CheckBoxData(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT, "Don't ask for unapplied layout when exiting application"));

                text += "Current layout is not applied to R3E.\n";
                promptUser = true;
            }

            text += "\nExit anyway?";

            if (!promptUser) return true;


            PromptView prompt = Injector.GetInstance<PromptView>();
            prompt.Initialize("Unsaved changes", text, checkData.ToArray());

            DialogResult result = prompt.ShowDialog();

            if (prompt.HasCkeck(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT) && prompt.GetChecked(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT))
            {
                preferences.SetPromptPreference(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT, false);
                database.SavePromptSavePref(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT, false);
            }

            if (prompt.HasCkeck(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT) && prompt.GetChecked(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT))
            {
                preferences.SetPromptPreference(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT, false);
                database.SavePromptSavePref(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT, false);
            }

            if (DialogResult.No == result)
            {
                args.FormArgs.Cancel = true;
                return false;
            }

            return true;
        }
    }
}
