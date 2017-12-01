using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.model
{
    class LocationModel
    {
        public string R3eHomeBasePath { get; internal set; }

        public string HudOptionsPath { get => Path.Combine(R3eHomeBasePath, "UserData", "hud_options.xml"); }
    }
}
