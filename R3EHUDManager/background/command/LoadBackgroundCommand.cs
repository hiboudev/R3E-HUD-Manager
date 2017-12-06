using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.command
{
    class LoadBackgroundCommand : ICommand
    {
        private readonly StringEventArgs args;
        private readonly BackgroundModel backgroundModel;

        public LoadBackgroundCommand(StringEventArgs args, BackgroundModel backgroundModel)
        {
            this.args = args;
            this.backgroundModel = backgroundModel;
        }

        public void Execute()
        {
            backgroundModel.LoadBackground(args.Text);
        }
    }
}
