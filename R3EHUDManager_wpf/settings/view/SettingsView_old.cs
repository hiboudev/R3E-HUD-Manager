//using da2mvc.core.events;
//using R3EHUDManager.application.events;
//using R3EHUDManager.application.view;
//using R3EHUDManager.userpreferences.events;
//using R3EHUDManager.userpreferences.model;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace R3EHUDManager.settings.view
//{
//    class SettingsView : BaseModalForm, IEventDispatcher
//    {
//        public event EventHandler MvcEventHandler;
//        public static readonly int EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED = EventId.New();
//        public static readonly int EVENT_PROMPT_PREF_CHANGED = EventId.New();
//        public static readonly int EVENT_CULTURE_PREF_CHANGED = EventId.New();
//        private CheckBox outsideBox;
//        private Dictionary<PreferenceType, CheckBox> promptCheckBoxes = new Dictionary<PreferenceType, CheckBox>();
//        private CheckBox cultureBox;

//        public SettingsView(UserPreferencesModel preferences) : base("Settings")
//        {
//            InitializeUI(preferences);
//        }

//        private Label NewLabel(string text)
//        {
//            return new Label()
//            {
//                Text = text,
//                AutoSize = true,
//                Padding = new Padding(3, 8, 3, 8),
//                Font = new Font(Font, FontStyle.Underline),
//            };
//        }

//        private CheckBox NewCheckBox(string text, bool check)
//        {
//            return new CheckBox()
//            {
//                Text = text,
//                AutoSize = true,
//                Checked = check,
//                Padding = new Padding(14,0,0,0),
//            };
//        }

//        private CheckBox NewPromptCheckBox(PreferenceType prefType, string text, UserPreferencesModel preferences)
//        {
//            CheckBox checkBox = NewCheckBox(text, preferences.GetSavePromptPreference(prefType));
//            promptCheckBoxes.Add(prefType, checkBox);
//            return checkBox;
//        }

//        public void DispatchEvent(BaseEventArgs args)
//        {
//            MvcEventHandler?.Invoke(this, args);
//        }
//    }
//}
