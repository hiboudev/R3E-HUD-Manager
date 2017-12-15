using da2mvc.core.command;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager.r3esupport.rule;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.command
{
    class ValidateCollectionCommand : ICommand
    {
        private readonly PlaceHolderCollectionModel placeholderCollection;
        private readonly SupportRuleValidator validator;
        private readonly ScreenModel screenModel;

        public ValidateCollectionCommand(PlaceHolderCollectionModel placeholderCollection, SupportRuleValidator validator, ScreenModel screenModel)
        {
            this.placeholderCollection = placeholderCollection;
            this.validator = validator;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            string description;
            ValidationResult result;

            foreach(var placeholder in placeholderCollection.Items)
            {
                description = "";

                if (validator.Matches(placeholder, screenModel.Layout, ref description))
                    result = ValidationResult.GetInvalid(description);
                else
                    result = ValidationResult.GetValid();

               placeholder.SetValidationResult(result);
            }
        }
    }
}
