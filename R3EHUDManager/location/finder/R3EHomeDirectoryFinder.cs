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
        private static readonly string HUD_FILE_SUBPATH = Path.Combine("UserData", "hud_options.xml");

        public List<string> GetPaths()
        {
            string simbinPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), RELATIVE_SIMBIN_PATH);

            string[] r3EDirectories = Directory.GetDirectories(simbinPath, $"{BASE_R3E_DIR_NAME}*");

            return SearchActiveR3EDirectory(r3EDirectories);
        }

        private List<string> SearchActiveR3EDirectory(string[] r3EDirectories)
        {
            List<string> directories = new List<string>();

            // "RaceRoom Racing Experience Install 7"
            Regex r3EDirRegex = new Regex($@"{BASE_R3E_DIR_NAME}( Install \d+)?$");

            foreach (string r3EDirectory in r3EDirectories)
            {
                if (!r3EDirRegex.IsMatch(r3EDirectory))
                {
                    continue;
                }

                string iniFilePath = Path.Combine(r3EDirectory, INI_FILE_NAME);

                if (File.Exists(iniFilePath))
                {

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

                    if (gameInstallDir != null && Directory.Exists(gameInstallDir) && File.Exists(Path.Combine(r3EDirectory, HUD_FILE_SUBPATH)))
                    {
                        directories.Add(r3EDirectory);
                    }
                }
            }

            return directories;
        }
    }
}
