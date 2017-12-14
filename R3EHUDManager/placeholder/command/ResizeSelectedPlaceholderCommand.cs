using da2mvc.core.command;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
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

        public ResizeSelectedPlaceholderCommand(SelectionViewEventArgs args, SelectionModel selectionModel)
        {
            this.args = args;
            this.selectionModel = selectionModel;
        }

        public void Execute()
        {
            if (args.Point.X < 0.1)
                selectionModel.Selection.Resize(new R3ePoint(0.1, 0.1));
            else
                selectionModel.Selection.Resize(args.Point);
        }
    }
}
