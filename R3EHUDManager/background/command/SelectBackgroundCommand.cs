using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.contextmenu.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.command
{
    class SelectBackgroundCommand : ICommand
    {
        private readonly ContextMenuEventArgs args;
        private readonly SelectedBackgroundModel backgroundSelection;
        private readonly BackgroundCollectionModel collectionModel;

        public SelectBackgroundCommand(ContextMenuEventArgs args, SelectedBackgroundModel backgroundSelection, BackgroundCollectionModel collectionModel)
        {
            this.args = args;
            this.backgroundSelection = backgroundSelection;
            this.collectionModel = collectionModel;
        }

        public void Execute()
        {
            backgroundSelection.SelectBackground(collectionModel.Get(args.ItemId));
        }
    }
}
