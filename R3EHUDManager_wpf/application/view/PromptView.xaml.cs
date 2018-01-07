using da2mvc.framework.application.view;
using R3EHUDManager.userpreferences.model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace R3EHUDManager_wpf.application.view
{
    /// <summary>
    /// Logique d'interaction pour PromptView.xaml
    /// </summary>
    public partial class PromptView : ModalWindow
    {
        private Dictionary<PreferenceType, CheckBox> checkBoxes = new Dictionary<PreferenceType, CheckBox>();

        public PromptView()
        {
            InitializeComponent();
        }

        public bool RememberChoice { get; private set; }

        public bool GetChecked(PreferenceType prefType)
        {
            return checkBoxes[prefType].IsChecked == true;
        }

        public bool HasCkeck(PreferenceType prefType)
        {
            return checkBoxes.ContainsKey(prefType);
        }

        public void Initialize(string title, string text, CheckBoxData[] checkBoxesData)
        {
            Title = title;
            label.Text = text;

            yesButton.Click += (sender, args) => SetDialogResult(true);
            noButton.Click += (sender, args) => SetDialogResult(false);

            foreach (var checkBoxData in checkBoxesData)
            {
                CheckBox checkBox = new CheckBox()
                {
                    Content = checkBoxData.Text,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66)),
                    FontSize = 10,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };
                checkBoxes.Add(checkBoxData.PrefType, checkBox);
                checkBoxesContainer.Children.Add(checkBox);
            }
        }

        private void SetDialogResult(bool result)
        {
            RememberChoice = true;
            DialogResult = result;
        }
    }

    public class CheckBoxData
    {

        public CheckBoxData(PreferenceType prefType, string text)
        {
            PrefType = prefType;
            Text = text;
        }

        public PreferenceType PrefType { get; }
        public string Text { get; }
    }
}