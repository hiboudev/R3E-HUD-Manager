using da2mvc.framework.application.view;
using da2mvc.framework.collection.model;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;
using R3EHUDManager.application.view;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace R3EHUDManager.background.view
{
    /// <summary>
    /// Logique d'interaction pour BackgroundImporterView.xaml
    /// </summary>
    public partial class BackgroundImporterView : ModalWindow
    {
        private Rect cropRect;
        private HashSet<string> usedNames;
        private Size bitmapSize;

        public BackgroundImporterView(CollectionModel<BackgroundModel> collectionModel)
        {
            // TODO Generic font instead of Courier New, how in WPF?
            InitializeComponent();
            usedNames = new HashSet<string>();

            foreach (BackgroundModel background in collectionModel.Items)
                usedNames.Add(background.Name);

            InitializeUI();
        }

        public Rect CropRect { get => radioCrop.IsChecked == true ? cropRect : new Rect(); }

        public ScreenLayoutType BackgroundLayout
        {
            get
            {
                if (radioTriple.IsChecked == true) return ScreenLayoutType.TRIPLE;
                return ScreenLayoutType.SINGLE;
            }
        }

        public string BackgroundName { get => inputField.Text; }

        internal void SetBitmap(BitmapSource bitmap)
        {
            bitmapSize = new Size(bitmap.PixelWidth, bitmap.PixelHeight);
            preview.SetBitmap(bitmap);

            radioSingle.IsChecked = true;
        }

        private void OnRadioButtonChanged(object sender, EventArgs e)
        {
            bool check = (bool)((RadioButton)sender).IsChecked;
            if (!check) return;

            if ((RadioButton)sender == radioCrop)
            {
                int cropSize = (int)(bitmapSize.Width / 3);
                cropRect = new Rect(cropSize, 0, cropSize, bitmapSize.Height);
                backgroundInfo.Content = $"Single screen layout: 1 x [{bitmapSize.Width / 3}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio((int)(bitmapSize.Width / 3), (int)bitmapSize.Height)})";
                preview.SetCuttingType(CuttingType.CROP_TO_SINGLE);
            }
            else if ((RadioButton)sender == radioSingle)
            {
                backgroundInfo.Content = $"Single screen layout: 1 x [{bitmapSize.Width}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio((int)bitmapSize.Width, (int)bitmapSize.Height)})";
                preview.SetCuttingType(CuttingType.NONE);
            }
            else if ((RadioButton)sender == radioTriple)
            {
                backgroundInfo.Content = $"Triple screen layout: 3 x [{bitmapSize.Width / 3}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio((int)(bitmapSize.Width / 3), (int)bitmapSize.Height)})";
                preview.SetCuttingType(CuttingType.TRIPLE_SCREEN);
            }
        }

        private void CheckText(object sender, EventArgs e)
        {
            bool nameIsValid = Regex.Replace(inputField.Text, @"\s+", "").Length > 0;
            bool nameIsAvailable = !usedNames.Contains(inputField.Text);

            okButton.IsEnabled = nameIsValid && nameIsAvailable;

            if (!nameIsAvailable)
                errorField.Content = "This name is already used by another background.";
            else
                errorField.Content = "";
        }

        private void InitializeUI()
        {
            // Force autosize now.
            errorField.Content = "";

            okButton.Click += (sender, args) => this.DialogResult = true;
            okButton.IsEnabled = false;
            inputField.TextChanged += CheckText;

            radioSingle.Checked += OnRadioButtonChanged;
            radioTriple.Checked += OnRadioButtonChanged;
            radioCrop.Checked += OnRadioButtonChanged;
        }

    }
}
