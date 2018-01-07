using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.rule
{
    public class SupportRuleValidator
    {
        private SupportRule[] rules = new SupportRule[] { };
        private readonly ScreenModel screenModel;

        public SupportRuleValidator(ScreenModel screenModel)
        {
            this.screenModel = screenModel;
        }

        public void SetRules(SupportRule[] rules)
        {
            this.rules = rules;
        }

        public LayoutValidationResult Matches(PlaceholderModel placeholder)
        {
            bool isMatch = false;

            string description = "";
            List<Fix> fixes = new List<Fix>();

            foreach (var rule in rules)
            {
                if (rule.Matches(placeholder, ref description, screenModel.Layout, fixes))
                {
                    isMatch = true;
                }
            }

            if (isMatch)
                return LayoutValidationResult.GetInvalid(description, fixes);
            else
                return LayoutValidationResult.GetValid();
        }
    }
}
