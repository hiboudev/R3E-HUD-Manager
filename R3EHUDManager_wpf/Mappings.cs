using da2mvc.core.injection;
using da2mvc.framework.collection.model;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.command;
using R3EHUDManager.apppresentation.command;
using R3EHUDManager.background.command;
using R3EHUDManager.background.model;
using R3EHUDManager.background.view;
using R3EHUDManager.database;
using R3EHUDManager.fileexplorer.command;
using R3EHUDManager.huddata.command;
using R3EHUDManager.huddata.model;
using R3EHUDManager.huddata.parser;
using R3EHUDManager.huddata.view;
using R3EHUDManager.location.command;
using R3EHUDManager.location.finder;
using R3EHUDManager.location.model;
using R3EHUDManager.location.view;
using R3EHUDManager.placeholder.command;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.placeholder.validator;
using R3EHUDManager.placeholder.view;
using R3EHUDManager.profile.command;
using R3EHUDManager.profile.model;
using R3EHUDManager.profile.view;
using R3EHUDManager.r3esupport.command;
using R3EHUDManager.r3esupport.parser;
using R3EHUDManager.r3esupport.rule;
using R3EHUDManager.screen.command;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.view;
using R3EHUDManager.selection.command;
using R3EHUDManager.selection.model;
using R3EHUDManager.selection.view;
using R3EHUDManager.settings.view;
using R3EHUDManager.userpreferences.command;
using R3EHUDManager.userpreferences.model;
using R3EHUDManager.application.view;
using R3EHUDManager.apppresentation.view;
using R3EHUDManager.background.view;
using R3EHUDManager.huddata.view;
using R3EHUDManager.placeholder.view;
using R3EHUDManager.profile.view;
using R3EHUDManager.screen.view;
using R3EHUDManager.selection.view;
using R3EHUDManager.settings.view;

namespace R3EHUDManager
{
    class Mappings
    {
        // For WPF design mode.
        private static bool initialized;

        public static void Initialize(MainWindow mainWindow)
        {
            if (initialized) return;

            Injector.MapInstance(mainWindow);
            Initialize();
        }

        public static void Initialize()
        {
            if (initialized) return;
            initialized = true;

            Injector.MapType<SupportRuleValidator>(true);
            Injector.MapType<ScreenModel>(true);
            Injector.MapType<SelectionModel>(true);
            Injector.MapType<PlaceholderModel>();
            //Injector.MapType<PlaceholderView>();
            Injector.MapType<LocationModel>(true);
            Injector.MapType<Database>(true);
            Injector.MapType<DatabaseUpgrader>(true);
            Injector.MapType<UserPreferencesModel>(true);
            Injector.MapType<PlaceHolderCollectionModel>(true);
            Injector.MapType<PlaceholderBlackListModel>(true);
            Injector.MapType<R3eHomeDirectoryFinder>(true);
            Injector.MapType<CollectionModel<R3eDirectoryModel>>(true);
            Injector.MapType<CollectionModel<ProfileModel>>(true);
            Injector.MapType<CollectionModel<BackgroundModel>>(true);
            Injector.MapType<SelectedR3eDirectoryModel>(true);
            Injector.MapType<SelectedProfileModel>(true);
            Injector.MapType<LayoutIOModel>(true);
            Injector.MapType<HudOptionsParser>(true);
            Injector.MapType<ScreenViewMouseInteraction>(true);
            Injector.MapType<PlaceholderUserChangeValidator>(true);
            Injector.MapType<BackgroundImporterView>();
            Injector.MapType<ProfileCreationView>();
            Injector.MapType<SettingsMenuView>(true);
            Injector.MapType<SettingsView>();
            Injector.MapType<PlaceholderBlacklistView>();
            Injector.MapType<AppPresentationView>();
            Injector.MapType<SupportRuleParser>(true);
            Injector.MapType<SaveStatusChecker>(true);
            Injector.MapType<PromptView>();

            Injector.MapView<ScreenView, ScreenMediator>(true);
            Injector.MapView<PlaceholderView, PlaceholderMediator>();
            Injector.MapView<BackgroundManagerView, CollectionMediator<CollectionModel<BackgroundModel>, BackgroundModel, BackgroundManagerView>>();
            Injector.MapView<BackgroundMenuView, BackgroundMenuMediator>(true);
            Injector.MapView<ProfileMenuView, ProfileMenuMediator>(true);
            Injector.MapView<ProfileManagerView, CollectionMediator<CollectionModel<ProfileModel>, ProfileModel, ProfileManagerView>>();
            Injector.MapView<SelectionView, SelectionMediator>(true);
            Injector.MapView<LayoutLoadSaveView, LayoutLoadSaveMediator>(true);
            Injector.MapView<R3eDirectoryMenuView, R3eDirectoryMenuMediator>(true);
            Injector.MapView<LayoutSourceView, LayoutSourceMediator>(true);
            Injector.MapView<PlaceholderListView, PlaceholdersListMediator>(true);
            Injector.MapView<ZoomView, ZoomMediator>(true);
            Injector.MapView<ScreenScrollerView, ScreenScrollerMediator>(true);

            Injector.MapCommand<ScreenViewMouseInteraction, MovePlaceholderCommand>(ScreenViewMouseInteraction.EVENT_REQUEST_MOVE);
            Injector.MapCommand<ScreenViewMouseInteraction, SelectPlaceholderCommand>(ScreenViewMouseInteraction.EVENT_REQUEST_SELECTION);
            Injector.MapCommand<ScreenViewMouseInteraction, SelectNoneCommand>(ScreenViewMouseInteraction.EVENT_REQUEST_DESELECTION);

            Injector.MapCommand<SelectionView, MoveSelectedPlaceholderCommand>(SelectionView.EVENT_PLACEHOLDER_MOVED);
            Injector.MapCommand<SelectionView, ResizeSelectedPlaceholderCommand>(SelectionView.EVENT_PLACEHOLDER_RESIZED);
            Injector.MapCommand<SelectionView, MoveSelectedAnchorCommand>(SelectionView.EVENT_ANCHOR_MOVED);
            Injector.MapCommand<SelectionView, ChangePlaceholderScreenCommand>(SelectionView.EVENT_MOVE_TO_SCREEN);
            Injector.MapCommand<BackgroundMenuView, ImportBackgroundCommand>(BackgroundMenuView.EVENT_IMPORT_BACKGROUND);
            Injector.MapCommand<BackgroundMenuView, SelectBackgroundCommand>(BackgroundMenuView.EVENT_ITEM_CLICKED);
            Injector.MapCommand<BackgroundManagerView, DeleteBackgroundCommand>(BackgroundManagerView.EVENT_DELETE_BACKGROUND);
            Injector.MapCommand<ProfileMenuView, CreateProfileCommand>(ProfileMenuView.EVENT_CREATE_NEW_PROFILE);
            Injector.MapCommand<ProfileMenuView, SelectProfileCommand>(ProfileMenuView.EVENT_ITEM_CLICKED);
            Injector.MapCommand<LayoutLoadSaveView, SaveHudCommand>(LayoutLoadSaveView.EVENT_SAVE_CLICKED);
            Injector.MapCommand<LayoutLoadSaveView, LoadHudDataCommand>(LayoutLoadSaveView.EVENT_RELOAD_CLICKED);
            Injector.MapCommand<LayoutLoadSaveView, ReloadDefaultHudDataCommand>(LayoutLoadSaveView.EVENT_RELOAD_DEFAULT_CLICKED);
            Injector.MapCommand<R3eDirectoryMenuView, SelectR3eDirectoryCommand>(R3eDirectoryMenuView.EVENT_ITEM_CLICKED);

            Injector.MapCommand<SettingsMenuView, OpenAppInstallDirectoryCommand>(SettingsMenuView.EVENT_OPEN_APP_INSTALL_DIRECTORY);
            Injector.MapCommand<SettingsMenuView, OpenAppDataDirectoryCommand>(SettingsMenuView.EVENT_OPEN_APP_DATA_DIRECTORY);
            Injector.MapCommand<SettingsMenuView, OpenHudDirectoryCommand>(SettingsMenuView.EVENT_OPEN_HUD_DIRECTORY);

            Injector.MapCommand<SettingsView, SaveOutsidePlaceholdersPrefCommand>(SettingsView.EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED);
            Injector.MapCommand<SettingsView, SavePromptPrefsCommand>(SettingsView.EVENT_PROMPT_PREF_CHANGED);
            Injector.MapCommand<SettingsView, SaveCulturePrefCommand>(SettingsView.EVENT_CULTURE_PREF_CHANGED);

            Injector.MapCommand<PlaceholderBlacklistView, UpdateFiltersCommand>(PlaceholderBlacklistView.EVENT_FILTERS_CHANGED);
            Injector.MapCommand<SettingsMenuView, ShowPresentationCommand>(SettingsMenuView.EVENT_SHOW_PRESENTATION);

            Injector.MapCommand<PlaceholderListView, SelectPlaceholderCommand>(PlaceholderListView.EVENT_PLACEHOLDER_SELECTED);
            Injector.MapCommand<PlaceholderListView, ApplyLayoutFixCommand>(PlaceholderListView.EVENT_REQUEST_LAYOUT_FIX);
            Injector.MapCommand<PlaceholderListView, SelectNoneCommand>(PlaceholderListView.EVENT_PLACEHOLDER_UNSELECTED);

            Injector.MapCommand<PlaceHolderCollectionModel, ValidatePlaceholderCollectionCommand>(PlaceHolderCollectionModel.EVENT_ITEMS_ADDED);
            Injector.MapCommand<PlaceholderView, ApplyLayoutFixCommand>(PlaceholderView.EVENT_REQUEST_LAYOUT_FIX);

            Injector.MapCommand<ZoomView, ChangeZoomLevelCommand>(ZoomView.EVENT_ZOOM_LEVEL_CHANGED);


            Injector.MapCommand<PlaceholderModel, CheckSaveStatusCommand>(PlaceholderModel.EVENT_UPDATED);
            Injector.MapCommand<ScreenModel, CheckSaveStatusCommand>(ScreenModel.EVENT_BACKGROUND_CHANGED);

            Injector.MapCommand<ProfileMenuView, SaveProfileCommand>(ProfileMenuView.EVENT_SAVE_PROFILE);
            Injector.MapCommand<ProfileManagerView, DeleteProfileCommand>(ProfileManagerView.EVENT_DELETE_PROFILE);

            Injector.MapCommand<LayoutIOModel, PromptUnsavedChangesCommand>(LayoutIOModel.EVENT_UNSAVED_CHANGES);
            Injector.MapCommand<MainWindow, ApplicationExitCommand>(MainWindow.EVENT_APP_EXIT);
        }
    }
}
