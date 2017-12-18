using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3EHUDManager.placeholder.model;

namespace R3EHUDManager.r3esupport.rule
{
    class PropertyCheck
    {
        private readonly PropertyType property;
        private readonly Operation operation;

        public PropertyCheck(PropertyType property, Operation operation)
        {
            this.property = property;
            this.operation = operation;
        }

        internal bool Matches(PlaceholderModel placeholder)
        {
            return operation.Matches(GetPropertyValue(placeholder));
        }

        private double GetPropertyValue(PlaceholderModel placeholder)
        {
            switch (property)
            {
                case PropertyType.X:
                case PropertyType.Y:
                    return (double)placeholder.Position.GetType().GetProperty(GetPropertyName(property)).GetValue(placeholder.Position);
                case PropertyType.SIZE:
                    return placeholder.Size.X;
                case PropertyType.ANCHOR_X:
                    return placeholder.Anchor.X;
                case PropertyType.ANCHOR_Y:
                    return placeholder.Anchor.Y;

            }
            throw new Exception("Unsupported property type.");
        }

        private string GetPropertyName(PropertyType type)
        {
            switch (type)
            {
                case PropertyType.X:
                    return "X";
                case PropertyType.Y:
                    return "Y";
                case PropertyType.SIZE:
                    return "Size";
            }
            throw new Exception("Unsupported property type.");
        }
    }
}
