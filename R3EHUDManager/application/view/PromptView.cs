using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.application.view
{
    class PromptView : BaseModalForm
    {
        private Dictionary<PreferenceType, CheckBox> checkBoxes = new Dictionary<PreferenceType, CheckBox>();


        public PromptView() : base("Prompt")
        {
        }

        public bool GetChecked(PreferenceType prefType)
        {
            return checkBoxes[prefType].Checked;
        }

        public void Initialize(string title, string text, CheckBoxData[] checkBoxesData)
        {
            MinimumSize = new Size(50, 20);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Text = title;
            //FormBorderStyle = FormBorderStyle.Sizable;

            Panel layout = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                //BackColor = Color.Green,
                WrapContents = false,
                Padding = new Padding(8),
        };

            Label label = new Label()
            {
                Text = text,
                //BackColor = Color.Blue,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0, 0, 0, 8)
            };

            Button yesButton = new Button()
            {
                Text = "Yes",
                Anchor = AnchorStyles.Right,
            };
            yesButton.Click += (sender, args) => DialogResult = DialogResult.Yes;

            Button noButton = new Button()
            {
                Text = "No",
                Anchor = AnchorStyles.Right,
            };
            noButton.Click += (sender, args) => DialogResult = DialogResult.No;

            Panel buttonLayout = new TableLayoutPanel()
            {
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                //BackColor = Color.Red,
                Anchor = AnchorStyles.Right,
            };

            buttonLayout.Controls.Add(yesButton);
            buttonLayout.Controls.Add(noButton);

            layout.Controls.Add(label);

            foreach (var checkBoxData in checkBoxesData)
            {
                CheckBox checkBox = new CheckBox()
                {
                    AutoSize = true,
                    Text = checkBoxData.Text,
                };
                checkBoxes.Add(checkBoxData.PrefType, checkBox);
                layout.Controls.Add(checkBox);
            }

            layout.Controls.Add(buttonLayout);

            Controls.Add(layout);

            noButton.Select();
        }
    }

    class CheckBoxData
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
