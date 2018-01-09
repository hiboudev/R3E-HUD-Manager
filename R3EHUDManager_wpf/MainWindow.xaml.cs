using da2mvc.core.events;
using da2mvc.core.injection;
using R3EHUDManager.application.command;
using R3EHUDManager.application.events;
using R3EHUDManager.selection.view;
using R3EHUDManager.screen.view;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace R3EHUDManager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_APP_EXIT = EventId.New();

        public MainWindow()
        {
            Mappings.Initialize(this);

            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            Injector.ExecuteCommand<StartApplicationCommand>();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DispatchEvent(new ApplicationExitEventArgs(EVENT_APP_EXIT, e));
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
