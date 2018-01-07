using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.userpreferences.events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.userpreferences.model
{
    public class UserPreferencesModel : IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_CULTURE_CHANGED = EventId.New();

        public OutsidePlaceholdersPrefType PromptOutsidePlaceholders { get; internal set; }

        private Dictionary<PreferenceType, bool> savePromptPreferences = new Dictionary<PreferenceType, bool>
        {
            { PreferenceType.PROMPT_SAVE_PROFILE_LAYOUT_CHANGE, true },
            { PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT, true },
            { PreferenceType.PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE, true },
            { PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT, true },
        };

        public bool GetSavePromptPreference(PreferenceType prefType)
        {
            return savePromptPreferences[prefType];
        }

        public void SetPromptPreference(PreferenceType prefType, bool value)
        {
            savePromptPreferences[prefType] = value;
        }

        public bool UserWatchedPresentation { get; internal set; }
        public int LastProfileId { get; internal set; }

        private bool useInvariantCulture = false;

        public bool UseInvariantCulture
        {
            get => useInvariantCulture;
            set
            {
                useInvariantCulture = value;
                CultureInfo.DefaultThreadCurrentCulture = useInvariantCulture ? CultureInfo.InvariantCulture : CultureInfo.InstalledUICulture;
                DispatchEvent(new BaseEventArgs(EVENT_CULTURE_CHANGED));
            }
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

    }
}
