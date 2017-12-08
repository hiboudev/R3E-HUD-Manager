using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.screen.command
{
    class ChangeTripleScreenCommand : ICommand
    {
        private readonly BooleanEventArgs args;
        private readonly ScreenModel screenModel;

        public ChangeTripleScreenCommand(BooleanEventArgs args, ScreenModel screenModel)
        {
            this.args = args;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            screenModel.SetTripleScreen(args.Value);
        }
    }
}
