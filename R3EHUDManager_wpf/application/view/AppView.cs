using da2mvc.core.injection;
using da2mvc.framework.application.view;
using R3EHUDManager_wpf.utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace R3EHUDManager_wpf.application.view
{
    public class AppView : AbstractAppView
    {
        protected override void InitializeMappings()
        {
            Mappings.Initialize();
        }
    }
}
