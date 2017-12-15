﻿using da2mvc.core.command;
using da2mvc.core.injection;
using da2mvc.framework.collection.model;
using da2mvc.framework.menubutton.events;
using R3EHUDManager.background.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.command;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;
using System.Diagnostics;

namespace R3EHUDManager.background.command
{
    class SelectBackgroundCommand : ICommand
    {
        private readonly MenuButtonEventArgs args;
        private readonly ScreenModel screenModel;
        private readonly CollectionModel<BackgroundModel> backgroundCollection;
        private readonly PlaceHolderCollectionModel placeholderCollection;

        public SelectBackgroundCommand(MenuButtonEventArgs args, ScreenModel screenModel, CollectionModel<BackgroundModel> backgroundCollection, PlaceHolderCollectionModel placeholderCollection)
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

            Injector.ExecuteCommand<ValidateCollectionCommand>();
        }
    }
}
