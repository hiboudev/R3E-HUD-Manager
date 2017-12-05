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
using R3EHUDManager.contextmenu.view;
using R3EHUDManager.fileexplorer.command;

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
            Injector.MapType(typeof(BackgroundModel), typeof(BackgroundModel), true);

            Injector.MapView(typeof(ScreenView), typeof(ScreenMediator), true);
            Injector.MapView(typeof(SelectionView), typeof(SelectionMediator), true);
            Injector.MapView(typeof(PlaceholdersListView), typeof(PlaceholdersListMediator), true);
            Injector.MapView(typeof(PreferencesMenuView), typeof(PreferencesMenuMediator), true);

            Injector.MapCommand(typeof(Form1), Form1.EVENT_SAVE_CLICKED, typeof(SaveHudCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_CLICKED, typeof(LoadHudDataCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_DEFAULT_CLICKED, typeof(ReloadDefaultHudDataCommand));

            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_SELECTED, typeof(SelectPlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_ANCHOR_MOVED, typeof(MoveAnchorCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_PLACEHOLDER_RESIZED, typeof(ResizePlaceholderCommand));
            Injector.MapCommand(typeof(PlaceholdersListView), PlaceholdersListView.EVENT_PLACEHOLDER_SELECTED, typeof(SelectPlaceholderCommand));
            Injector.MapCommand(typeof(PreferencesMenuView), PreferencesMenuView.EVENT_OPEN_APP_INSTALL_DIRECTORY, typeof(OpenAppInstallDirectoryCommand));
            Injector.MapCommand(typeof(PreferencesMenuView), PreferencesMenuView.EVENT_OPEN_APP_DATA_DIRECTORY, typeof(OpenAppDataDirectoryCommand));
            Injector.MapCommand(typeof(PreferencesMenuView), PreferencesMenuView.EVENT_OPEN_HUD_DIRECTORY, typeof(OpenHudDirectoryCommand));
        }
    }
}
