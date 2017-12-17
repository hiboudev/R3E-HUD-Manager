using da2mvc.core.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.application.events
{
    class ApplicationExitEventArgs : BaseEventArgs
    {
        public ApplicationExitEventArgs(int eventId, FormClosingEventArgs formArgs) : base(eventId)
        {
            FormArgs = formArgs;
        }

        public FormClosingEventArgs FormArgs { get; }
    }
}
