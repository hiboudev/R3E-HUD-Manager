﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.placeholder.view
{
    class AnchorView : Control
    {
        public AnchorView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Size = new Size(10, 10);
            BackColor = Color.Red;
        }
    }
}
