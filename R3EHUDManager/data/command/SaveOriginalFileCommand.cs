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

        public SaveOriginalFileCommand(LocationModel locationModel)
        {
            this.locationModel = locationModel;
        }

        public void Execute()
        {
            if (!File.Exists(locationModel.HudOptionsPath))
            {
                MessageBox.Show("HUD file 'hud_options.xml' not found, application will exit.", "Error");
                Environment.Exit(0);
            }

            if (!File.Exists(locationModel.HudOptionsBackupPath))
            {
                File.Copy(locationModel.HudOptionsPath, locationModel.HudOptionsBackupPath);
            }
        }
    }
}
