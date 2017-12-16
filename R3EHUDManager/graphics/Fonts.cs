using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.graphics
{
    class Fonts
    {
        public static readonly Font UNSAVED_FONT = new Font(Control.DefaultFont.FontFamily, Control.DefaultFont.Size, FontStyle.Italic);
        public static readonly Font SAVED_FONT = new Font(Control.DefaultFont.FontFamily, Control.DefaultFont.Size, FontStyle.Regular);
    }
}
