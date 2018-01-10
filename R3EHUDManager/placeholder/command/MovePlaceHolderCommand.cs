using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using da2mvc.core.command;
using R3EHUDManager.coordinates;
using System.Drawing;
using R3EHUDManager.screen.view;
using R3EHUDManager.screen.model;
using R3EHUDManager.placeholder.validator;
using R3EHUDManager.screen.view;
using System.Windows;

namespace R3EHUDManager.placeholder.command
{
    class MovePlaceholderCommand : ICommand
    {
        private readonly PlaceholderViewEventArgs args;
        private readonly PlaceHolderCollectionModel collectionModel;
        private readonly ScreenView screenView;
        private readonly ScreenModel screenModel;
        private readonly PlaceholderUserChangeValidator moveValidator;

        public MovePlaceholderCommand(PlaceholderViewEventArgs args, PlaceHolderCollectionModel collectionModel, ScreenView screenView, ScreenModel screenModel,
            PlaceholderUserChangeValidator moveValidator)
        {
            this.args = args;
            this.collectionModel = collectionModel;
            this.screenView = screenView;
            this.screenModel = screenModel;
            this.moveValidator = moveValidator;
        }

        public void Execute()
        {
            PlaceholderModel placeholder = collectionModel.Get(args.View.Model.Id);
            placeholder.Move(moveValidator.GetValidPosition(placeholder, GetR3eLocation(args.Point)));
        }

        private R3ePoint GetR3eLocation(Point location)
        {
            Size objectScreenRatio = new Size(args.View.ContentBounds.Width / screenView.ScreenArea.Width, args.View.ContentBounds.Height / screenView.ScreenArea.Height);

            R3ePoint anchorSize = new R3ePoint(
                objectScreenRatio.Width * (args.View.Model.Anchor.X + 1),
                objectScreenRatio.Height * (args.View.Model.Anchor.Y - 1));

            Point position = new Point(location.X - screenView.ScreenArea.X, location.Y - screenView.ScreenArea.Y);
            R3ePoint r3ePosition = Coordinates.ToR3e(position, screenView.ScreenArea.Size);

            r3ePosition = new R3ePoint(r3ePosition.X + anchorSize.X, r3ePosition.Y + anchorSize.Y);

            if (screenModel.Layout == ScreenLayoutType.TRIPLE)
                return new R3ePoint(r3ePosition.X * 3, r3ePosition.Y);
            else
                return r3ePosition;
        }
    }
}
