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

        public static R3ePoint ToScreenOffset(PlaceholderModel placeholder, ScreenPositionType targetScreen)
        {
            double zeroX = placeholder.Position.X + 3;
            double moduloX = zeroX % 2;
            double positionXInScreen = Math.Abs(moduloX);

            // Replace placeholder Y in screen in case it's outside.
            double positionYInScreen = placeholder.Position.Y;
            if (placeholder.Position.Y < -1) positionYInScreen = -1;
            else if (placeholder.Position.Y > 1) positionYInScreen = 1;

            switch (targetScreen)
            {
                case ScreenPositionType.LEFT:
                    return new R3ePoint(positionXInScreen - 3 - placeholder.Position.X, positionYInScreen - placeholder.Position.Y);

                case ScreenPositionType.RIGHT:
                    return new R3ePoint(positionXInScreen + 1 - placeholder.Position.X, positionYInScreen - placeholder.Position.Y);

                case ScreenPositionType.CENTER:
                    return new R3ePoint(positionXInScreen - 1 - placeholder.Position.X, positionYInScreen - placeholder.Position.Y);

                case ScreenPositionType.OUTSIDE:
                    return new R3ePoint(0, 0);

                default:
                    throw new Exception("Not implemented type.");
            }
        }
    }
}
