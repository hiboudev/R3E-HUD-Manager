using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            if (Px >= -3 && Px < -1)
                return ScreenPositionType.LEFT;

            if (Px > -1 && Px < 1)
                return ScreenPositionType.CENTER;

            if (Px > 1 && Px <= 3)
                return ScreenPositionType.RIGHT;

            return ScreenPositionType.OUTSIDE;
        }

        public static double ToScreenOffset(PlaceholderModel placeholder, ScreenPositionType targetScreen)
        {
            double zeroX = placeholder.Position.X + 3;
            double moduloX = zeroX % 2;
            double positionXInScreen = Math.Abs(moduloX);

            switch (targetScreen)
            {
                case ScreenPositionType.LEFT:
                    return positionXInScreen - 3 - placeholder.Position.X;

                case ScreenPositionType.RIGHT:
                    return positionXInScreen + 1 - placeholder.Position.X;

                case ScreenPositionType.CENTER:
                    return positionXInScreen - 1 - placeholder.Position.X;

                case ScreenPositionType.OUTSIDE:
                    return 0;

                default:
                    throw new Exception("Not implemented type.");
            }
        }
    }
}
