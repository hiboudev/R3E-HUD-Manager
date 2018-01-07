using da2mvc.framework.application.view;
using da2mvc.framework.collection.model;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;
using R3EHUDManager_wpf.application.view;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace R3EHUDManager_wpf.background.view
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

        private void OnRadioLayoutChanged(object sender, EventArgs e)
        {
            bool check = (bool)((RadioButton)sender).IsChecked;
            int centerScreenWidth;

            if ((RadioButton)sender == radioCrop)
            {
                if (check)
                {
                    centerScreenWidth = DrawRectangle(false);
                    backgroundInfo.Content = $"Single screen layout: 1 x [{centerScreenWidth}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio(centerScreenWidth, (int)bitmapSize.Height)})";
                }
                else
                    preview.ClearRectangle();
            }
            else if (check && (RadioButton)sender == radioSingle)
            {
                backgroundInfo.Content = $"Single screen layout: 1 x [{bitmapSize.Width}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio((int)bitmapSize.Width, (int)bitmapSize.Height)})";
            }
            else if ((RadioButton)sender == radioTriple)
            {
                if (check)
                {
                    backgroundInfo.Content = $"Triple screen layout: 3 x [{bitmapSize.Width / 3}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio((int)(bitmapSize.Width / 3), (int)bitmapSize.Height)})";
                    DrawRectangle(true);
                }
                else
                    preview.ClearRectangle();
            }
        }

        private int DrawRectangle(bool lineStyle)
        {
            int screenWidth = (int)Math.Round((decimal)bitmapSize.Width / 3); // TODO revoir tout ça en simplifiant
            int centerScreenWidth = (int)bitmapSize.Width / 3;

            if (!lineStyle)
            {
                cropRect = new Rect(screenWidth, 0, centerScreenWidth, bitmapSize.Height);
            }
            preview.DrawRectangle(lineStyle);

            return centerScreenWidth;
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

        private void OnDispose(object sender, EventArgs e)
        {
        }

        private void InitializeUI()
        {
            // Force autosize now.
            errorField.Content = "";

            okButton.Click += (sender, args) => this.DialogResult = true;
            okButton.IsEnabled = false;
            inputField.TextChanged += CheckText;

            radioSingle.Checked += OnRadioLayoutChanged;
            radioTriple.Checked += OnRadioLayoutChanged;
            radioCrop.Checked += OnRadioLayoutChanged;
        }

    }
}
