using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.graphics;
using R3EHUDManager.layout.model;

namespace R3EHUDManager.layout.view
{
    class LayoutSourceView : Label
    {
        private const byte MAX_LENGTH = 60;

        public LayoutSourceView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            AutoSize = true;
            TextAlign = ContentAlignment.BottomRight;
            Font = new Font(Font.FontFamily, 7);
            ForeColor = Color.FromArgb(0x96a2a8);
            Dock = DockStyle.Fill;
            //BackColor = Color.Red;
            Margin = new Padding();
        }

        internal void SetSource(LayoutSourceType sourceType, string name)
        {
            string headerText = "Layout from";
            string sourceText = "";

            switch (sourceType)
            {
                case LayoutSourceType.R3E:
                    sourceText = "R3E";
                    break;

                case LayoutSourceType.BACKUP:
                    sourceText = "backup";
                    break;

                case LayoutSourceType.PROFILE:
                    sourceText = "profile";
                    break;
            }

            Text = $"{headerText} {sourceText}: {TroncateText(name)}";
        }

        private string TroncateText(string text)
        {
            if (text.Length <= MAX_LENGTH)
                return text;

            return $"(...){text.Substring(text.Length - MAX_LENGTH, MAX_LENGTH)}";
        }
    }
}
