using da2mvc.core.view;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.model;

namespace R3EHUDManager.huddata.view
{
    class LayoutSourceMediator : BaseMediator<LayoutSourceView>
    {
        public LayoutSourceMediator()
        {
            HandleEvent<LayoutIOModel, LayoutSourceEventArgs>(LayoutIOModel.EVENT_SOURCE_CHANGED, OnSourceChanged);
        }

        private void OnSourceChanged(LayoutSourceEventArgs args)
        {
            View.SetSource(args.SourceType, args.SourceName);
        }
    }
}
