using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_MVC.command;

namespace R3EHUDManager.placeholder.command
{
    class MovePlaceholderCommand : ICommand
    {
        private readonly PlaceHolderMovedEventArgs args;
        private readonly PlaceHolderCollectionModel collectionModel;

        public MovePlaceholderCommand(PlaceHolderMovedEventArgs args, PlaceHolderCollectionModel collectionModel)
        {
            this.args = args;
            this.collectionModel = collectionModel;
        }

        public void Execute()
        {
            collectionModel.UpdatePlaceholder(args.PlaceholderName, args.Position);
        }
    }
}
