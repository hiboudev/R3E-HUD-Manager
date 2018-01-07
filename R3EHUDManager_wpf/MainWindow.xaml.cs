using da2mvc.core.injection;
using R3EHUDManager.application.command;
using R3EHUDManager.selection.view;
using R3EHUDManager_wpf.screen.view;
using System;
using System.Diagnostics;
using System.Windows;

namespace R3EHUDManager_wpf
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Mappings.Initialize();

            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            Injector.ExecuteCommand<StartApplicationCommand>();
        }
    }
}
