using da2mvc.core.command;
using R3EHUDManager.selection.events;
using R3EHUDManager.selection.model;

namespace R3EHUDManager.placeholder.command
{
    class MoveSelectedAnchorCommand : ICommand
    {
        private readonly SelectionViewEventArgs args;
        private readonly SelectionModel selectionModel;

        public MoveSelectedAnchorCommand(SelectionViewEventArgs args, SelectionModel selectionModel)
        {
            this.args = args;
            this.selectionModel = selectionModel;
        }

        public void Execute()
        {
            selectionModel.Selection.MoveAnchor(args.Point);
        }
    }
}
