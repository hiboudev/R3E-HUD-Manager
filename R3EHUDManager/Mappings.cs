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
using R3EHUDManager.location.command;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.command;
using R3EHUDManager.selection.view;

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
            Injector.MapType(typeof(SelectionModel), typeof(SelectionModel), true);

            Injector.MapView(typeof(ScreenView), typeof(ScreenMediator), true);
            Injector.MapView(typeof(SelectionView), typeof(SelectionMediator), true);

            Injector.MapCommand(typeof(Form1), Form1.EVENT_SAVE_CLICKED, typeof(SaveHudCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_CLICKED, typeof(LoadHudDataCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_DEFAULT_CLICKED, typeof(ReloadDefaultHudDataCommand));

            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_SELECTED, typeof(SelectPlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_ANCHOR_MOVED, typeof(MoveAnchorCommand));
        }
    }
}
