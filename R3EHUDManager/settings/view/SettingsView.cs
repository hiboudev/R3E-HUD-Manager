using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
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
        private CheckBox checkBox;

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
                RowCount = 2,
                ColumnCount = 1,
            };

            checkBox = new CheckBox()
            {
                Text = "Prompt for outside placeholders when switching background layout.",
                AutoSize = true,
                Checked = preferences.PromptOutsidePlaceholders == OutsidePlaceholdersPrefType.PROMPT,
            };

            Button applyButton = new Button()
            {
                Text = "Apply",
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
            };
            applyButton.Click += ApplyClicked;

            panel.Controls.Add(checkBox);
            panel.Controls.Add(applyButton);
            Controls.Add(panel);
        }

        private void ApplyClicked(object sender, EventArgs e)
        {
            if (checkBox.Checked)
                DispatchEvent(new IntEventArgs(EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED, (int)OutsidePlaceholdersPrefType.PROMPT));

            DialogResult = DialogResult.OK;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
