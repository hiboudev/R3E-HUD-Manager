using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.graphics;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
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
        public static readonly int EVENT_SAVE_STATUS = EventId.New();

        private readonly HudOptionsParser parser;
        private readonly LocationModel location;
        private readonly PlaceHolderCollectionModel collection;

        private readonly ScreenModel screenModel;
        private readonly GraphicalAssetFactory assetFactory;
        private SourceLayout source;
        private SaveStatus saveStatus;
        private SourceLayout currentR3eLayout;

        public LayoutIOModel(HudOptionsParser parser, LocationModel location, PlaceHolderCollectionModel collection, ScreenModel screenModel, GraphicalAssetFactory assetFactory)
        {
            this.parser = parser;
            this.location = location;
            this.collection = collection;
            this.screenModel = screenModel;
            this.assetFactory = assetFactory;
            saveStatus = new SaveStatus();
            currentR3eLayout = new SourceLayout(LayoutSourceType.R3E, location.HudOptionsFile, parser.Parse(location.HudOptionsFile), -1, -1);
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
            return LoadLayout(LayoutSourceType.PROFILE, profile.Name, Path.Combine(location.LocalDirectoryProfiles, profile.FileName), profile.BackgroundId, profile.MotecId);
        }

        public void WriteR3eLayout(List<PlaceholderModel> placeholders)
        {
            parser.Write(location.HudOptionsFile, placeholders);
            //if (source.SourceType == LayoutSourceType.R3E || source.SourceType == LayoutSourceType.BACKUP)
            currentR3eLayout.UpdateLayout(placeholders);
        }

        public void WriteProfileLayout(ProfileModel profile, List<PlaceholderModel> placeholders)
        {
            parser.Write(Path.Combine(location.LocalDirectoryProfiles, profile.FileName), placeholders);
            SetSource(LayoutSourceType.PROFILE, profile.Name, placeholders, profile.BackgroundId, profile.MotecId);
        }

        public void ProfileDeleted(ProfileModel profile)
        {
            // TODO pas top de reposer sur le Name.
            if (source.SourceType == LayoutSourceType.PROFILE && source.Name == profile.Name)
            {
                // TODO Actuellement pas de prompt pour unsaved.
                SetSource(LayoutSourceType.DELETED_PROFILE, profile.Name, source.Layout, -1, -1);
            }
        }

        public UnsavedChangeType GetSaveStatus()
        {
            if (source == null) return UnsavedChangeType.PROFILE | UnsavedChangeType.R3E;

            bool profileIsSaved;
            bool r3eIsSaved;

            if (source.SourceType == LayoutSourceType.PROFILE)
            {
                profileIsSaved = saveStatus.IsSaved(collection.Items, source, currentR3eLayout, screenModel, assetFactory);
                r3eIsSaved = saveStatus.IsSaved(collection.Items, currentR3eLayout, currentR3eLayout, screenModel, assetFactory);
            }
            else
            {
                profileIsSaved = true;
                r3eIsSaved = saveStatus.IsSaved(collection.Items, source, currentR3eLayout, screenModel, assetFactory);
            }

            return (profileIsSaved ? UnsavedChangeType.PROFILE : 0) | (r3eIsSaved ? UnsavedChangeType.R3E : 0);
        }

        private List<PlaceholderModel> LoadLayout(LayoutSourceType type, string name, string path, int backgroundId = -1, int motecId = -1)
        {
            // TODO quand on recharge l'original on devrait comparer le layout actuel avec le R3E et non l'original.
            if (!saveStatus.IsSaved(collection.Items, source, currentR3eLayout, screenModel, assetFactory))
            {
                UnsavedChangesEventArgs args = new UnsavedChangesEventArgs(EVENT_UNSAVED_CHANGES, ToUnsavedType(source.SourceType), source.Name);
                DispatchEvent(args);
                if (args.IsLoadingCancelled)
                    return null;
            }
            return SetSource(type, name, parser.Parse(path), backgroundId, motecId);
        }

        private UnsavedChangeType ToUnsavedType(LayoutSourceType sourceType)
        {
            switch (sourceType)
            {
                case LayoutSourceType.PROFILE:
                    return UnsavedChangeType.PROFILE;

                case LayoutSourceType.R3E:
                case LayoutSourceType.BACKUP:
                case LayoutSourceType.DELETED_PROFILE:
                    return UnsavedChangeType.R3E;

                default:
                    throw new Exception("Unsupported type.");
            }
        }

        // TODO nullable backgroundId
        private List<PlaceholderModel> SetSource(LayoutSourceType sourceType, String name, List<PlaceholderModel> list, int backgroundId, int motecId)
        {
            if (sourceType == LayoutSourceType.R3E)
            {
                currentR3eLayout.UpdateLayout(list);
                //currentR3eLayout.UpdateBackgroundId(backgroundId);

                source = currentR3eLayout;
            }
            else
                source = new SourceLayout(sourceType, name, list, backgroundId, motecId);

            DispatchEvent(new LayoutSourceEventArgs(EVENT_SOURCE_CHANGED, sourceType, name));

            return list;
        }

        internal void DispatchSaveStatus()
        {
            DispatchEvent(new SaveStatusEventArgs(EVENT_SAVE_STATUS, GetSaveStatus()));
        }



        class SourceLayout
        {
            public SourceLayout(LayoutSourceType sourceType, String name, List<PlaceholderModel> layout, int backgroundId, int motecId)
            {
                SourceType = sourceType;
                Name = name;
                Layout = CloneLayout(layout);
                BackgroundId = backgroundId;
                MotecId = motecId;
            }

            public void UpdateLayout(List<PlaceholderModel> layout)
            {
                Layout = CloneLayout(layout);
            }

            //public void UpdateBackgroundId(int backgroundId)
            //{
            //    BackgroundId = backgroundId;
            //}

            private List<PlaceholderModel> CloneLayout(List<PlaceholderModel> layout)
            {
                List<PlaceholderModel> newList = new List<PlaceholderModel>();
                foreach (var placeholder in layout)
                    newList.Add(placeholder.Clone());
                return newList;
            }

            public LayoutSourceType SourceType { get; }
            public string Name { get; }
            public List<PlaceholderModel> Layout { get; private set; }
            public int BackgroundId { get; private set; } = -1;
            public int MotecId { get; } = -1;
        }



        class SaveStatus
        {
            public bool IsSaved(List<PlaceholderModel> currentLayout, SourceLayout source, SourceLayout r3eLayout, ScreenModel screenModel, GraphicalAssetFactory assetFactory)
            {
                if (source == null) return true;
                if (source.SourceType == LayoutSourceType.PROFILE && (source.BackgroundId != screenModel.Background.Id || source.MotecId != assetFactory.SelectedMotec.Id))
                    return false;
                if (source.SourceType == LayoutSourceType.BACKUP)
                    return AreLayoutEquals(currentLayout, source.Layout) || AreLayoutEquals(currentLayout, r3eLayout.Layout);
                if (source.SourceType == LayoutSourceType.R3E)
                    return AreLayoutEquals(currentLayout, source.Layout); // TODO ici on pourrait éventuellement comparer avec le backup
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
