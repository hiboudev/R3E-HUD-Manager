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

namespace R3EHUDManager.huddata.command
{
    class SaveHudCommand : ICommand, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_HUD_LAYOUT_APPLIED = EventId.New();

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
            parser.Write(location.HudOptionsFile, placeholders.Items);
            DispatchEvent(new BaseEventArgs(EVENT_HUD_LAYOUT_APPLIED));
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
