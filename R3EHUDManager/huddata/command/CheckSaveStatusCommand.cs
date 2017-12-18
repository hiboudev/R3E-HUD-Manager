using da2mvc.core.command;
using R3EHUDManager.huddata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.command
{
    class CheckSaveStatusCommand : ICommand
    {
        private readonly SaveStatusChecker saveStatus;

        public CheckSaveStatusCommand(SaveStatusChecker saveStatus)
        {
            this.saveStatus = saveStatus;
        }

        public void Execute()
        {
            saveStatus.StartDelayedCheck();
        }
    }
}
