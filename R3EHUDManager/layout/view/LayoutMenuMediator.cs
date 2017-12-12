using da2mvc.core.view;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.screen.events;

namespace R3EHUDManager.layout.view
{
    class LayoutMenuMediator : BaseMediator
    {
        public LayoutMenuMediator()
        {
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnChange);
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_BACKGROUND_CHANGED, OnChange);
        }

        private void OnChange(BaseEventArgs args)
        {
            ((LayoutMenuView)View).SetSelectedItem((int)((ScreenModelEventArgs)args).ScreenModel.Layout);
        }
    }
}
