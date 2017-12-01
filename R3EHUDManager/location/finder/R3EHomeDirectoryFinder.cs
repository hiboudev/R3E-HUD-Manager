using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace R3EHUDManager.location.finder
{
    class R3eHomeDirectoryFinder
    {
        private const string RELATIVE_SIMBIN_PATH = @"My Games\SimBin";
        private const string BASE_R3E_DIR_NAME = "RaceRoom Racing Experience";
        private const string INI_FILE_NAME = "GameInstallDir.ini";
        private const string INI_INSTALL_DIR_KEY = "GameInstallDir=";

        public string GetPath()
        {
            string simbinPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), RELATIVE_SIMBIN_PATH);

            string[] r3EDirectories = Directory.GetDirectories(simbinPath, $"{BASE_R3E_DIR_NAME}*");

            if (r3EDirectories.Length == 0)
                return null;

            return SearchActiveR3EDirectory(r3EDirectories);
        }

        private string SearchActiveR3EDirectory(string[] r3EDirectories)
        {
            // "RaceRoom Racing Experience Install 7"
            Regex r3EDirRegex = new Regex($@"{BASE_R3E_DIR_NAME}( Install \d+)?");

            foreach (string r3EDirectory in r3EDirectories)
            {
                if (!r3EDirRegex.IsMatch(r3EDirectory))
                    continue;

                Debug.WriteLine($"++ Testing directory: {r3EDirectory}");

                string iniFilePath = Path.Combine(r3EDirectory, INI_FILE_NAME);

                if (File.Exists(iniFilePath))
                {
                    string fileContent = File.ReadAllText(iniFilePath);
                    string gameInstallDir = fileContent.Substring(INI_INSTALL_DIR_KEY.Length);

                    if (Directory.Exists(gameInstallDir))
                    {
                        return r3EDirectory;
                    }
                }
            }
            throw new Exception("No R3E active directory found.");
        }
    }
}
