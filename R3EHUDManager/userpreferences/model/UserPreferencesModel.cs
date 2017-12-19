using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.userpreferences.model
{
    class UserPreferencesModel
    {

        public OutsidePlaceholdersPrefType PromptOutsidePlaceholders { get; internal set; }

        private Dictionary<PreferenceType, bool> promptPreferences = new Dictionary<PreferenceType, bool>
        {
            { PreferenceType.PROMPT_SAVE_PROFILE_LAYOUT_CHANGE, true },
            { PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT, true },
            { PreferenceType.PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE, true },
            { PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT, true },
        };

        public bool GetPromptPreference(PreferenceType prefType)
        {
            return promptPreferences[prefType];
        }

        public void SetPromptPreference(PreferenceType prefType, bool value)
        {
            promptPreferences[prefType] = value;
        }

        public bool UserWatchedPresentation { get; internal set; }
        public int LastProfileId { get; internal set; }
    }
}
