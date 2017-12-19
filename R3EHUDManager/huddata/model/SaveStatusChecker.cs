using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.huddata.model
{
    class SaveStatusChecker
    {
        private readonly LayoutIOModel layoutIO;
        private Timer timer;

        public SaveStatusChecker(LayoutIOModel layoutIO)
        {
            this.layoutIO = layoutIO;
            timer = new Timer()
            {
                Interval = 1000,
            };
            timer.Tick += TimerElapsed;
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            timer.Stop();
            layoutIO.DispatchSaveStatus();
        }

        public void StartDelayedCheck()
        {
            timer.Stop();
            timer.Start();
        }
    }
}
