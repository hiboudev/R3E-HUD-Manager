using da2mvc.command;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.command
{
    class ChangePlaceholderScreenCommand : ICommand
    {
        private readonly PlaceholderScreenEventArgs args;
        private readonly PlaceHolderCollectionModel collectionModel;
        private readonly ScreenModel screenModel;

        public ChangePlaceholderScreenCommand(PlaceholderScreenEventArgs args, PlaceHolderCollectionModel collectionModel, ScreenModel screenModel)
        {
            this.args = args;
            this.collectionModel = collectionModel;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            if (screenModel.Layout != ScreenLayoutType.TRIPLE) return;

            PlaceholderModel placeholder = collectionModel.Get(args.PlaceholderName);
            ScreenPositionType currentScreen = ScreenUtils.GetScreen(placeholder);

            if (currentScreen == args.ScreenType) return;

            R3ePoint screenOffset = ScreenUtils.ToScreenOffset(placeholder, args.ScreenType);
            R3ePoint newPosition = new R3ePoint(placeholder.Position.X + screenOffset.X, placeholder.Position.Y + screenOffset.Y);
            collectionModel.UpdatePlaceholderPosition(placeholder.Name, newPosition);
        }
    }
}
