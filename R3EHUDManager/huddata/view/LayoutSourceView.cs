using System.Drawing;
using System.Windows.Forms;
using R3EHUDManager.huddata.model;

namespace R3EHUDManager.huddata.view
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
            Margin = new Padding();
        }

        internal void SetSource(LayoutSourceType sourceType, string name)
        {
            string headerText = "Layout source";
            string sourceText = "";

            switch (sourceType)
            {
                case LayoutSourceType.R3E:
                    sourceText = "R3E";
                    break;

                case LayoutSourceType.BACKUP:
                    sourceText = "Backup";
                    break;

                case LayoutSourceType.PROFILE:
                    sourceText = "Profile";
                    break;
            }

            Text = $"{headerText}: {sourceText} \"{TroncateText(name)}\"";
        }

        private string TroncateText(string text)
        {
            if (text.Length <= MAX_LENGTH)
                return text;

            return $"...{text.Substring(text.Length - MAX_LENGTH, MAX_LENGTH)}";
        }
    }
}
