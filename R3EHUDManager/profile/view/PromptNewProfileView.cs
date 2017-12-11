using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.profile.view
{
    class PromptNewProfileView : Form // TODO généraliser avec les autres
    {
        private HashSet<string> usedNames;
        private string backgroundName;
        private Label errorField;
        private Button okButton;
        private TableLayoutPanel layout;
        private TextBox nameField;
        public string ProfileName { get => nameField.Text; }

        public PromptNewProfileView(ProfileCollectionModel profileCollection, ScreenModel screen)
        {
            usedNames = new HashSet<string>();
            backgroundName = screen.Background.Name;

            foreach (ProfileModel profile in profileCollection.Profiles)
                usedNames.Add(profile.Name);

            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Profile creation";
            MinimumSize = new Size(50, 50);
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(300, 170);
            Padding = new Padding(6);
            FormBorderStyle = FormBorderStyle.FixedSingle;

            layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
            };
            
            Label prompt = new Label()
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Text = "Profile name",
            };

            nameField = new TextBox()
            {
                Dock = DockStyle.Fill,
                MaxLength = 40,
            };
            nameField.TextChanged += CheckText;

            Label backgroundField = new Label()
            {
                Text = $"Background: {backgroundName}",
                AutoSize = true,
                Dock = DockStyle.Fill,
                Enabled = false,
                Margin = new Padding(Margin.Left, Margin.Top, Margin.Right, 4)
            };

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

            AddControl(prompt, SizeType.AutoSize);
            AddControl(nameField, SizeType.AutoSize);
            AddControl(backgroundField, SizeType.AutoSize);
            AddControl(errorField, SizeType.AutoSize);
            AddControl(okButton, SizeType.AutoSize);

            Controls.Add(layout);
        }

        private void CheckText(object sender, EventArgs e)
        {
            bool nameIsValid = Regex.Replace(nameField.Text, @"\s+", "").Length > 0;
            bool nameIsAvailable = !usedNames.Contains(nameField.Text);

            okButton.Enabled = nameIsValid && nameIsAvailable;

            if (!nameIsAvailable)
                errorField.Text = "This name is already used by another profile.";
            else
                errorField.Text = "";
        }

        private void AddControl(Control control, SizeType sizeType)
        {
            layout.RowStyles.Add(new RowStyle(sizeType));
            layout.Controls.Add(control);
        }
    }
}
