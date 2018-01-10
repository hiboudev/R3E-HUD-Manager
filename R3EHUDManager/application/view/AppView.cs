using da2mvc.core.injection;
using da2mvc.framework.application.view;
using R3EHUDManager.utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace R3EHUDManager.application.view
{
    public class AppView : AbstractAppView
    {
        protected override void InitializeMappings()
        {
            Mappings.Initialize();
        }
    }
}
