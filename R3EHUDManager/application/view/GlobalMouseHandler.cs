using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.application.view
{
    internal class GlobalMouseHandler : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;
        public event EventHandler MouseMoved;

        public void Enable()
        {
            Application.AddMessageFilter(this);
        }

        public void Disable()
        {
            Application.RemoveMessageFilter(this);
        }

        #region IMessageFilter Members

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEMOVE)
            {
                MouseMoved?.Invoke(this, EventArgs.Empty);
            }
            return false;
        }

        #endregion
    }
}
