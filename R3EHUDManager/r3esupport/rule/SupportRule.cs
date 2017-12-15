using da2mvc.framework.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.rule
{
    class SupportRule
    {
        private RulePart[] parts = new RulePart[] { };
        private HashSet<string> targets = new HashSet<string>();
        private readonly RuleLayoutType layout;

        public string Name { get; }

        public SupportRule(string name, RuleLayoutType layout)
        {
            Name = name;
            this.layout = layout;
        }

        public void SetTargets(HashSet<string> placeholderNames)
        {
            this.targets = placeholderNames;
        }

        public void SetParts(RulePart[] parts)
        {
            this.parts = parts;
        }

        public bool Matches(PlaceholderModel placeholder, ref string description, ScreenLayoutType layout, List<Fix> fixes)
        {
            if (this.layout == RuleLayoutType.SINGLE && layout != ScreenLayoutType.SINGLE ||
                this.layout == RuleLayoutType.TRIPLE && layout != ScreenLayoutType.TRIPLE)
                return false;

            // If no target specified, all targets are concerned.
            if (targets.Count > 0 && !targets.Contains(placeholder.Name))
                return false;

            bool isMatch = false;

            foreach (var part in parts)
            {
                if (part.Matches(placeholder))
                {
                    isMatch = true;
                    
                    description += (description.Length > 0 ? Environment.NewLine : "") + part.Description;
                    fixes.AddRange(part.Fixes);
                }
            }

            return isMatch;
        }
    }
}
