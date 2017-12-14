using R3EHUDManager.huddata.command;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.location.finder;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.command;
using R3EHUDManager.placeholder.model;
using da2mvc.core.injection;
using R3EHUDManager.location.command;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.command;
using R3EHUDManager.selection.view;
using R3EHUDManager.fileexplorer.command;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.view;
using R3EHUDManager.settings.view;
using R3EHUDManager.background.view;
using R3EHUDManager.background.command;
using R3EHUDManager.database;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.command;
using R3EHUDManager.profile.model;
using R3EHUDManager.profile.view;
using R3EHUDManager.profile.command;
using R3EHUDManager.location.view;
using da2mvc.framework.collection.view;
using da2mvc.framework.collection.model;
using R3EHUDManager.placeholder.view;

namespace R3EHUDManager
{
    class Mappings
    {
        public static void InitializeMappings(Form1 form)
        {
            Injector.MapInstance<Form1>(form);

            Injector.MapType<LocationModel>(true);
            Injector.MapType<R3eHomeDirectoryFinder>(true);
            Injector.MapType<HudOptionsParser>(true);
            Injector.MapType<PlaceHolderCollectionModel>(true);
            Injector.MapType<SelectionModel>(true);
            Injector.MapType<ScreenModel>(true);
            Injector.MapType<Database>(true);
            Injector.MapType<CollectionModel<BackgroundModel>>(true);
            Injector.MapType<PromptNewBackgroundView>();
            Injector.MapType<SettingsMenuView>(true);
            Injector.MapType<DatabaseUpgrader>(true);
            Injector.MapType<CollectionModel<ProfileModel>>(true);
            Injector.MapType<SelectedProfileModel>(true);
            Injector.MapType<PromptNewProfileView>();
            Injector.MapType<CollectionModel<R3eDirectoryModel>>(true);
            Injector.MapType<SelectedR3eDirectoryModel>(true);
            Injector.MapType<PlaceholderModel>();


            Injector.MapView<ScreenView, ScreenMediator>(true);
            Injector.MapView<SelectionView, SelectionMediator>(true);
            Injector.MapView<PlaceholdersListView, PlaceholdersListMediator>(true);
            Injector.MapView<BackgroundView, BackgroundMediator>(true);
            Injector.MapView<BackgroundMenuView, BackgroundMenuMediator>(true);
            Injector.MapView<BackgroundManagerView, CollectionMediator<CollectionModel<BackgroundModel>, BackgroundModel, BackgroundManagerView>>();
            Injector.MapView<ZoomView, ZoomMediator>(true);
            Injector.MapView<ProfileMenuView, ProfileMenuMediator>(true);
            Injector.MapView<ProfileManagerView, CollectionMediator<CollectionModel<ProfileModel>, ProfileModel, ProfileManagerView>>();
            Injector.MapView<R3eDirectoryMenuView, R3eDirectoryMenuMediator>(true);
            Injector.MapView<PlaceholderView, PlaceholderMediator>();

            Injector.MapCommand<Form1, SaveHudCommand>(Form1.EVENT_SAVE_CLICKED);
            Injector.MapCommand<Form1, LoadHudDataCommand>(Form1.EVENT_RELOAD_CLICKED);
            Injector.MapCommand<Form1, ReloadDefaultHudDataCommand>(Form1.EVENT_RELOAD_DEFAULT_CLICKED);
            Injector.MapCommand<PlaceholderView, SelectPlaceholderCommand>(PlaceholderView.EVENT_REQUEST_SELECTION);
            Injector.MapCommand<PlaceholderView, MovePlaceholderCommand>(PlaceholderView.EVENT_REQUEST_MOVE);
            Injector.MapCommand<ScreenView, SelectNoneCommand>(ScreenView.EVENT_BACKGROUND_CLICKED);
            Injector.MapCommand<SelectionView, MoveSelectedPlaceholderCommand>(SelectionView.EVENT_PLACEHOLDER_MOVED);
            Injector.MapCommand<SelectionView, MoveSelectedAnchorCommand>(SelectionView.EVENT_ANCHOR_MOVED);
            Injector.MapCommand<SelectionView, ResizeSelectedPlaceholderCommand>(SelectionView.EVENT_PLACEHOLDER_RESIZED);
            Injector.MapCommand<SelectionView, ChangePlaceholderScreenCommand>(SelectionView.EVENT_MOVE_TO_SCREEN);
            Injector.MapCommand<PlaceholdersListView, SelectPlaceholderCommand>(PlaceholdersListView.EVENT_PLACEHOLDER_SELECTED);
            Injector.MapCommand<SettingsMenuView, OpenAppInstallDirectoryCommand>(SettingsMenuView.EVENT_OPEN_APP_INSTALL_DIRECTORY);
            Injector.MapCommand<SettingsMenuView, OpenAppDataDirectoryCommand>(SettingsMenuView.EVENT_OPEN_APP_DATA_DIRECTORY);
            Injector.MapCommand<SettingsMenuView, OpenHudDirectoryCommand>(SettingsMenuView.EVENT_OPEN_HUD_DIRECTORY);
            Injector.MapCommand<BackgroundMenuView, ImportBackgroundCommand>(BackgroundMenuView.EVENT_IMPORT_BACKGROUND);
            Injector.MapCommand<BackgroundMenuView, SelectBackgroundCommand>(BackgroundMenuView.EVENT_ITEM_CLICKED);
            Injector.MapCommand<BackgroundManagerView, DeleteBackgroundCommand>(BackgroundManagerView.EVENT_DELETE_BACKGROUND);
            Injector.MapCommand<ZoomView, ChangeZoomLevelCommand>(ZoomView.EVENT_ZOOM_LEVEL_CHANGED);
            Injector.MapCommand<ProfileMenuView, SelectProfileCommand>(ProfileMenuView.EVENT_ITEM_CLICKED);
            Injector.MapCommand<ProfileMenuView, CreateProfileCommand>(ProfileMenuView.EVENT_CREATE_NEW_PROFILE);
            Injector.MapCommand<ProfileMenuView, SaveProfileCommand>(ProfileMenuView.EVENT_SAVE_PROFILE);
            Injector.MapCommand<ProfileManagerView, DeleteProfileCommand>(ProfileManagerView.EVENT_DELETE_PROFILE);
            Injector.MapCommand<R3eDirectoryMenuView, SelectR3eDirectoryCommand>(R3eDirectoryMenuView.EVENT_ITEM_CLICKED);
        }
    }
}
