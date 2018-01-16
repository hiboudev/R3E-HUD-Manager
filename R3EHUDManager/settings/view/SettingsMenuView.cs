using da2mvc.core.events;
using da2mvc.core.injection;
using da2mvc.framework.menubutton.view;
using da2mvc.framework.utils;
using R3EHUDManager.graphics;
using R3EHUDManager.huddata.view;
using R3EHUDManager.settings.view;
using R3EHUDManager.utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace R3EHUDManager.settings.view
{
    class SettingsMenuView : SettingsMenuButtonView
    {
        public static readonly int EVENT_OPEN_APP_DATA_DIRECTORY = EventId.New();
        public static readonly int EVENT_OPEN_APP_INSTALL_DIRECTORY = EventId.New();
        public static readonly int EVENT_OPEN_HUD_DIRECTORY = EventId.New();
        public static readonly int EVENT_SHOW_PRESENTATION = EventId.New();
        private readonly GraphicalAssetFactory assetsFactory;

        public SettingsMenuView(GraphicalAssetFactory assetsFactory)
        {
            this.assetsFactory = assetsFactory;
            InitializeUI();
        }

        protected override string Title => "";

        private void InitializeUI()
        {
            // Note : Si on appelle SetSelectedItem le Content va être remplacé.
            ContentTemplate = new ButtonContentTemplate();
            DrawArrow = false;

            if (!WpfUtils.IsInDesignMode())
                Content = assetsFactory.GetPreferencesIcon();

            Width = Height = 20;
        }

        protected override List<MenuItem> GetBuiltInItems()
        {
            var openDataDirItem = new MenuItem()
            {
                Header = "Open application data directory",
            };
            var openInstallDirItem = new MenuItem()
            {
                Header = "Open application install directory"
            };
            var openHudDirItem = new MenuItem()
            {
                Header = "Open HUD directory"
            };

            var openFilteredPlaceholders = new MenuItem()
            {
                Header = "Manage filtered placeholders..."
            };
            var showPresentation = new MenuItem()
            {
                Header = "Quick presentation"
            };
            var openSettings = new MenuItem()
            {
                Header = "Settings..."
            };

            openHudDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_HUD_DIRECTORY));
            openDataDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_DATA_DIRECTORY));
            openInstallDirItem.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_OPEN_APP_INSTALL_DIRECTORY));
            openFilteredPlaceholders.Click += OpenFiltersManager;
            showPresentation.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_SHOW_PRESENTATION));
            openSettings.Click += OpenSettings;

            return new List<MenuItem>(new MenuItem[] {
                openHudDirItem,
                openDataDirItem,
                openInstallDirItem,
                new MenuItemSeparator(),
                showPresentation,
                new MenuItemSeparator(),
                openFilteredPlaceholders,
                openSettings
            });
        }

        private void OpenSettings(object sender, EventArgs e)
        {
            var dialog = Injector.GetInstance<SettingsView>();

            if (dialog.ShowDialog() == true)
            {
            }
        }

        private void OpenFiltersManager(object sender, EventArgs e)
        {
            var dialog = Injector.GetInstance<PlaceholderBlacklistView>();

            if (dialog.ShowDialog() == true)
            {
            }
        }
    }

    class ButtonContentTemplate : DataTemplate
    {
        public ButtonContentTemplate()
        {
            var image = new FrameworkElementFactory(typeof(Image));
            image.SetValue(Image.SourceProperty, new Binding());
            image.SetValue(Image.StretchProperty, Stretch.Uniform);

            VisualTree = image;
        }
    }
}
