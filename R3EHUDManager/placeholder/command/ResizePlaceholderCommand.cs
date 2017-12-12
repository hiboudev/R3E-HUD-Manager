using da2mvc.core.command;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.command
{
    class ResizePlaceholderCommand : ICommand
    {
        private readonly PlaceHolderResizedEventArgs args;
        private readonly PlaceHolderCollectionModel collectionModel;

        public ResizePlaceholderCommand(PlaceHolderResizedEventArgs args, PlaceHolderCollectionModel collectionModel)
        {
            this.args = args;
            this.collectionModel = collectionModel;
        }

        public void Execute()
        {
            if (args.Size < 0.1m)
                collectionModel.UpdatePlaceholderSize(args.PlaceholderName, 0.1m);
            else
                collectionModel.UpdatePlaceholderSize(args.PlaceholderName, args.Size);
        }
    }
}
