using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager.r3esupport.rule;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.command
{
    class ValidatePlaceholderCommand : ICommand
    {

        private readonly PlaceHolderUpdatedEventArgs args;
        private readonly SupportRuleValidator validator;
        private readonly ScreenModel screenModel;

        public ValidatePlaceholderCommand(PlaceHolderUpdatedEventArgs args, SupportRuleValidator validator, ScreenModel screenModel)
        {
            this.args = args;
            this.validator = validator;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            string description = "";
            ValidationResult result;

            List<Fix> fixes = new List<Fix>();

            if (validator.Matches(args.Placeholder, screenModel.Layout, ref description, fixes))
                result = ValidationResult.GetInvalid(description, fixes);
            else
                result = ValidationResult.GetValid();

            args.Placeholder.SetValidationResult(result);
        }
    }
}
