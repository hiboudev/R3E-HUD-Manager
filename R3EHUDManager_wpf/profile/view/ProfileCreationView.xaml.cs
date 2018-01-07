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
            fileNameField.Content = errorField.Content = "";

            backgroundField.Content = $"Background: {backgroundName}";
            nameField.TextChanged += CheckText;

            okButton.Click += (sender, args) => DialogResult = true;
            okButton.IsEnabled = false;
        }

        private void CheckText(object sender, EventArgs e)
        {
            string fileName = CreateProfileCommand.ToFileName(nameField.Text);
            // WTF this "access key" concept that removes underscores by default... Need to start text with "_" to use the Label like it should work.
            // TODO Ban Label from the app and use only TextBlock?
            fileNameField.Content = $"_File name: {fileName}";

            bool nameIsValid = Regex.Replace(nameField.Text, @"\s+", "").Length > 0;

            if (usedNames.Contains(nameField.Text))
            {
                errorField.Content = "This name is already used by another profile.";
                nameIsValid = false;
            }
            else if (usedFileNames.Contains(fileName))
            {
                errorField.Content = "This file name is already used by another profile.";
                nameIsValid = false;
            }
            else
                errorField.Content = "";

            okButton.IsEnabled = nameIsValid;
        }
    }
}
