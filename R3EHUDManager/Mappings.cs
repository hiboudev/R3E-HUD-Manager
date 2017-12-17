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
using R3EHUDManager.placeholder.validator;
using R3EHUDManager.r3esupport.parser;
using R3EHUDManager.r3esupport.rule;
using R3EHUDManager.r3esupport.command;
using R3EHUDManager.savestatus.model;
using R3EHUDManager.savestatus.command;
using R3EHUDManager.huddata.view;
using R3EHUDManager.layout.model;
using R3EHUDManager.layout.view;
using R3EHUDManager.huddata.model;
using R3EHUDManager.apppresentation.view;
using R3EHUDManager.userpreferences.model;
using R3EHUDManager.userpreferences.command;

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
            Injector.MapType<PlaceholderUserChangeValidator>(true);
            Injector.MapType<SupportRuleParser>(true);
            Injector.MapType<SupportRuleValidator>(true);
            Injector.MapType<SaveStatusModel>(true);
            Injector.MapType<LayoutSourceModel>(true);
            Injector.MapType<PlaceholderBlackListModel>(true);
            Injector.MapType<PlaceholderBlackListView>();
            Injector.MapType<AppPresentationView>();
            Injector.MapType<UserPreferencesModel>(true);
            Injector.MapType<PromptOutsidePlaceholderView>();
            Injector.MapType<SettingsView>();
            
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
            Injector.MapView<ReloadLayoutView, ReloadLayoutMediator>(true);
            Injector.MapView<LayoutSourceView, LayoutSourceMediator>(true);

            Injector.MapCommand<ReloadLayoutView, SaveHudCommand>(ReloadLayoutView.EVENT_SAVE_CLICKED);
            Injector.MapCommand<ReloadLayoutView, LoadHudDataCommand>(ReloadLayoutView.EVENT_RELOAD_CLICKED);
            Injector.MapCommand<ReloadLayoutView, ReloadDefaultHudDataCommand>(ReloadLayoutView.EVENT_RELOAD_DEFAULT_CLICKED);
            Injector.MapCommand<PlaceholderView, SelectPlaceholderCommand>(PlaceholderView.EVENT_REQUEST_SELECTION);
            Injector.MapCommand<PlaceholderView, MovePlaceholderCommand>(PlaceholderView.EVENT_REQUEST_MOVE);
            Injector.MapCommand<PlaceholderView, ApplyLayoutFixCommand>(PlaceholderView.EVENT_REQUEST_LAYOUT_FIX);
            Injector.MapCommand<ScreenView, SelectNoneCommand>(ScreenView.EVENT_BACKGROUND_CLICKED);
            Injector.MapCommand<SelectionView, MoveSelectedPlaceholderCommand>(SelectionView.EVENT_PLACEHOLDER_MOVED);
            Injector.MapCommand<SelectionView, MoveSelectedAnchorCommand>(SelectionView.EVENT_ANCHOR_MOVED);
            Injector.MapCommand<SelectionView, ResizeSelectedPlaceholderCommand>(SelectionView.EVENT_PLACEHOLDER_RESIZED);
            Injector.MapCommand<SelectionView, ChangePlaceholderScreenCommand>(SelectionView.EVENT_MOVE_TO_SCREEN);
            Injector.MapCommand<PlaceholdersListView, SelectPlaceholderCommand>(PlaceholdersListView.EVENT_PLACEHOLDER_SELECTED);
            Injector.MapCommand<PlaceholdersListView, ApplyLayoutFixCommand>(PlaceholdersListView.EVENT_REQUEST_LAYOUT_FIX);
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
            Injector.MapCommand<PlaceholderModel, ValidatePlaceholderCommand>(PlaceholderModel.EVENT_UPDATED);
            Injector.MapCommand<PlaceHolderCollectionModel, ValidatePlaceholderCommand>(PlaceHolderCollectionModel.EVENT_ITEMS_ADDED);
            Injector.MapCommand<PlaceholderBlackListView, UpdateFiltersCommand>(PlaceholderBlackListView.EVENT_FILTERS_CHANGED);
            Injector.MapCommand<PromptOutsidePlaceholderView, SaveOutsidePlaceholdersPrefCommand>(PromptOutsidePlaceholderView.EVENT_REMEMBER_CHOICE);
            Injector.MapCommand<SettingsView, SaveOutsidePlaceholdersPrefCommand>(SettingsView.EVENT_OUTSIDE_PLACEHOLDERS_PREF_CHANGED);

            // Save status
            Injector.MapCommand<SaveProfileCommand, UpdateSaveStatusCommand>(SaveProfileCommand.EVENT_PROFILE_CHANGES_SAVED);
            Injector.MapCommand<SaveHudCommand, UpdateSaveStatusCommand>(SaveHudCommand.EVENT_HUD_LAYOUT_APPLIED);
            Injector.MapCommand<LoadHudDataCommand, UpdateSaveStatusCommand>(LoadHudDataCommand.EVENT_HUD_LAYOUT_LOADED);
            Injector.MapCommand<ReloadDefaultHudDataCommand, UpdateSaveStatusCommand>(ReloadDefaultHudDataCommand.EVENT_DEFAULT_HUD_LAYOUT_LOADED);
            Injector.MapCommand<PlaceholderModel, UpdateSaveStatusCommand>(PlaceholderModel.EVENT_UPDATED);
            Injector.MapCommand<CreateProfileCommand, UpdateSaveStatusCommand>(CreateProfileCommand.EVENT_PROFILE_CREATED);
            Injector.MapCommand<ScreenModel, UpdateSaveStatusCommand>(ScreenModel.EVENT_BACKGROUND_CHANGED);
            Injector.MapCommand<SelectedProfileModel, UpdateSaveStatusCommand>(SelectedProfileModel.EVENT_SELECTION_CHANGED);
            Injector.MapCommand<SelectedProfileModel, UpdateSaveStatusCommand>(SelectedProfileModel.EVENT_SELECTION_CLEARED);
        }
    }
}
