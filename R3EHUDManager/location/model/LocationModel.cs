using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.location.model
{
    class LocationModel
    {
        public string R3eHomeBaseDirectory { get; internal set; }

        public string HudOptionsFile { get => Path.Combine(R3eHomeBaseDirectory, "UserData", "hud_options.xml"); }

        public string HudOptionsBackupFileName { get => "hud_options.xml.backup"; }

        public string HudOptionsBackupFile { get => Path.Combine(Application.UserAppDataPath, HudOptionsBackupFileName); }
        
        public string AppDataDirectory { get => Application.UserAppDataPath; }

        public string HudOldBackupsDirectory { get => Path.Combine(Application.UserAppDataPath, "Old Backups"); }
    }
}
