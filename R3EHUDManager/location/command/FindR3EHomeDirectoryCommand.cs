using R3EHUDManager.location.finder;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using da2mvc.command;
using System.Windows.Forms;
using da2mvc.events;

namespace R3EHUDManager.location.command
{
    class FindR3eHomeDirectoryCommand:ICommand
    {
        private readonly LocationModel locationModel;
        private readonly R3eHomeDirectoryFinder finder;

        public FindR3eHomeDirectoryCommand(LocationModel locationModel, R3eHomeDirectoryFinder finder)
        {
            this.locationModel = locationModel;
            this.finder = finder;
        }
        
        public void Execute()
        {
            string R3eHomePath = finder.GetPath();
            if (R3eHomePath != null)
            {
                locationModel.R3eHomeBaseDirectory = R3eHomePath;
                return;
            }


            string txtFile = Path.Combine(locationModel.AppDataDirectory, "customR3eDirectory.txt");

            if (File.Exists(txtFile))
            {
                if(MessageBox.Show("Use your custom R3E directory?", "Use custom directory?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string r3eDirectory = File.ReadAllText(txtFile);
                    locationModel.R3eHomeBaseDirectory = r3eDirectory;
                    return;
                }
            }

            var dialog = new FolderBrowserDialog();
            dialog.Description = @"Please select your 'RaceRoom Racing Experience' directory (Documents\My Games\SimBin\RaceRoom Racing Experience). Its name may end with 'Install N'.";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                locationModel.R3eHomeBaseDirectory = dialog.SelectedPath;
                File.WriteAllText(txtFile, locationModel.R3eHomeBaseDirectory);
            }
            else
            {
                MessageBox.Show("Can't find R3E user directory, application will exit.", "Error");
                Environment.Exit(0);
            }
        }
    }
}
