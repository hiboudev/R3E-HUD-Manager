using da2mvc.core.command;
using R3EHUDManager.r3esupport.parser;
using R3EHUDManager.r3esupport.rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.command
{
    class InitializeSupportRulesCommand : ICommand
    {
        private readonly SupportRuleParser parser;
        private readonly SupportRuleValidator validator;

        public InitializeSupportRulesCommand(SupportRuleParser parser, SupportRuleValidator validator)
        {
            this.parser = parser;
            this.validator = validator;
        }

        public void Execute()
        {
            validator.SetRules(parser.Parse(@"_r3eRules\r3e_support_rules.xml")); // TODO locationModel
        }
    }
}
