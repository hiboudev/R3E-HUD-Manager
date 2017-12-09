using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.screen.model
{
    class ScreenUtils
    {
        public static ScreenPositionType GetScreen(PlaceholderModel placeholder)
        {
            double Px = placeholder.Position.X;
            double Ax = placeholder.Anchor.X;

            if (Px == -1)
                return Ax > 0 ? ScreenPositionType.LEFT : ScreenPositionType.CENTER;

            if (Px == 1)
                return Ax > 0 ? ScreenPositionType.CENTER : ScreenPositionType.RIGHT;

            if (Px < -1)
                return ScreenPositionType.LEFT;

            if (Px > -1 && Px < 1)
                return ScreenPositionType.CENTER;

            if (Px > 1)
                return ScreenPositionType.RIGHT;

            throw new Exception("This can't happen.");
        }

        public static double ToScreenOffset(PlaceholderModel placeholder, ScreenPositionType targetScreen)
        {
            ScreenPositionType currentScreen = GetScreen(placeholder);

            if (currentScreen == targetScreen) return 0;

            switch (targetScreen)
            {
                case ScreenPositionType.LEFT:
                    if (currentScreen == ScreenPositionType.CENTER) return -2;
                    else return -4;

                case ScreenPositionType.RIGHT:
                    if (currentScreen == ScreenPositionType.CENTER) return 2;
                    else return 4;

                case ScreenPositionType.CENTER:
                    if (currentScreen == ScreenPositionType.LEFT) return 2;
                    else return -2;
            }

            return 0;
        }
    }
}
