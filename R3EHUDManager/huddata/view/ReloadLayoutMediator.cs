using da2mvc.core.view;
using R3EHUDManager.savestatus.events;
using R3EHUDManager.savestatus.model;
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
            HandleEvent<SaveStatusModel, SaveStatusEventArgs>(SaveStatusModel.EVENT_STATUS_CHANGED, OnSaveStatusChanged);
        }

        private void OnSaveStatusChanged(SaveStatusEventArgs args)
        {
            if (args.Type.HasFlag(SaveType.R3E_HUD))
                View.SetSaveStatus(args.IsSaved);
        }
    }
}
