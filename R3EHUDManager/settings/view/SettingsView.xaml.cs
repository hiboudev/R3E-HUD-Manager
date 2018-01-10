using da2mvc.core.events;
using da2mvc.framework.application.view;
using R3EHUDManager.application.events;
using R3EHUDManager.userpreferences.events;
using R3EHUDManager.userpreferences.model;
using R3EHUDManager.application.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace R3EHUDManager.settings.view
{
    /// <summary>
    /// Logique d'interaction pour SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ModalWindow, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED = EventId.New();
        public static readonly int EVENT_PROMPT_PREF_CHANGED = EventId.New();
        public static readonly int EVENT_CULTURE_PREF_CHANGED = EventId.New();
        private Dictionary<PreferenceType, CheckBox> promptCheckBoxes;

        public SettingsView(UserPreferencesModel preferences)
        {
            InitializeComponent();
            InitializeUI(preferences);
        }

        private void InitializeUI(UserPreferencesModel preferences)
        {
            promptOutsidePlaceholder.IsChecked = preferences.PromptOutsidePlaceholders == OutsidePlaceholdersPrefType.PROMPT;
            promptOutsidePlaceholder.Visibility = promptOutsidePlaceholder.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;

            promptUnsavedProfiledLayout.IsChecked = preferences.GetSavePromptPreference(PreferenceType.PROMPT_SAVE_PROFILE_LAYOUT_CHANGE);
            promptUnsavedLayoutLayout.IsChecked = preferences.GetSavePromptPreference(PreferenceType.PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE);
            promptUnsavedProfiledExit.IsChecked = preferences.GetSavePromptPreference(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT);
            promptUnsavedLayoutExit.IsChecked = preferences.GetSavePromptPreference(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT);

            useInvariantCulture.IsChecked = preferences.UseInvariantCulture;

            promptCheckBoxes = new Dictionary<PreferenceType, CheckBox>
            {
                { PreferenceType.PROMPT_SAVE_PROFILE_LAYOUT_CHANGE, promptUnsavedProfiledLayout },
                { PreferenceType.PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE, promptUnsavedLayoutLayout },
                { PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT, promptUnsavedProfiledExit },
                { PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT, promptUnsavedLayoutExit },
            };

            applyButton.Click += ApplyClicked;
        }

        private void ApplyClicked(object sender, EventArgs e)
        {
            if (promptOutsidePlaceholder.IsChecked == true)
                DispatchEvent(new IntEventArgs(EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED, (int)OutsidePlaceholdersPrefType.PROMPT));

            DispatchEvent(new PromptPrefsEventArgs(EVENT_PROMPT_PREF_CHANGED, promptCheckBoxes.ToDictionary(x => x.Key, x => x.Value.IsChecked == true)));
            DispatchEvent(new BooleanEventArgs(EVENT_CULTURE_PREF_CHANGED, useInvariantCulture.IsChecked == true));

            DialogResult = true;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
