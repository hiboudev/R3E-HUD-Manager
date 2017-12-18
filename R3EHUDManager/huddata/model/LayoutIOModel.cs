using da2mvc.core.events;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.model
{
    class LayoutIOModel : EventDispatcher
    {
        public static readonly int EVENT_SOURCE_CHANGED = EventId.New();
        public static readonly int EVENT_UNSAVED_CHANGES = EventId.New();

        private readonly HudOptionsParser parser;
        private readonly LocationModel location;
        private readonly PlaceHolderCollectionModel collection;
        private SourceLayout source;
        private SaveStatus saveStatus;

        public LayoutIOModel(HudOptionsParser parser, LocationModel location, PlaceHolderCollectionModel collection)
        {
            this.parser = parser;
            this.location = location;
            this.collection = collection;
            saveStatus = new SaveStatus();
        }

        public List<PlaceholderModel> LoadR3eLayout()
        {
            return LoadLayout(LayoutSourceType.R3E, location.HudOptionsFile, location.HudOptionsFile);
        }

        public List<PlaceholderModel> LoadDefaultR3eLayout()
        {
            return LoadLayout(LayoutSourceType.BACKUP, location.HudOptionsBackupFile, location.HudOptionsBackupFile);
        }

        public List<PlaceholderModel> LoadProfileLayout(ProfileModel profile)
        {
            return LoadLayout(LayoutSourceType.PROFILE, profile.Name, Path.Combine(location.LocalDirectoryProfiles, profile.FileName));
        }

        public void WriteR3eLayout(List<PlaceholderModel> placeholders)
        {
            parser.Write(location.HudOptionsFile, placeholders);
        }

        public void WriteProfileLayout(ProfileModel profile, List<PlaceholderModel> placeholders)
        {
            parser.Write(Path.Combine(location.LocalDirectoryProfiles, profile.FileName), placeholders);
        }

        private List<PlaceholderModel> LoadLayout(LayoutSourceType type, string name, string path)
        {
            // TODO quand on recharge l'original on devrait comparer le layout actuel avec le R3E et non l'original.
            if (!saveStatus.IsSaved(collection.Items, source))
            {
                UnsavedChangesEventArgs args = new UnsavedChangesEventArgs(EVENT_UNSAVED_CHANGES, ToUnsavedType(source.SourceType), source.Name);
                DispatchEvent(args);
                if (args.IsLoadingCancelled)
                    return null;
            }
            return SetSource(type, name, parser.Parse(path));
        }

        private UnsavedChangeType ToUnsavedType(LayoutSourceType sourceType)
        {
            switch (sourceType)
            {
                case LayoutSourceType.PROFILE:
                    return UnsavedChangeType.PROFILE;

                case LayoutSourceType.R3E:
                case LayoutSourceType.BACKUP:
                    return UnsavedChangeType.R3E;

                default:
                    throw new Exception("Unsupported type.");
            }
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



        class SaveStatus
        {
            public bool IsSaved(List<PlaceholderModel> currentLayout, SourceLayout source)
            {
                if (source == null) return true;
                return AreLayoutEquals(currentLayout, source.Layout);
            }

            private bool AreLayoutEquals(List<PlaceholderModel> layout1, List<PlaceholderModel> layout2)
            {
                // Actually possible if we filtered placeholders but didn't reload layout.
                if (layout1.Count != layout2.Count) return false;

                Dictionary<string, PlaceholderModel> placeholders1 = layout1.ToDictionary(x => x.Name, x => x);
                Dictionary<string, PlaceholderModel> placeholders2 = layout2.ToDictionary(x => x.Name, x => x);



                foreach (KeyValuePair<string, PlaceholderModel> keyValue in placeholders1)
                {
                    if (!keyValue.Value.Position.Equals(placeholders2[keyValue.Key].Position)) return false;
                    if (!keyValue.Value.Anchor.Equals(placeholders2[keyValue.Key].Anchor)) return false;
                    if (!keyValue.Value.Size.Equals(placeholders2[keyValue.Key].Size)) return false;
                }

                return true;
            }
        }


    }
}
