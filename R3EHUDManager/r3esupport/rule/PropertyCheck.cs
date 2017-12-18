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
        private readonly CheckPropertyType property;
        private readonly Operation operation;

        public PropertyCheck(CheckPropertyType property, Operation operation)
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
                case CheckPropertyType.X:
                case CheckPropertyType.Y:
                    return (double)placeholder.Position.GetType().GetProperty(GetPropertyName(property)).GetValue(placeholder.Position);
                case CheckPropertyType.SIZE:
                    return placeholder.Size.X;
                case CheckPropertyType.ANCHOR_X:
                    return placeholder.Anchor.X;
                case CheckPropertyType.ANCHOR_Y:
                    return placeholder.Anchor.Y;

            }
            throw new Exception("Unsupported property type.");
        }

        private string GetPropertyName(CheckPropertyType type)
        {
            switch (type)
            {
                case CheckPropertyType.X:
                    return "X";
                case CheckPropertyType.Y:
                    return "Y";
                case CheckPropertyType.SIZE:
                    return "Size";
            }
            throw new Exception("Unsupported property type.");
        }
    }
}
