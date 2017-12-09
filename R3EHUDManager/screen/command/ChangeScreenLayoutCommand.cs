using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.screen.command
{
    class ChangeScreenLayoutCommand : ICommand
    {
        private readonly ContextMenuEventArgs args;
        private readonly ScreenModel screenModel;

        public ChangeScreenLayoutCommand(ContextMenuEventArgs args, ScreenModel screenModel)
        {
            this.args = args;
            this.screenModel = screenModel;
        }

        public void Execute()
        {
            screenModel.SetLayout((ScreenLayoutType)args.ItemId);
        }
    }
}
