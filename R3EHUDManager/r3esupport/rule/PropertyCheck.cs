using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3EHUDManager.placeholder.model;

namespace R3EHUDManager.r3esupport.rule
{
    public class PropertyCheck
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
                    return placeholder.Position.X;

                case CheckPropertyType.Y:
                    return placeholder.Position.Y;

                case CheckPropertyType.SIZE:
                    return placeholder.Size.X;

                case CheckPropertyType.ANCHOR_X:
                    return placeholder.Anchor.X;

                case CheckPropertyType.ANCHOR_Y:
                    return placeholder.Anchor.Y;

            }
            throw new Exception("Unsupported property type.");
        }
    }
}
