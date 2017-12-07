using R3EHUDManager.background.model;
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

        public string LocalDirectoryDatabase = Path.Combine(Application.UserAppDataPath, "database");
        public string LocalDirectoryBackgrounds = Path.Combine(Application.UserAppDataPath, "backgrounds");
        public string LocalDirectoryOldBackups = Path.Combine(Application.UserAppDataPath, "old backups");
        public string LocalDirectoryLogs = Path.Combine(Application.UserAppDataPath, "logs");

        public string[] LocalDirectories { get; }

        public LocationModel()
        {
            LocalDirectories = new string[] {
                LocalDirectoryDatabase,
                LocalDirectoryBackgrounds,
                LocalDirectoryOldBackups,
                LocalDirectoryLogs
            };
        }

        public string R3eHomeBaseDirectory { get; internal set; }

        public string HudOptionsDirectory { get => Path.Combine(R3eHomeBaseDirectory, "UserData"); }

        public string HudOptionsFileName { get => "hud_options.xml"; }

        public string HudOptionsFile { get => Path.Combine(HudOptionsDirectory, HudOptionsFileName); }

        public string HudOptionsBackupFileName { get => "hud_options.xml.backup"; }

        public string HudOptionsBackupFile { get => Path.Combine(Application.UserAppDataPath, HudOptionsBackupFileName); }
        
        public string AppDataDirectory { get => Application.UserAppDataPath; }

        public string AppInstallDirectory { get => Application.StartupPath; }

        public string DatabaseFile { get => Path.Combine(LocalDirectoryDatabase, "database.sqlite"); }

        public string GraphicalAssetDirectory { get => "_graphical_assets"; }

        public string GetGraphicBasePath(BaseDirectoryType directoryType)
        {
            switch (directoryType)
            {
                case BaseDirectoryType.BACKGROUNDS_DIRECTORY:
                    return LocalDirectoryBackgrounds;

                case BaseDirectoryType.GRAPHICAL_ASSETS:
                    return GraphicalAssetDirectory;

                default:
                    throw new Exception("Unkown DirectoryType.");
            }
        }

        public string LogFile { get => Path.Combine(LocalDirectoryLogs, "log.txt"); }
    }
}
