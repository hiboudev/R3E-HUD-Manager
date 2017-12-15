using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.rule
{
    class Fix
    {
        private readonly double value;
        private readonly PropertyType property;

        public Fix(double value, PropertyType property)
        {
            this.value = value;
            this.property = property;
        }

        public void Apply(PlaceholderModel placeholder)
        {
            switch (property)
            {
                case PropertyType.X:
                    placeholder.Move(new R3ePoint(value, placeholder.Position.Y));
                    break;
                case PropertyType.Y:
                    placeholder.Move(new R3ePoint(placeholder.Position.X, value));
                    break;
                case PropertyType.SIZE:
                    placeholder.Resize(new R3ePoint(value, value));
                    break;
                case PropertyType.ANCHOR_X:
                    placeholder.MoveAnchor(new R3ePoint(value, placeholder.Anchor.Y));
                    break;
                case PropertyType.ANCHOR_Y:
                    placeholder.MoveAnchor(new R3ePoint(placeholder.Anchor.X, value));
                    break;

                default:
                    throw new Exception("Not implemented property type.");
            }
        }
    }
}
