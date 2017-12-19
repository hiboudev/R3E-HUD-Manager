﻿using da2mvc.core.command;
using da2mvc.core.events;
using da2mvc.framework.collection.events;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
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

        private readonly BaseEventArgs args;
        private readonly SupportRuleValidator validator;

        // TODO improve commands in MVC by choosing the ctor depending of event arg type.
        public ValidatePlaceholderCommand(BaseEventArgs args, SupportRuleValidator validator)
        {
            this.args = args;
            this.validator = validator;
        }

        public void Execute()
        {
            if (args is PlaceHolderUpdatedEventArgs)
                ValidatePlaceholders(new PlaceholderModel[] { ((PlaceHolderUpdatedEventArgs)args).Placeholder });
            else if(args is CollectionEventArgs<PlaceholderModel>)
                ValidatePlaceholders(((CollectionEventArgs<PlaceholderModel>)args).ChangedItems);
        }

        private void ValidatePlaceholders(PlaceholderModel[] placeholders)
        {
            foreach(var placeholder in placeholders)
            {
                ValidationResult result = validator.Matches(placeholder);

                placeholder.SetValidationResult(result);

                //if (result.HasFix()) result.ApplyFixes(placeholder); // TODO experimental autofix, dangerous like it is if a rule doesn't fix the problem => infinite calls to this command
            }
        }
    }
}
