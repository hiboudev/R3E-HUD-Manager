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
    class LayoutMenuMediator : BaseMediator<LayoutMenuView>
    {
        public LayoutMenuMediator()
        {
            RegisterEventListener<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_TRIPLE_SCREEN_CHANGED, OnChange);
            RegisterEventListener<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_BACKGROUND_CHANGED, OnChange);
        }

        private void OnChange(ScreenModelEventArgs args)
        {
            View.SetSelectedItem((int)(args.ScreenModel.Layout));
        }
    }
}
