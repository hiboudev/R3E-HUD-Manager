using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
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

        public void Apply(PlaceholderGeom geom, ScreenModel screenModel, IResizeRule resizeRule)
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
                    Size backgroundSize = screenModel.GetBackgroundImage().PhysicalDimension.ToSize();
                    float bitmapWidth = resizeRule.GetSize(ScreenView.BASE_RESOLUTION, backgroundSize, geom.BitmapSize, screenModel.Layout == ScreenLayoutType.TRIPLE).Width;
                    double r3eWidth = geom.Size.X * 2 * value * bitmapWidth / backgroundSize.Width;
                    geom.Move(new R3ePoint(geom.Position.X + r3eWidth, geom.Position.Y));
                    break;
                case FixPropertyType.HEIGHT:
                    backgroundSize = screenModel.GetBackgroundImage().PhysicalDimension.ToSize();
                    float bitmapHeight = resizeRule.GetSize(ScreenView.BASE_RESOLUTION, backgroundSize, geom.BitmapSize, screenModel.Layout == ScreenLayoutType.TRIPLE).Height;
                    double r3eHeight = geom.Size.Y * 2 * value * bitmapHeight / backgroundSize.Height;
                    geom.Move(new R3ePoint(geom.Position.X, geom.Position.Y + r3eHeight));
                    break;

                default:
                    throw new Exception("Not implemented property type.");
            }
        }
    }
}
