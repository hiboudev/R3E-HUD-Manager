using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.validator
{
    class PlaceholderUserChangeValidator
    {
        private readonly ScreenModel screenModel;

        public PlaceholderUserChangeValidator(ScreenModel screenModel)
        {
            this.screenModel = screenModel;
        }

        public R3ePoint GetValidPosition(PlaceholderModel placeholder, R3ePoint wantedPosition)
        {
            double validX = wantedPosition.X;
            double validY = wantedPosition.Y;

            int xMin = screenModel.Layout == ScreenLayoutType.SINGLE ? -1 : -3;
            int xMax = screenModel.Layout == ScreenLayoutType.SINGLE ? 1 : 3;
            int yMin = -1;
            int yMax = 1;

            // No check if placeholder is already outside the screen (case: we were in triple screen and switch to a single screen background, and at prompt we choose NOT to move placeholders in center screen.
            if (wantedPosition.X < xMin && placeholder.Position.X >= xMin) validX = xMin;
            else if (wantedPosition.X > xMax && placeholder.Position.X <= xMax) validX = xMax;
            if (wantedPosition.Y < yMin && placeholder.Position.Y >= yMin) validY = yMin;
            else if (wantedPosition.Y > yMax && placeholder.Position.Y <= yMax) validY = yMax;

            return new R3ePoint(validX, validY);
        }

        public R3ePoint GetValidSize(PlaceholderModel placeholder, double wantedSize)
        {
            if (wantedSize < 0.1) return new R3ePoint(0.1, 0.1);
            if (wantedSize > 1) return new R3ePoint(1, 1);

            return new R3ePoint(wantedSize, wantedSize);
        }
    }
}
