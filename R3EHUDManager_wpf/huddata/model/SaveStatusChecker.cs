using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace R3EHUDManager.huddata.model
{
    class SaveStatusChecker
    {
        private readonly LayoutIOModel layoutIO;
        private readonly DispatcherTimer timer;

        public SaveStatusChecker(LayoutIOModel layoutIO)
        {
            this.layoutIO = layoutIO;

            timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 1, 0),
                IsEnabled = true,
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
