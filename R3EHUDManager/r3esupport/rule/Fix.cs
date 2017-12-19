using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.rule
{
    class Fix
    {
        private readonly double value;
        private readonly FixPropertyType property;

        public Fix(double value, FixPropertyType property)
        {
            this.value = value;
            this.property = property;
        }

        public void Apply(PlaceholderGeom geom)
        {
            switch (property)
            {
                case FixPropertyType.X:
                    geom.Move(new R3ePoint(value, geom.Position.Y));
                    break;
                case FixPropertyType.Y:
                    geom.Move(new R3ePoint(geom.Position.X, value));
                    break;
                case FixPropertyType.SIZE:
                    geom.Resize(new R3ePoint(value, value));
                    break;
                case FixPropertyType.ANCHOR_X:
                    geom.MoveAnchor(new R3ePoint(value, geom.Anchor.Y));
                    break;
                case FixPropertyType.ANCHOR_Y:
                    geom.MoveAnchor(new R3ePoint(geom.Anchor.X, value));
                    break;
                case FixPropertyType.WIDTH:
                    double r3eWidth = 2 * value * geom.BitmapSize.Width / ScreenView.BASE_RESOLUTION.Width;
                    geom.Move(new R3ePoint(geom.Position.X + r3eWidth, geom.Position.Y));
                    break;
                case FixPropertyType.HEIGHT:
                    double r3eHeight = 2 * value * geom.BitmapSize.Height / ScreenView.BASE_RESOLUTION.Height;
                    geom.Move(new R3ePoint(geom.Position.X, geom.Position.Y + r3eHeight));
                    break;

                default:
                    throw new Exception("Not implemented property type.");
            }
        }
    }
}
