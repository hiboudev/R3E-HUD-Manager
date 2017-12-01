using R3EHUDManager.data.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.command;
using da2mvc.events;

namespace R3EHUDManager.data.command
{
    class SaveHudCommand : ICommand
    {
        private readonly BaseEventArgs args;
        private readonly PlaceHolderCollectionModel placeholders;
        private readonly HudOptionsParser parser;
        private readonly LocationModel location;

        public SaveHudCommand(BaseEventArgs args, PlaceHolderCollectionModel placeholders, HudOptionsParser parser, LocationModel location)
        {
            this.args = args;
            this.placeholders = placeholders;
            this.parser = parser;
            this.location = location;
        }

        public void Execute()
        {
            parser.Write(location.HudOptionsPath, placeholders.Placeholders);
        }
    }
}
