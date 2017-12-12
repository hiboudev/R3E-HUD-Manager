using da2mvc.core.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;
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
        private readonly BackgroundCollectionModel backgroundCollection;
        private readonly PlaceHolderCollectionModel placeholderCollection;

        public SelectBackgroundCommand(ContextMenuEventArgs args, ScreenModel screenModel, BackgroundCollectionModel backgroundCollection, PlaceHolderCollectionModel placeholderCollection)
        {
            this.args = args;
            this.screenModel = screenModel;
            this.backgroundCollection = backgroundCollection;
            this.placeholderCollection = placeholderCollection;
        }

        public void Execute()
        {
            ScreenLayoutType currentLayout = screenModel.Layout;
            BackgroundModel background = backgroundCollection.Get(args.ItemId);

            screenModel.SetBackground(background);

            if (currentLayout == ScreenLayoutType.TRIPLE && background.Layout == ScreenLayoutType.SINGLE)
                ScreenUtils.PromptUserIfOutsideOfCenterScreenPlaceholders(placeholderCollection);
        }
    }
}
