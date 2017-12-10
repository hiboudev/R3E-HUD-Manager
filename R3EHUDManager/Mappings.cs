using R3EHUDManager.huddata.command;
using R3EHUDManager.huddata.parser;
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
using R3EHUDManager.background.model;
using R3EHUDManager.screen.view;
using R3EHUDManager.settings.view;
using R3EHUDManager.background.view;
using R3EHUDManager.background.command;
using R3EHUDManager.database;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.command;
using R3EHUDManager.layout.view;

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
            Injector.MapType(typeof(ScreenModel), typeof(ScreenModel), true);
            Injector.MapType(typeof(Database), typeof(Database), true);
            Injector.MapType(typeof(BackgroundCollectionModel), typeof(BackgroundCollectionModel), true);
            Injector.MapType(typeof(BackgroundToolbarView), typeof(BackgroundToolbarView), true); 
            Injector.MapType(typeof(PromptNewBackgroundView), typeof(PromptNewBackgroundView));
            Injector.MapType(typeof(SettingsMenuView), typeof(SettingsMenuView), true);
            Injector.MapType(typeof(LayoutToolbarView), typeof(LayoutToolbarView), true); 
            Injector.MapType(typeof(DatabaseUpgrader), typeof(DatabaseUpgrader), true); 

            Injector.MapView(typeof(ScreenView), typeof(ScreenMediator), true);
            Injector.MapView(typeof(SelectionView), typeof(SelectionMediator), true);
            Injector.MapView(typeof(PlaceholdersListView), typeof(PlaceholdersListMediator), true);
            Injector.MapView(typeof(BackgroundView), typeof(BackgroundMediator), true);
            Injector.MapView(typeof(BackgroundMenuView), typeof(BackgroundMenuMediator), true);
            Injector.MapView(typeof(BackgroundManagerView), typeof(BackgroundManagerMediator));
            Injector.MapView(typeof(LayoutMenuView), typeof(LayoutMenuMediator));
            Injector.MapView(typeof(ZoomView), typeof(ZoomMediator));

            Injector.MapCommand(typeof(Form1), Form1.EVENT_SAVE_CLICKED, typeof(SaveHudCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_CLICKED, typeof(LoadHudDataCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_DEFAULT_CLICKED, typeof(ReloadDefaultHudDataCommand));

            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_SELECTED, typeof(SelectPlaceholderCommand));
            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_BACKGROUND_CLICKED, typeof(SelectNoneCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_ANCHOR_MOVED, typeof(MoveAnchorCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_PLACEHOLDER_RESIZED, typeof(ResizePlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_MOVE_TO_SCREEN, typeof(ChangePlaceholderScreenCommand));
            Injector.MapCommand(typeof(PlaceholdersListView), PlaceholdersListView.EVENT_PLACEHOLDER_SELECTED, typeof(SelectPlaceholderCommand));
            Injector.MapCommand(typeof(SettingsMenuView), SettingsMenuView.EVENT_OPEN_APP_INSTALL_DIRECTORY, typeof(OpenAppInstallDirectoryCommand));
            Injector.MapCommand(typeof(SettingsMenuView), SettingsMenuView.EVENT_OPEN_APP_DATA_DIRECTORY, typeof(OpenAppDataDirectoryCommand));
            Injector.MapCommand(typeof(SettingsMenuView), SettingsMenuView.EVENT_OPEN_HUD_DIRECTORY, typeof(OpenHudDirectoryCommand));
            Injector.MapCommand(typeof(BackgroundMenuView), BackgroundMenuView.EVENT_IMPORT_BACKGROUND, typeof(ImportBackgroundCommand));
            Injector.MapCommand(typeof(BackgroundMenuView), BackgroundMenuView.EVENT_ITEM_CLICKED, typeof(SelectBackgroundCommand));
            Injector.MapCommand(typeof(BackgroundManagerView), BackgroundManagerView.EVENT_DELETE_BACKGROUND, typeof(DeleteBackgroundCommand));
            Injector.MapCommand(typeof(LayoutMenuView), LayoutMenuView.EVENT_ITEM_CLICKED, typeof(ChangeScreenLayoutCommand));
            Injector.MapCommand(typeof(ZoomView), ZoomView.EVENT_ZOOM_LEVEL_CHANGED, typeof(ChangeZoomLevelCommand));
        }
    }
}
