using da2mvc.command;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.fileexplorer.command
{
    abstract class AbstractOpenFileExplorerCommand : ICommand
    {
        protected readonly LocationModel locationModel;

        public AbstractOpenFileExplorerCommand(LocationModel locationModel)
        {
            this.locationModel = locationModel;
        }

        abstract public void Execute();

        virtual protected void OpenFileExplorer(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Process.Start("explorer.exe", directoryPath);
            }
        }

        virtual protected void OpenFileExplorer(string directoryPath, string fileName)
        {
            string filePath = Path.Combine(directoryPath, fileName);
            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", "/select, " + filePath);
            }
        }
    }
}
