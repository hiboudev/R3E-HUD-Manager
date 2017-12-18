using da2mvc.core.view;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.view
{
    class ReloadLayoutMediator : BaseMediator<ReloadLayoutView>
    {
        public ReloadLayoutMediator()
        {
            HandleEvent<LayoutIOModel, SaveStatusEventArgs>(LayoutIOModel.EVENT_SAVE_STATUS, OnSaveStatusChanged);
        }

        private void OnSaveStatusChanged(SaveStatusEventArgs args)
        {
            View.SetSaveStatus(args.SavedTypes.HasFlag(UnsavedChangeType.R3E));
        }
    }
}
