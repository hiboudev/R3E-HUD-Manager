using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.model
{
    class ProfileModel
    {
        public ProfileModel(int id, string name, int backgroundId, string hudFilePath)
        {
            Id = id;
            Name = name;
            BackgroundId = backgroundId;
            HudFilePath = hudFilePath;
        }

        public int Id { get; }
        public string Name { get; }
        public int BackgroundId { get; }
        public string HudFilePath { get; }
    }
}
