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
    }
}
