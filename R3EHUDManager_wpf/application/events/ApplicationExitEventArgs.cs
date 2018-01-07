using da2mvc.core.events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace R3EHUDManager.application.events
{
    class ApplicationExitEventArgs : BaseEventArgs
    {
        public ApplicationExitEventArgs(int eventId, CancelEventArgs formArgs) : base(eventId)
        {
            FormArgs = formArgs;
        }

        public CancelEventArgs FormArgs { get; }
    }
}
