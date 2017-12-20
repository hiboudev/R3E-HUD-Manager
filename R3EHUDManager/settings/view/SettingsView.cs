﻿using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
using R3EHUDManager.userpreferences.events;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.settings.view
{
    class SettingsView : BaseModalForm, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED = EventId.New();
        public static readonly int EVENT_PROMPT_PREF_CHANGED = EventId.New();
        public static readonly int EVENT_CULTURE_PREF_CHANGED = EventId.New();
        private CheckBox outsideBox;
        private Dictionary<PreferenceType, CheckBox> promptCheckBoxes = new Dictionary<PreferenceType, CheckBox>();
        private CheckBox cultureBox;

        public SettingsView(UserPreferencesModel preferences) : base("Settings")
        {
            InitializeUI(preferences);
        }

        private void InitializeUI(UserPreferencesModel preferences)
        {
            MinimumSize = new Size(50, 50);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding();

            TableLayoutPanel panel = new TableLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
            };

            Label labelPrompt = NewLabel("Prompt for:");
            Label labelCulture = NewLabel("Culture:");

            outsideBox = NewCheckBox("Outside placeholders when switching background layout.", preferences.PromptOutsidePlaceholders == OutsidePlaceholdersPrefType.PROMPT);
            outsideBox.Visible = !outsideBox.Checked;

            cultureBox = NewCheckBox("Use invariant culture (modify the decimal separator used in numeric up/down controls).", preferences.UseInvariantCulture);

            Button applyButton = new Button()
            {
                Text = "Apply",
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
            };
            applyButton.Click += ApplyClicked;

            panel.Controls.Add(labelPrompt);
            panel.Controls.Add(outsideBox);
            panel.Controls.Add(NewPromptCheckBox(PreferenceType.PROMPT_SAVE_PROFILE_LAYOUT_CHANGE, "Unsaved profile when loading a new layout", preferences));
            panel.Controls.Add(NewPromptCheckBox(PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT, "Unsaved layout when loading a new one", preferences));
            panel.Controls.Add(NewPromptCheckBox(PreferenceType.PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE, "Unsaved profile when exiting application", preferences));
            panel.Controls.Add(NewPromptCheckBox(PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT, "Unapplied layout when exiting application", preferences));
            panel.Controls.Add(labelCulture);
            panel.Controls.Add(cultureBox);
            panel.Controls.Add(applyButton);
            Controls.Add(panel);
        }

        private Label NewLabel(string text)
        {
            return new Label()
            {
                Text = text,
                AutoSize = true,
                Padding = new Padding(3, 8, 3, 8),
                Font = new Font(Font, FontStyle.Underline),
            };
        }

        private CheckBox NewCheckBox(string text, bool check)
        {
            return new CheckBox()
            {
                Text = text,
                AutoSize = true,
                Checked = check,
                Padding = new Padding(14,0,0,0),
            };
        }

        private CheckBox NewPromptCheckBox(PreferenceType prefType, string text, UserPreferencesModel preferences)
        {
            CheckBox checkBox = NewCheckBox(text, preferences.GetSavePromptPreference(prefType));
            promptCheckBoxes.Add(prefType, checkBox);
            return checkBox;
        }

        private void ApplyClicked(object sender, EventArgs e)
        {
            if(outsideBox.Checked)
                DispatchEvent(new IntEventArgs(EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED, (int)OutsidePlaceholdersPrefType.PROMPT));

            DispatchEvent(new PromptPrefsEventArgs(EVENT_PROMPT_PREF_CHANGED, promptCheckBoxes.ToDictionary(x => x.Key, x => x.Value.Checked)));
            DispatchEvent(new BooleanEventArgs(EVENT_CULTURE_PREF_CHANGED, cultureBox.Checked));

            DialogResult = DialogResult.OK;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
