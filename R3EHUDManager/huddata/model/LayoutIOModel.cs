using da2mvc.core.events;
using R3EHUDManager.huddata.events;
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
    class LayoutIOModel : EventDispatcher
    {
        public static readonly int EVENT_SOURCE_CHANGED = EventId.New();

        private readonly HudOptionsParser parser;
        private readonly LocationModel location;
        private SourceLayout source;

        public LayoutIOModel(HudOptionsParser parser, LocationModel location)
        {
            this.parser = parser;
            this.location = location;
        }

        public List<PlaceholderModel> LoadR3eLayout()
        {
            return SetSource(LayoutSourceType.R3E, location.HudOptionsFile, parser.Parse(location.HudOptionsFile));
        }

        public List<PlaceholderModel> LoadDefaultR3eLayout()
        {
            return SetSource(LayoutSourceType.BACKUP, location.HudOptionsBackupFile, parser.Parse(location.HudOptionsBackupFile));
        }

        public List<PlaceholderModel> LoadProfileLayout(ProfileModel profile)
        {
            return SetSource(LayoutSourceType.PROFILE, profile.Name, parser.Parse(Path.Combine(location.LocalDirectoryProfiles, profile.FileName)));
        }

        public void WriteR3eLayout(List<PlaceholderModel> placeholders)
        {
            parser.Write(location.HudOptionsFile, placeholders);
        }

        public void WriteProfileLayout(ProfileModel profile, List<PlaceholderModel> placeholders)
        {
            parser.Write(Path.Combine(location.LocalDirectoryProfiles, profile.FileName), placeholders);
        }

        private List<PlaceholderModel> SetSource(LayoutSourceType sourceType, String name, List<PlaceholderModel> list)
        {
            List<PlaceholderModel> newList = new List<PlaceholderModel>();
            foreach (var placeholder in list)
                newList.Add(placeholder.Clone());

            source = new SourceLayout(sourceType, name, newList);

            DispatchEvent(new LayoutSourceEventArgs(EVENT_SOURCE_CHANGED, sourceType, name));

            return list;
        }



        class SourceLayout
        {
            public SourceLayout(LayoutSourceType sourceType, String name, List<PlaceholderModel> layout)
            {
                SourceType = sourceType;
                Name = name;
                Layout = layout;
            }

            public LayoutSourceType SourceType { get; }
            public string Name { get; }
            public List<PlaceholderModel> Layout { get; }
        }
    }
}
