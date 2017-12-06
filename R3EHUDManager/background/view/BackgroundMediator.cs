using MVC_Framework.view;
using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using R3EHUDManager.background.events;

namespace R3EHUDManager.background.view
{
    class BackgroundMediator : BaseMediator
    {
        public BackgroundMediator()
        {
            RegisterEventListener(typeof(SelectedBackgroundModel), SelectedBackgroundModel.EVENT_BACKGROUND_CHANGED, OnBackgroundChanged);
        }

        private void OnBackgroundChanged(BaseEventArgs args)
        {
            ((BackgroundView)View).SetBackground(((SelectedBackgroundEventArgs)args).Model);
        }
    }
}
