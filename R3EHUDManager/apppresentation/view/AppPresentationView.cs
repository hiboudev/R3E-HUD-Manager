using R3EHUDManager.application.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.apppresentation.view
{
    class AppPresentationView : BaseModalForm
    {
        private string[] paths;
        private int currentPathIndex;
        private Bitmap currentImage;
        private PictureBox pictureBox;
        private Button nextButton;
        private Button prevButton;

        public AppPresentationView() : base("Quick presentation")
        {
            FormClosed += DisposeForm; // TODO generaliser aux autres form

            InitializeBitmaps();
            InitializeUI();
            DisplayNextImage();
        }

        private void DisplayNextImage()
        {
            if (currentImage != null) currentImage.Dispose();
            if (currentPathIndex == paths.Length) return;

            currentImage = new Bitmap(paths[++currentPathIndex]);
            pictureBox.Image = currentImage;
        }

        private void DisplayPrevImage()
        {
            if (currentImage != null) currentImage.Dispose();
            if (currentPathIndex == 0) return;

            currentImage = new Bitmap(paths[--currentPathIndex]);
            pictureBox.Image = currentImage;
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

            currentPathIndex = -1;
        }

        private void InitializeUI()
        {
            Padding = new Padding();
            AutoSize = true;

            Panel layout = new FlowLayoutPanel()
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Margin = new Padding(),
            };

            pictureBox = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.Normal,
                Size = new Size(630, 385),
                Margin = new Padding(),
            };

            nextButton = new Button()
            {
                Text = "Next",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };

            prevButton = new Button()
            {
                Text = "Previous",
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
            };
            prevButton.Enabled = false;

            TableLayoutPanel buttonsPanel = new TableLayoutPanel()
            {
                AutoSize = true,
                //Dock = DockStyle.Bottom,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                RowCount = 1,
                ColumnCount = 2,
                Margin = new Padding(),
            };

            nextButton.Click += OnNextClicked;
            prevButton.Click += OnPrevClicked;

            buttonsPanel.Controls.Add(prevButton);
            buttonsPanel.Controls.Add(nextButton);

            layout.Controls.Add(pictureBox);
            layout.Controls.Add(buttonsPanel);

            Controls.Add(layout);

            nextButton.Select();
        }

        private void OnPrevClicked(object sender, EventArgs e)
        {
            DisplayPrevImage();
            UpdateButtons();
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            if(nextButton.Text == "Thanks! :)")
            {
                DialogResult = DialogResult.OK;
                return;
            }
            
            DisplayNextImage();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            prevButton.Enabled = currentPathIndex > 0;

            nextButton.Text = currentPathIndex == paths.Length - 1 ? "Thanks! :)" : "Next";
        }

        private void OnCloseClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void DisposeForm(object sender, FormClosedEventArgs e)
        {
            if (currentImage != null) currentImage.Dispose();
        }
    }
}
