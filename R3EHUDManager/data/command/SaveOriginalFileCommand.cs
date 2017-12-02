using da2mvc.command;
using R3EHUDManager.data.parser;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.data.command
{
    class SaveOriginalFileCommand : ICommand
    {
        private readonly LocationModel locationModel;
        private readonly HudOptionsParser parser;

        public SaveOriginalFileCommand(LocationModel locationModel, HudOptionsParser parser)
        {
            this.locationModel = locationModel;
            this.parser = parser;
        }

        public void Execute()
        {
            if (!File.Exists(locationModel.HudOptionsFile))
            {
                MessageBox.Show("HUD file 'hud_options.xml' not found, application will exit.", "Error");
                Environment.Exit(0);
            }

            if (!File.Exists(locationModel.HudOptionsBackupFile))
            {
                File.Copy(locationModel.HudOptionsFile, locationModel.HudOptionsBackupFile);
            }
            else
            {
                int r3eVersion = parser.GetVersion(locationModel.HudOptionsFile);
                int backupVersion = parser.GetVersion(locationModel.HudOptionsBackupFile);

                if(r3eVersion > backupVersion)
                {
                    Debug.WriteLine("New version found for the HUD options file.");

                    if (!Directory.Exists(locationModel.HudOldBackupsDirectory))
                        Directory.CreateDirectory(locationModel.HudOldBackupsDirectory);

                    int fileCount = Directory.GetFiles(locationModel.HudOldBackupsDirectory).Length;
                    File.Move(locationModel.HudOptionsBackupFile,
                        Path.Combine(locationModel.HudOldBackupsDirectory, locationModel.HudOptionsBackupFileName + $"{++fileCount}"));

                    File.Copy(locationModel.HudOptionsFile, locationModel.HudOptionsBackupFile);
                }
            }
        }
    }
}
