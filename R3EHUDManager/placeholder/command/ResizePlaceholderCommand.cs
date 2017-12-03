﻿using da2mvc.command;
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
            collectionModel.UpdatePlaceholderSize(args.PlaceholderName, args.Size);
        }
    }
}
