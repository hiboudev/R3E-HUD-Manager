using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.screen.model;
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
        private readonly ScreenModel screenModel;
        private readonly BackgroundCollectionModel collectionModel;

        public SelectBackgroundCommand(ContextMenuEventArgs args, ScreenModel screenModel, BackgroundCollectionModel collectionModel)
        {
            this.args = args;
            this.screenModel = screenModel;
            this.collectionModel = collectionModel;
        }

        public void Execute()
        {
            screenModel.SetBackground(collectionModel.Get(args.ItemId));
        }
    }
}
