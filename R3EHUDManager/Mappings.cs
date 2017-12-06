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
            Injector.MapType(typeof(SelectedBackgroundModel), typeof(SelectedBackgroundModel), true);
            Injector.MapType(typeof(ImportBackgroundView), typeof(ImportBackgroundView), true);
            Injector.MapType(typeof(Database), typeof(Database), true);
            Injector.MapType(typeof(BackgroundCollectionModel), typeof(BackgroundCollectionModel), true);
            Injector.MapType(typeof(BackgroundToolbarView), typeof(BackgroundToolbarView), true); 
            Injector.MapType(typeof(PromptBackgroundNameView), typeof(PromptBackgroundNameView), true); 

            Injector.MapView(typeof(ScreenView), typeof(ScreenMediator), true);
            Injector.MapView(typeof(SelectionView), typeof(SelectionMediator), true);
            Injector.MapView(typeof(PlaceholdersListView), typeof(PlaceholdersListMediator), true);
            Injector.MapView(typeof(SettingsMenuView), typeof(SettingsMenuMediator), true);
            Injector.MapView(typeof(BackgroundView), typeof(BackgroundMediator), true);
            Injector.MapView(typeof(BackgroundListView), typeof(BackgroundListMediator), true);

            Injector.MapCommand(typeof(Form1), Form1.EVENT_SAVE_CLICKED, typeof(SaveHudCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_CLICKED, typeof(LoadHudDataCommand));
            Injector.MapCommand(typeof(Form1), Form1.EVENT_RELOAD_DEFAULT_CLICKED, typeof(ReloadDefaultHudDataCommand));

            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(ScreenView), ScreenView.EVENT_PLACEHOLDER_SELECTED, typeof(SelectPlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_PLACEHOLDER_MOVED, typeof(MovePlaceholderCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_ANCHOR_MOVED, typeof(MoveAnchorCommand));
            Injector.MapCommand(typeof(SelectionView), SelectionView.EVENT_PLACEHOLDER_RESIZED, typeof(ResizePlaceholderCommand));
            Injector.MapCommand(typeof(PlaceholdersListView), PlaceholdersListView.EVENT_PLACEHOLDER_SELECTED, typeof(SelectPlaceholderCommand));
            Injector.MapCommand(typeof(SettingsMenuView), SettingsMenuView.EVENT_OPEN_APP_INSTALL_DIRECTORY, typeof(OpenAppInstallDirectoryCommand));
            Injector.MapCommand(typeof(SettingsMenuView), SettingsMenuView.EVENT_OPEN_APP_DATA_DIRECTORY, typeof(OpenAppDataDirectoryCommand));
            Injector.MapCommand(typeof(SettingsMenuView), SettingsMenuView.EVENT_OPEN_HUD_DIRECTORY, typeof(OpenHudDirectoryCommand));
            Injector.MapCommand(typeof(ImportBackgroundView), ImportBackgroundView.EVENT_IMPORT_BACKGROUND, typeof(ImportBackgroundCommand));
            Injector.MapCommand(typeof(BackgroundListView), BackgroundListView.EVENT_BACKGROUND_SELECTED, typeof(SelectBackgroundCommand));
        }
    }
}
