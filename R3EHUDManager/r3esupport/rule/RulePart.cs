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
        private readonly PropertyCheck[] checks;

        public string Description { get; }
        public Fix[] Fixes { get; }

        public RulePart(PropertyCheck[] checks, string description, Fix[] fixes)
        {
            this.checks = checks;
            Description = description;
            Fixes = fixes;
        }

        public bool Matches(PlaceholderModel placeholder)
        {
            foreach (PropertyCheck check in checks)
                if (!check.Matches(placeholder))
                    return false;

            return true;
        }

        //private double GetPropertyValue(PlaceholderModel placeholder)
        //{
        //    switch (property)
        //    {
        //        case PropertyType.X:
        //        case PropertyType.Y:
        //            return (double)placeholder.Position.GetType().GetProperty(GetPropertyName(property)).GetValue(placeholder.Position);
        //        case PropertyType.SIZE:
        //            return placeholder.Size.X;
        //        case PropertyType.ANCHOR_X:
        //            return placeholder.Anchor.X;
        //        case PropertyType.ANCHOR_Y:
        //            return placeholder.Anchor.Y;

        //    }
        //    throw new Exception("Unsupported property type.");
        //}

        //private string GetPropertyName(PropertyType type)
        //{
        //    switch (type)
        //    {
        //        case PropertyType.X:
        //            return "X";
        //        case PropertyType.Y:
        //            return "Y";
        //        case PropertyType.SIZE:
        //            return "Size";
        //    }
        //    throw new Exception("Unsupported property type.");
        //}
    }
}
