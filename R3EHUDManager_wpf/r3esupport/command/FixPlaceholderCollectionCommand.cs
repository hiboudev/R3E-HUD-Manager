using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.r3esupport.command
{
    class FixPlaceholderCollectionCommand : ICommand
    {
        private readonly PlaceHolderCollectionModel collection;

        public FixPlaceholderCollectionCommand(PlaceHolderCollectionModel collection)
        {
            this.collection = collection;
        }

        public void Execute()
        {
            foreach (var placeholder in collection.Items)
            {
                placeholder.UpdateLayoutValidation();
                placeholder.ApplyLayoutFix();
            }
        }
    }
}
