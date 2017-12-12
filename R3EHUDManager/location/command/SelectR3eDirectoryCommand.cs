﻿using da2mvc.core.command;
using da2mvc.framework.model;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.command
{
    class SelectR3eDirectoryCommand : ICommand
    {
        private readonly ContextMenuEventArgs args;
        private readonly CollectionModel<R3eDirectoryModel> directoryCollection;
        private readonly SelectedR3eDirectoryModel directorySelection;
        private readonly LocationModel location;

        public SelectR3eDirectoryCommand(ContextMenuEventArgs args, CollectionModel<R3eDirectoryModel> directoryCollection, SelectedR3eDirectoryModel directorySelection, LocationModel location)
        {
            this.args = args;
            this.directoryCollection = directoryCollection;
            this.directorySelection = directorySelection;
            this.location = location;
        }

        public void Execute()
        {
            R3eDirectoryModel directory = directoryCollection.Get(args.ItemId);
            location.R3eHomeBaseDirectory = directory.Path;
            directorySelection.SelectDirectory(directory);
        }
    }
}
