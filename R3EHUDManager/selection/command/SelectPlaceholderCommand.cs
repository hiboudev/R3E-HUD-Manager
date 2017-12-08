using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.selection.command
{
    class SelectPlaceholderCommand : ICommand
    {
        private readonly StringEventArgs args;
        private readonly PlaceHolderCollectionModel collectionModel;
        private readonly SelectionModel selectionModel;

        public SelectPlaceholderCommand(StringEventArgs args, PlaceHolderCollectionModel collectionModel, SelectionModel selectionModel)
        {
            this.args = args;
            this.collectionModel = collectionModel;
            this.selectionModel = selectionModel;
        }

        public void Execute()
        {
            PlaceholderModel model = collectionModel.Get(args.Value);
            selectionModel.Select(model);
        }
    }
}
