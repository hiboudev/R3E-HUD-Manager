using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.background.events;
using da2mvc.core.view;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.events;

namespace R3EHUDManager.background.view
{
    class BackgroundMediator : BaseMediator
    {
        public BackgroundMediator()
        {
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
        }

        private void OnBackgroundChanged(BaseEventArgs args)
        {
            ((BackgroundView)View).SetBackground(((ScreenModelEventArgs)args).ScreenModel);
        }
    }
}
