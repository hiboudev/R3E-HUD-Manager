using da2mvc.core.command;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.command
{
    class ApplyLayoutFixCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly PlaceHolderCollectionModel placeholderCollection;

        public ApplyLayoutFixCommand(IntEventArgs args, PlaceHolderCollectionModel placeholderCollection)
        {
            this.args = args;
            this.placeholderCollection = placeholderCollection;
        }

        public void Execute()
        {
            PlaceholderModel placeholder = placeholderCollection.Get(args.Value);
            if (placeholder.ValidationResult.HasFix())
                placeholder.ValidationResult.ApplyFixes(placeholder);
        }
    }
}
