using da2mvc.framework.collection.model;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.events;
using R3EHUDManager.graphics;
using R3EHUDManager.motec.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.motec.view
{
    class MotecMenuMediator : CollectionMediator<CollectionModel<MotecModel>, MotecModel, MotecMenuView>
    {
        public MotecMenuMediator()
        {
            HandleEvent<GraphicalAssetFactory, IntEventArgs>(GraphicalAssetFactory.EVENT_MOTEC_CHANGED, OnMotecChanged);
        }

        private void OnMotecChanged(IntEventArgs args)
        {
            View.SelectMotec(args.Value);
        }
    }
}
