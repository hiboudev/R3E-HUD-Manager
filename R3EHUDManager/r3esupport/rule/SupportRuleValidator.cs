using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.rule
{
    class SupportRuleValidator
    {
        private SupportRule[] rules = new SupportRule[] { };

        public void SetRules(SupportRule[] rules)
        {
            this.rules = rules;
        }

        public bool Matches(PlaceholderModel placeholder, ref string description)
        {
            description = "";
            bool isMatch = false;

            foreach (var rule in rules)
            {
                if (rule.Matches(placeholder, ref description))
                {
                    isMatch = true;
                }
            }

            return isMatch;
        }
    }
}
