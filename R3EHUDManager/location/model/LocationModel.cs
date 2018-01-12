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
    public class LocationModel
    {
        private static string appDataDirectory = System.Windows.Forms.Application.UserAppDataPath; // TODO read https://www.codeproject.com/Tips/370232/Where-should-I-store-my-data

        public string LocalDirectoryDatabase = Path.Combine(appDataDirectory, "database");
        public string LocalDirectoryBackgrounds = Path.Combine(appDataDirectory, "backgrounds");
        public string LocalDirectoryOldBackups = Path.Combine(appDataDirectory, "old backups");
        public string LocalDirectoryLogs = Path.Combine(appDataDirectory, "logs");
        public string LocalDirectoryProfiles = Path.Combine(appDataDirectory, "profiles");

        public string[] LocalDirectories { get; }

        public LocationModel()
        {
            LocalDirectories = new string[] {
                LocalDirectoryDatabase,
                LocalDirectoryBackgrounds,
                LocalDirectoryOldBackups,
                LocalDirectoryLogs,
                LocalDirectoryProfiles
            };
        }

        public string R3eHomeBaseDirectory { get; internal set; }

        public string HudOptionsDirectory { get => Path.Combine(R3eHomeBaseDirectory, "UserData"); }

        public string HudOptionsFileName { get => "hud_options.xml"; }

        public string HudOptionsFile { get => Path.Combine(HudOptionsDirectory, HudOptionsFileName); }

        public string HudOptionsBackupFileName { get => "hud_options.xml.backup"; }

        public string HudOptionsBackupFile { get => Path.Combine(appDataDirectory, HudOptionsBackupFileName); }

        public string AppDataDirectory { get => appDataDirectory; }

        public string AppInstallDirectory { get => AppDomain.CurrentDomain.BaseDirectory; }

        public string DatabaseFile { get => Path.Combine(LocalDirectoryDatabase, "database.sqlite"); }

        public string GraphicalAssetDirectory { get => Path.Combine(AppInstallDirectory, "_graphical_assets"); } // TODO Why in WPF relative path doesn't work?

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

        // TODO get file depending of current version
        public string HudTemplateFile { get => Path.Combine(AppInstallDirectory, @"_template\hud_options_template_v6.xml"); }


        public string MotecXmlFile { get => Path.Combine(AppInstallDirectory, @"_motec_data\motecs.xml"); }
    }
}
