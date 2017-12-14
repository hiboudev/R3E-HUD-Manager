using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager.r3esupport.rule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.command
{
    class ValidateRulesCommand : ICommand, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;

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
            ValidationResult result;

            if (validator.Matches(args.Placeholder, ref description))
            {
                result = ValidationResult.GetInvalid(description);
            }
            else
            {
                result = ValidationResult.GetValid();
            }

            args.Placeholder.SetValidationResult(result);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
