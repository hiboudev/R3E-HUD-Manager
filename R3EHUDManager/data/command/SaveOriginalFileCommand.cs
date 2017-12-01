using da2mvc.command;
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
        private const string BACKUP_NAME = "hud_options.xml.backup";

        public SaveOriginalFileCommand(LocationModel locationModel)
        {
            this.locationModel = locationModel;
        }

        public void Execute()
        {
            string backupPath = Path.Combine(Application.UserAppDataPath, BACKUP_NAME);

            if (!File.Exists(backupPath))
            {
                File.Copy(locationModel.HudOptionsPath, backupPath);
            }
        }
    }
}
