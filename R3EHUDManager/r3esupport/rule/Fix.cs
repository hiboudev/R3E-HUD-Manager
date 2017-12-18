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

        public void Apply(PlaceholderModel placeholder)
        {
            switch (property)
            {
                case FixPropertyType.X:
                    placeholder.Move(new R3ePoint(value, placeholder.Position.Y));
                    break;
                case FixPropertyType.Y:
                    placeholder.Move(new R3ePoint(placeholder.Position.X, value));
                    break;
                case FixPropertyType.SIZE:
                    placeholder.Resize(new R3ePoint(value, value));
                    break;
                case FixPropertyType.ANCHOR_X:
                    placeholder.MoveAnchor(new R3ePoint(value, placeholder.Anchor.Y));
                    break;
                case FixPropertyType.ANCHOR_Y:
                    placeholder.MoveAnchor(new R3ePoint(placeholder.Anchor.X, value));
                    break;
                case FixPropertyType.WIDTH:
                    int width = GraphicalAsset.GetPlaceholderSize(placeholder.Name).Width;
                    double r3eWidth = 2 * value * width / ScreenView.BASE_RESOLUTION.Width;
                    placeholder.Move(new R3ePoint(placeholder.Position.X + r3eWidth, placeholder.Position.Y));
                    break;
                case FixPropertyType.HEIGHT:
                    int height = GraphicalAsset.GetPlaceholderSize(placeholder.Name).Height;
                    double r3eHeight = 2 * value * height / ScreenView.BASE_RESOLUTION.Height;
                    placeholder.Move(new R3ePoint(placeholder.Position.X, placeholder.Position.Y + r3eHeight));
                    break;

                default:
                    throw new Exception("Not implemented property type.");
            }
        }
    }
}
