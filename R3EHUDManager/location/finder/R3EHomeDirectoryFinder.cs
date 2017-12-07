using R3EHUDManager.log;
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

            Logger.Add($"* Searching R3E user directory in {simbinPath}.");

            string[] r3EDirectories = Directory.GetDirectories(simbinPath, $"{BASE_R3E_DIR_NAME}*");

            if (r3EDirectories.Length == 0)
            {
                Logger.Add($"No suitable directory found in {simbinPath}.");
                return null;
            }

            return SearchActiveR3EDirectory(r3EDirectories);
        }

        private string SearchActiveR3EDirectory(string[] r3EDirectories)
        {
            // "RaceRoom Racing Experience Install 7"
            Regex r3EDirRegex = new Regex($@"{BASE_R3E_DIR_NAME}( Install \d+)?");

            foreach (string r3EDirectory in r3EDirectories)
            {
                Logger.Add($"Testing directory '{r3EDirectory}'...");

                if (!r3EDirRegex.IsMatch(r3EDirectory))
                {
                    Logger.Add($"Directory name doesn't match.");
                    continue;
                }

                Logger.Add($"Directory name matches.");

                string iniFilePath = Path.Combine(r3EDirectory, INI_FILE_NAME);

                if (File.Exists(iniFilePath))
                {
                    Logger.Add($"Checking ini file '{iniFilePath}'...");

                    Regex steamDirExp = new Regex($"^{INI_INSTALL_DIR_KEY}(.*)$", RegexOptions.Multiline);
                    string gameInstallDir = null;

                    string[] lines = File.ReadAllLines(iniFilePath);
                    foreach (string line in lines)
                    {
                        if (!steamDirExp.IsMatch(line))
                            continue;

                        Match match = steamDirExp.Match(line);
                        gameInstallDir = match.Groups[1].Value;
                    }

                    Logger.Add($"Testing game install directory: '{gameInstallDir}'...");

                    if (gameInstallDir != null && Directory.Exists(gameInstallDir))
                    {
                        Logger.Add($"Found valid directory: '{r3EDirectory}'.");
                        return r3EDirectory;
                    }

                    Logger.Add($"Game install directory not valid.");
                }

                Logger.Add($"No ini file found in {r3EDirectory}.");
            }

            Logger.Add($"No suitable directory found.");

            return null;
        }
    }
}
