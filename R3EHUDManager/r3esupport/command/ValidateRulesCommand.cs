using da2mvc.core.command;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.r3esupport.rule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.command
{
    class ValidateRulesCommand : ICommand
    {
        private readonly PlaceHolderUpdatedEventArgs args;
        private readonly SupportRuleValidator validator;

        public ValidateRulesCommand(PlaceHolderUpdatedEventArgs args, SupportRuleValidator validator)
        {
            this.args = args;
            this.validator = validator;
        }

        public void Execute()
        {
            string description = "";
            if(validator.Matches(args.Placeholder, args.UpdateType, ref description))
            {
                Debug.WriteLine("----------------");
                Debug.WriteLine(description);
            }
        }
    }
}
