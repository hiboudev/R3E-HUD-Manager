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
        private readonly bool matchAny;
        private readonly OperatorType operatorType;
        private readonly double value;

        public string Description { get; }

        public RulePart(PropertyType property, OperatorType operatorType, double value, string description)
        {
            this.property = property;
            propertyName = GetPropertyName(property);
            this.operatorType = operatorType;
            this.value = value;
            Description = description;
        }

        /**
         * Use this ctor to match any value.
         */
        public RulePart(PropertyType property, string description)
        {
            propertyName = GetPropertyName(property);
            matchAny = true;
            Description = description;
        }

        public bool Matches(PlaceholderModel placeholder)
        {
            //if (updateType == UpdateType.POSITION && (property != PropertyType.X && property != PropertyType.Y)) return false;
            //if (updateType == UpdateType.ANCHOR) return false; // TODO manage anchor
            //if (updateType == UpdateType.SIZE && (property != PropertyType.SIZE)) return false;

            if (matchAny) return true;


            return Matches(GetPropertyValue(placeholder));
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

        private bool Matches(double placeholderValue)
        {
            switch (operatorType)
            {
                case OperatorType.EQUAL:
                    return placeholderValue == value;
                case OperatorType.GREATER_OR_EQUAL:
                    return placeholderValue >= value;
                case OperatorType.LESS_OR_EQUAL:
                    return placeholderValue <= value;
                case OperatorType.GREATER:
                    return placeholderValue > value;
                case OperatorType.LESS:
                    return placeholderValue < value;
            }
            throw new Exception("Unsupported operator type.");
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
