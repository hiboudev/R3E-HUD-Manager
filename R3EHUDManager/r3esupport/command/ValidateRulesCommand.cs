using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.r3esupport.events;
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
        public static readonly int EVENT_INVALID_LAYOUT = EventId.New();
        public static readonly int EVENT_VALID_LAYOUT = EventId.New();

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
            if (validator.Matches(args.Placeholder, ref description))
            {
                DispatchEvent(new InvalidLayoutEventArgs(EVENT_INVALID_LAYOUT, args.Placeholder, description));
            }
            else DispatchEvent(new IntEventArgs(EVENT_VALID_LAYOUT, args.Placeholder.Id));
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
