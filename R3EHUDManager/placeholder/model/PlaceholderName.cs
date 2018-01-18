using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    class PlaceholderName // TODO dynamic from the XML
    {
        public static HashSet<String> GetAll()
        {
            return new HashSet<string>
            {
                MOTEC,
                MINI_MOTEC,
                POSITION_BAR,
                VIRTUAL_MIRROR,
                FLAGS,
                TRACK_MAP,
                APEXHUNT_DISPLAY,
                DRIVER_NAME_TAGS,
                CAR_STATUS,
                FFB_GRAPH,
            };
        }

        public const string MOTEC = "MoTeC";
        public const string MINI_MOTEC = "Mini MoTeC";
        public const string POSITION_BAR = "Position Bar";
        public const string VIRTUAL_MIRROR = "Virtual Mirror";
        public const string FLAGS = "Flags";
        public const string TRACK_MAP = "Track Map";
        public const string APEXHUNT_DISPLAY = "Apexhunt Display";
        public const string DRIVER_NAME_TAGS = "Driver Name Tags";
        public const string CAR_STATUS = "Car Status";
        public const string FFB_GRAPH = "FFB Graph";

    }
}
