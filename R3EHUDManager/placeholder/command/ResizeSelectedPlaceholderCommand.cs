using da2mvc.core.command;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.placeholder.validator;
using R3EHUDManager.selection.events;
using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
