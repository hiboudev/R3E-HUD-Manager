using da2mvc.framework.model;
using R3EHUDManager.application.view;
using R3EHUDManager.profile.command;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.utils;
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
    class PromptNewProfileView : BaseModalForm
    {
        private HashSet<string> usedNames;
        private HashSet<string> usedFileNames;
        private string backgroundName;
        private Label fileNameField;
        private Label errorField;
        private Button okButton;
        private TableLayoutPanel layout;
        private TextBox nameField;
        public string ProfileName { get => nameField.Text; }

        public PromptNewProfileView(CollectionModel<ProfileModel> profileCollection, ScreenModel screen):base("Profile creation")
        {
            usedNames = new HashSet<string>();
            usedFileNames = new HashSet<string>();
            backgroundName = screen.Background.Name;

            foreach (ProfileModel profile in profileCollection.Items) {
                usedNames.Add(profile.Name);
                usedFileNames.Add(profile.fileName);
            }

            InitializeUI();
        }

        private void InitializeUI()
        {
            Size = new Size(300, 185);

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

            fileNameField = new Label()
            {
                Text = $"File name:",
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
            AddControl(fileNameField, SizeType.AutoSize);
            AddControl(errorField, SizeType.AutoSize);
            AddControl(okButton, SizeType.AutoSize);

            Controls.Add(layout);
        }

        private void CheckText(object sender, EventArgs e)
        {
            string fileName = CreateProfileCommand.ToFileName(nameField.Text);
            fileNameField.Text = $"File name: {fileName}";

            bool nameIsValid = Regex.Replace(nameField.Text, @"\s+", "").Length > 0;

            if (usedNames.Contains(nameField.Text))
            {
                errorField.Text = "This name is already used by another profile.";
                nameIsValid = false;
            }
            else if (usedFileNames.Contains(fileName))
            {
                errorField.Text = "This file name is already used by another profile.";
                nameIsValid = false;
            }
            else
                errorField.Text = "";

            okButton.Enabled = nameIsValid;
        }

        private void AddControl(Control control, SizeType sizeType)
        {
            layout.RowStyles.Add(new RowStyle(sizeType));
            layout.Controls.Add(control);
        }
    }
}
