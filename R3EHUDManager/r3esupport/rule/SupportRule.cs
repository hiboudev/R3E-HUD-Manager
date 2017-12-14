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

        public bool Matches(PlaceholderModel placeholder, ref string description, ScreenLayoutType layout)
        {
            if (this.layout == RuleLayoutType.SINGLE && layout != ScreenLayoutType.SINGLE ||
                this.layout == RuleLayoutType.TRIPLE && layout != ScreenLayoutType.TRIPLE)
                return false;

            // If no target specified, all targets are concerned.
            if (targets.Count > 0 && !targets.Contains(placeholder.Name))
                return false;

            description = "";
            bool isMatch = false;

            foreach (var part in parts)
            {
                if (part.Matches(placeholder))
                {
                    isMatch = true;
                    description += part.Description + Environment.NewLine;
                }
            }

            if (description.Length > 0)
                description = description.Substring(0, description.Length - Environment.NewLine.Length);

            return isMatch;
        }
    }
}
