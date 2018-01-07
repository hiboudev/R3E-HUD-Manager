using da2mvc.core.events;
using R3EHUDManager.huddata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.events
{
    class UnsavedChangesEventArgs : BaseEventArgs
    {
        public bool IsLoadingCancelled { get; private set; }
        public UnsavedChangeType ChangeType { get; }
        public string SourceName { get; }

        public UnsavedChangesEventArgs(int eventId, UnsavedChangeType changeType, string sourceName) : base(eventId)
        {
            ChangeType = changeType;
            SourceName = sourceName;
        }

        public void CancelLoading()
        {
            IsLoadingCancelled = true;
        }
    }
}
