using da2mvc.framework.model;
using R3EHUDManager.placeholder.model;
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
        private HashSet<string> targets = new HashSet<string> ();

        public string Name { get; }

        public SupportRule(string name)
        {
            Name = name;
        }

        public void SetTargets(HashSet<string> placeholderNames)
        {
            this.targets = placeholderNames;
        }

        public void SetParts(RulePart[] parts)
        {
            this.parts = parts;
        }

        public bool Matches(PlaceholderModel placeholder, UpdateType updateType, ref string description)
        {
            if (!targets.Contains(placeholder.Name))
                return false;

            description = "";
            bool isMatch = false;

            foreach(var part in parts)
            {
                if (part.Matches(placeholder, updateType))
                {
                    isMatch = true;
                    description += part.Description + Environment.NewLine;
                }
            }

            if(description.Length > 0)
                description = description.Substring(0, description.Length - Environment.NewLine.Length);

            return isMatch;
        }
    }
}
