using da2mvc.core.command;
using R3EHUDManager.placeholder.validator;
using R3EHUDManager.selection.events;
using R3EHUDManager.selection.model;

namespace R3EHUDManager.placeholder.command
{
    class MoveSelectedPlaceholderCommand : ICommand
    {
        private readonly SelectionViewEventArgs args;
        private readonly SelectionModel selectionModel;
        private readonly PlaceholderUserChangeValidator moveValidator;

        public MoveSelectedPlaceholderCommand(SelectionViewEventArgs args, SelectionModel selectionModel, PlaceholderUserChangeValidator moveValidator)
        {
            this.args = args;
            this.selectionModel = selectionModel;
            this.moveValidator = moveValidator;
        }

        public void Execute()
        {
            selectionModel.Selection.Move(moveValidator.GetValidPosition(selectionModel.Selection, args.Point));
        }
    }
}
