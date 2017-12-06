using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class PromptBackgroundNameView : Form
    {
        private TextBox inputField;
        private HashSet<string> usedNames;
        private Label errorField;
        private Button okButton;

        public string BackgroundName { get => inputField.Text; }

        public PromptBackgroundNameView(BackgroundCollectionModel collectionModel)
        {
            usedNames = new HashSet<string>();

            foreach (BackgroundModel background in collectionModel.Backgrounds)
                usedNames.Add(background.Name);

            InitializeUI();
        }


        private void InitializeUI()
        {
            Text = "Enter a background name";

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            Size = new Size(300, 140);
            Padding = new Padding(6);

            Panel layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
            };

            Label prompt = new Label()
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Text = "Background name",
            };

            inputField = new TextBox()
            {
                Dock = DockStyle.Fill,
            };
            inputField.TextChanged += CheckText;

            errorField = new Label()
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                ForeColor = Color.OrangeRed,
            };

            okButton = new Button()
            {
                Text = "OK",
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            okButton.Click += (sender, args) => this.DialogResult = DialogResult.OK;
            okButton.Enabled = false;
            AcceptButton = okButton;

            Controls.Add(layout);

            layout.Controls.Add(prompt);
            layout.Controls.Add(inputField);
            layout.Controls.Add(errorField);
            layout.Controls.Add(okButton);

            ActiveControl = inputField;
        }

        private void CheckText(object sender, EventArgs e)
        {
            bool nameIsValid = Regex.Replace(inputField.Text, @"\s+", "").Length > 0;
            bool nameIsAvailable = !usedNames.Contains(inputField.Text);

            okButton.Enabled = nameIsValid && nameIsAvailable;

            if (!nameIsAvailable)
                errorField.Text = "This name is already used for another background.";
            else
                errorField.Text = "";
        }
    }
}
