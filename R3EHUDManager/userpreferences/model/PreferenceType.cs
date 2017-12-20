using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.userpreferences.model
{
    enum PreferenceType
    {
        // Fixed values cause they're used as keys in DB.
        PROMPT_OUTSIDE_PLACEHOLDER = 0,
        USER_WATCHED_PRESENTATION = 1, // not really a preference
        PROMPT_SAVE_PROFILE_LAYOUT_CHANGE = 2,
        PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE = 3,
        PROMPT_SAVE_PROFILE_APP_EXIT = 4,
        PROMPT_APPLY_LAYOUT_APP_EXIT = 5,
        LAST_PROFILE = 6,
        USE_INVARIANT_CULTURE = 7
    }
}
