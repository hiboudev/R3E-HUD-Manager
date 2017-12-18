using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.model
{
    class LayoutIOModel
    {
        private readonly HudOptionsParser parser;
        private readonly LocationModel location;

        public LayoutIOModel(HudOptionsParser parser, LocationModel location)
        {
            this.parser = parser;
            this.location = location;
        }

        public List<PlaceholderModel> LoadR3eLayout()
        {
            return parser.Parse(location.HudOptionsFile);
        }

        public List<PlaceholderModel> LoadDefaultR3eLayout()
        {
            return parser.Parse(location.HudOptionsBackupFile);
        }

        public List<PlaceholderModel> LoadProfileLayout(ProfileModel profile)
        {
            return parser.Parse(Path.Combine(location.LocalDirectoryProfiles, profile.FileName));
        }

        public void WriteR3eLayout(List<PlaceholderModel> placeholders)
        {
            parser.Write(location.HudOptionsFile, placeholders);
        }

        public void WriteProfileLayout(ProfileModel profile, List<PlaceholderModel> placeholders)
        {
            parser.Write(Path.Combine(location.LocalDirectoryProfiles, profile.FileName), placeholders);
        }
    }
}
