using da2mvc.core.view;
using R3EHUDManager.layout.events;
using R3EHUDManager.layout.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.layout.view
{
    class LayoutSourceMediator : BaseMediator<LayoutSourceView>
    {
        public LayoutSourceMediator()
        {
            HandleEvent<LayoutSourceModel, LayoutSourceEventArgs>(LayoutSourceModel.EVENT_SOURCE_CHANGED, OnSourceChanged);
        }

        private void OnSourceChanged(LayoutSourceEventArgs args)
        {
            View.SetSource(args.Model.SourceType, args.Model.Name);
        }
    }
}
