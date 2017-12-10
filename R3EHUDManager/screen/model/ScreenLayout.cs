using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.screen.model
{
    class ScreenLayout
    {
        public ScreenLayout(ScreenLayoutType type, int leftResolution, int centerResolution, int rightResolution)
        {
            Type = type;
            LeftResolution = leftResolution;
            CenterResolution = centerResolution;
            RightResolution = rightResolution;
        }

        public bool Equals(ScreenLayout screen)
        {
            return screen.Type == Type
                && screen.LeftResolution == LeftResolution
                && screen.CenterResolution == CenterResolution
                && screen.RightResolution == RightResolution;
        }

        public ScreenLayoutType Type { get; }
        public int LeftResolution { get; }
        public int CenterResolution { get; }
        public int RightResolution { get; }
    }
}
