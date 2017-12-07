using da2mvc.command;
using R3EHUDManager.selection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.selection.command
{
    class SelectNoneCommand : ICommand
    {
        private readonly SelectionModel selectionModel;

        public SelectNoneCommand(SelectionModel selectionModel)
        {
            this.selectionModel = selectionModel;
        }

        public void Execute()
        {
            selectionModel.Unselect();
        }
    }
}
