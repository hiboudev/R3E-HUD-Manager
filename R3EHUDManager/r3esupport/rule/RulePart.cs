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
        private readonly string propertyName;
        private readonly Operation[] operations;

        public string Description { get; }

        public RulePart(PropertyType property, Operation[] operations, string description)
        {
            this.property = property;
            propertyName = GetPropertyName(property);
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
                    return (double)placeholder.Position.GetType().GetProperty(propertyName).GetValue(placeholder.Position);
                case PropertyType.SIZE:
                    return placeholder.Size.X;

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
