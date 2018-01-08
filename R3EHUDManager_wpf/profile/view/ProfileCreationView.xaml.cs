using da2mvc.framework.application.view;
using da2mvc.framework.collection.model;
using R3EHUDManager.profile.command;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using R3EHUDManager_wpf.application.view;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace R3EHUDManager_wpf.profile.view
{
    /// <summary>
    /// Logique d'interaction pour ProfileCreationView.xaml
    /// </summary>
    public partial class ProfileCreationView : ModalWindow
    {

        private HashSet<string> usedNames;
        private HashSet<string> usedFileNames;
        private string backgroundName;
        public string ProfileName { get => nameField.Text; }

        public ProfileCreationView(CollectionModel<ProfileModel> profileCollection, ScreenModel screen)
        {
            usedNames = new HashSet<string>();
            usedFileNames = new HashSet<string>();
            backgroundName = screen.Background.Name;

            foreach (ProfileModel profile in profileCollection.Items)
            {
                usedNames.Add(profile.Name);
                usedFileNames.Add(profile.FileName);
            }

            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Autosize this fields immediatly.
            fileNameField.Text = errorField.Text = "";

            backgroundField.Text = $"Background: {backgroundName}";
            nameField.TextChanged += CheckText;

            okButton.Click += (sender, args) => DialogResult = true;
            okButton.IsEnabled = false;
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

            okButton.IsEnabled = nameIsValid;
        }
    }
}
