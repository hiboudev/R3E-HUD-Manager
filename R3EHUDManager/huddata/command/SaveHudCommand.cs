using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.command;
using da2mvc.core.events;
using R3EHUDManager.huddata.model;

namespace R3EHUDManager.huddata.command
{
    class SaveHudCommand : ICommand
    {
        private readonly BaseEventArgs args;
        private readonly PlaceHolderCollectionModel placeholders;
        private readonly LocationModel location;
        private readonly LayoutIOModel layoutIO;

        public SaveHudCommand(BaseEventArgs args, PlaceHolderCollectionModel placeholders, LocationModel location, LayoutIOModel layoutIO)
        {
            this.args = args;
            this.placeholders = placeholders;
            this.location = location;
            this.layoutIO = layoutIO;
        }

        public void Execute()
        {
            layoutIO.WriteR3eLayout(placeholders.Items);
            layoutIO.DispatchSaveStatus();
        }
    }
}
