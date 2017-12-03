using da2mvc.command;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.command
{
    class MoveAnchorCommand : ICommand
    {
        private readonly AnchorMovedEventArgs args;
        private readonly PlaceHolderCollectionModel collectionModel;

        public MoveAnchorCommand(AnchorMovedEventArgs args, PlaceHolderCollectionModel collectionModel)
        {
            this.args = args;
            this.collectionModel = collectionModel;
        }

        public void Execute()
        {
            collectionModel.UpdatePlaceholderAnchor(args.PlaceholderName, args.Anchor);
        }
    }
}
