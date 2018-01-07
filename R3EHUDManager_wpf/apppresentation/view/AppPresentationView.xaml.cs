using R3EHUDManager.userpreferences.model;
using R3EHUDManager_wpf.application.view;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace R3EHUDManager_wpf.apppresentation.view
{
    /// <summary>
    /// Logique d'interaction pour AppPresentationView.xaml
    /// </summary>
    public partial class AppPresentationView : ModalWindow
    {
        private bool alreadyWatched;
        private string endButtonName;
        private int minImageIndex;
        private string[] paths;
        private int currentPathIndex;
        private BitmapImage currentImage;

        public AppPresentationView(UserPreferencesModel preferences) : base("Quick presentation")
        {
            InitializeComponent();
            InitializeUI(preferences);
            InitializeBitmaps();
            DisplayNextImage();
            //TODO quand on arrive au bout, si on ferme avec le bouton close, ça ne prend pas en compte qu'on a tout vu, mais on ne peut pas définir DIalogResult pendant la fermeture.
        }

        private void DisplayNextImage()
        {
            if (currentPathIndex == paths.Length) return;

            currentImage = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, paths[++currentPathIndex])));
            pictureBox.Source = currentImage;
        }

        private void DisplayPrevImage()
        {
            if (currentPathIndex == 0) return;

            currentImage = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, paths[--currentPathIndex])));
            pictureBox.Source = currentImage;
        }

        private void InitializeBitmaps()
        {
            paths = new string[]
            {
                @"_graphical_assets\presentation\screen01.jpg",
                @"_graphical_assets\presentation\screen02.jpg",
                @"_graphical_assets\presentation\screen03.jpg",
                @"_graphical_assets\presentation\screen04.jpg",
                @"_graphical_assets\presentation\screen05.jpg",
                @"_graphical_assets\presentation\screen06.jpg",
            };

            currentPathIndex = minImageIndex - 1;
        }

        private void InitializeUI(UserPreferencesModel preferences)
        {
            alreadyWatched = preferences.UserWatchedPresentation;
            endButtonName = alreadyWatched ? "Close" : "Thanks ! :)";
            minImageIndex = alreadyWatched ? 1 : 0;

            prevButton.IsEnabled = false;

            nextButton.Click += OnNextClicked;
            prevButton.Click += OnPrevClicked;
        }

        private void OnPrevClicked(object sender, EventArgs e)
        {
            DisplayPrevImage();
            UpdateButtons();
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            if (currentPathIndex == paths.Length - 1)
            {
                DialogResult = true;
                return;
            }

            DisplayNextImage();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            prevButton.IsEnabled = currentPathIndex > minImageIndex;

            nextButton.Content = currentPathIndex == paths.Length - 1 ? endButtonName : "Next";
        }
    }
}
