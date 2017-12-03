using R3EHUDManager.coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.selection.view
{
    class AnchorPreset
    {
        public static readonly Dictionary<string, R3ePoint> presets;

        static AnchorPreset()
        {
            presets = new Dictionary<string, R3ePoint>
            {
                { "Top Left", new R3ePoint(-1, 1) },
                { "Top Middle", new R3ePoint(0, 1) },
                { "Top Right", new R3ePoint(1, 1) },

                { "Left Center", new R3ePoint(-1, 0) },
                { "Center", new R3ePoint(0, 0) },
                { "Right Center", new R3ePoint(1, 0) },

                { "Bottom Left", new R3ePoint(-1, -1) },
                { "Bottom Middle", new R3ePoint(0, -1) },
                { "Bottom Right", new R3ePoint(1, -1) }
            };
        }

        public static R3ePoint GetPreset(string name)
        {
            return presets[name].Clone();
        }

        public static string GetPresetName(R3ePoint anchor)
        {
            foreach(KeyValuePair<string, R3ePoint> entry in presets)
                if (entry.Value.Equals(anchor))
                    return entry.Key;

            return null;
        }
    }

}
