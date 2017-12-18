using da2mvc.core.command;
using R3EHUDManager.placeholder.validator;
using R3EHUDManager.selection.events;
using R3EHUDManager.selection.model;

namespace R3EHUDManager.placeholder.command
{
    class ResizeSelectedPlaceholderCommand : ICommand
    {
        private readonly SelectionViewEventArgs args;
        private readonly SelectionModel selectionModel;
        private readonly PlaceholderUserChangeValidator validator;

        public ResizeSelectedPlaceholderCommand(SelectionViewEventArgs args, SelectionModel selectionModel, PlaceholderUserChangeValidator validator)
        {
            this.args = args;
            this.selectionModel = selectionModel;
            this.validator = validator;
        }

        public void Execute()
        {
            selectionModel.Selection.Resize(validator.GetValidSize(selectionModel.Selection, args.Point.X));
        }
    }
}
