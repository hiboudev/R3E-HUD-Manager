using da2mvc.framework.menubutton.view;
using R3EHUDManager.location.model;
using System.Drawing;

namespace R3EHUDManager.location.view
{
    class R3eDirectoryMenuView : MenuButtonView<R3eDirectoryModel>
    {
        private const byte MAX_LENGTH = 14;

        public R3eDirectoryMenuView()
        {
            Font = new Font(Font.FontFamily, 7);
            AutoSize = false;
        }

        protected override string Title => "Dir";

        protected override string FormatTitle(string selectedName)
        {
            return $"{Title}: {TroncateText(selectedName)}";
        }

        private string TroncateText(string text)
        {
            // TODO generaliser avec LayoutSourceView
            if (text.Length <= MAX_LENGTH)
                return text;

            return $"...{text.Substring(text.Length - MAX_LENGTH, MAX_LENGTH)}";
        }
    }
}
