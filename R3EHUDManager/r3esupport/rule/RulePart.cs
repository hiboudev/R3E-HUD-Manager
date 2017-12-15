using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.rule
{
    class RulePart
    {
        private readonly PropertyType property;
        private readonly Operation[] operations;

        public string Description { get; }

        public RulePart(PropertyType property, Operation[] operations, string description)
        {
            this.property = property;
            this.operations = operations;
            Description = description;
        }

        public bool Matches(PlaceholderModel placeholder)
        {
            foreach (Operation operation in operations)
                if (!operation.Matches(GetPropertyValue(placeholder)))
                    return false;

            return true;
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
