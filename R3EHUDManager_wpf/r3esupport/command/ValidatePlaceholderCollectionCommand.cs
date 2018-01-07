using da2mvc.core.command;
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
    class ValidatePlaceholderCollectionCommand : ICommand
    {
        private readonly CollectionEventArgs<PlaceholderModel> args;

        // TODO improve commands in MVC by choosing the ctor depending of event arg type.
        public ValidatePlaceholderCollectionCommand(CollectionEventArgs<PlaceholderModel> args)
        {
            this.args = args;
        }

        public void Execute()
        {
            foreach (var placeholder in args.ChangedItems)
            {
                placeholder.UpdateLayoutValidation();
            }
        }
    }
}
