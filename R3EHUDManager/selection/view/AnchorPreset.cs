using R3EHUDManager.coordinates;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.selection.view
{
    class R3ePointPreset
    {
        public static readonly Dictionary<string, R3ePoint> presets;

        static R3ePointPreset()
        {
            presets = new Dictionary<string, R3ePoint>
            {
                { "Top Left", new R3ePoint(-1, 1) },
                { "Top Center", new R3ePoint(0, 1) },
                { "Top Right", new R3ePoint(1, 1) },

                { "Middle Left", new R3ePoint(-1, 0) },
                { "Middle Center", new R3ePoint(0, 0) },
                { "Middle Right", new R3ePoint(1, 0) },

                { "Bottom Left", new R3ePoint(-1, -1) },
                { "Bottom Center", new R3ePoint(0, -1) },
                { "Bottom Right", new R3ePoint(1, -1) }
            };
        }

        public static R3ePoint GetPreset(string name)
        {
            return presets[name].Clone();
        }

        public static R3ePoint GetPreset(string name, ScreenPositionType screenType)
        {
            R3ePoint preset = presets[name].Clone();

            switch (screenType)
            {
                case ScreenPositionType.LEFT:
                    return new R3ePoint(preset.X - 2, preset.Y);

                case ScreenPositionType.RIGHT:
                    return new R3ePoint(preset.X + 2, preset.Y);
            }

            return preset;
        }

        public static string GetPresetName(R3ePoint point)
        {
            foreach (KeyValuePair<string, R3ePoint> entry in presets)
                if (entry.Value.Equals(point))
                    return entry.Key;

            return null;
        }

        public static string GetPresetName(R3ePoint point, ScreenPositionType screenType)
        {
            if (screenType == ScreenPositionType.LEFT)
                point = new R3ePoint(point.X + 2, point.Y);
            else if (screenType == ScreenPositionType.RIGHT)
                point = new R3ePoint(point.X - 2, point.Y);

            return GetPresetName(point);
        }
    }
}
