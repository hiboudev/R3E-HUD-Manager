using R3EHUDManager.data.command;
using R3EHUDManager.data.parser;
using R3EHUDManager.location;
using R3EHUDManager.location.finder;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.command;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.placeholder.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.injection;

namespace R3EHUDManager
{
    class Mappings
    {
        public static void InitializeMappings(Form1 form)
        {
            Injector.MapInstance(typeof(Form1), form);

            Injector.MapType(typeof(LocationModel), typeof(LocationModel), true);
            Injector.MapType(typeof(R3eHomeDirectoryFinder), typeof(R3eHomeDirectoryFinder), true); 
            Injector.MapType(typeof(HudOptionsParser), typeof(HudOptionsParser), true);
            Injector.MapType(typeof(PlaceHolderCollectionModel), typeof(PlaceHolderCollectionModel), true);

            Injector.MapView(typeof(ScreenView), typeof(ScreenMediator), true);

            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_POSITION_CHANGED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_SAVE_CLICKED, typeof(SaveHudCommand));
        }
    }
}
