using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class PromptNewBackgroundView : Form
    {
        private TextBox inputField;
        private HashSet<string> usedNames;
        private Label errorField;
        private Button okButton;
        private BabckgroundPreviewView preview;
        private int bitmapWidth;
        private int bitmapHeight;
        private CheckBox tripleScreenCheck;
        private FlowLayoutPanel stepperPanel;
        private NumericUpDown stepperLeft;
        private NumericUpDown stepperRight;
        private Rectangle cropRect;
        public Rectangle CropRect { get => tripleScreenCheck.Checked ? cropRect : new Rectangle(); }

        public string BackgroundName { get => inputField.Text; }

        public PromptNewBackgroundView(BackgroundCollectionModel collectionModel)
        {
            usedNames = new HashSet<string>();

            foreach (BackgroundModel background in collectionModel.Backgrounds)
                usedNames.Add(background.Name);

            InitializeUI();
            Disposed += OnDispose;
        }

        private void InitializeUI()
        {
            Text = "Enter a background name";

            StartPosition = FormStartPosition.CenterParent;
            //FormBorderStyle = FormBorderStyle.FixedDialog;

            Size = new Size(500, 450);
            //AutoSize = true;
            //AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(6);

            TableLayoutPanel layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                //RowCount = 5,
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
                MaxLength = 40,
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
                //Dock = DockStyle.Fill
            };
            okButton.Click += (sender, args) => this.DialogResult = DialogResult.OK;
            okButton.Enabled = false;

            AcceptButton = okButton;

            preview = new BabckgroundPreviewView();
            preview.Dock = DockStyle.Fill;


            tripleScreenCheck = new CheckBox()
            {
                Text = "Triple screen image",
                AutoSize = true,
            };

            stepperPanel = new FlowLayoutPanel()
            {
                AutoSize = true,
                WrapContents = false,
                Enabled = false,
            };

            tripleScreenCheck.CheckedChanged += OnTripleScreenCheckChanged;

            stepperLeft = new NumericUpDown()
            {
                Minimum = 10,
                Maximum = decimal.MaxValue,
            };
            stepperRight = new NumericUpDown()
            {
                Minimum = 10,
                Maximum = decimal.MaxValue,
            };
            stepperLeft.ValueChanged += OnStepperValueChanged;
            stepperRight.ValueChanged += OnStepperValueChanged;

            stepperLeft.GotFocus += OnStepperFocused;
            stepperRight.GotFocus += OnStepperFocused;

            Label labelLeft = new Label()
            {
                Text = "Left",
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            Label labelRight = new Label()
            {
                Text = "Right",
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            stepperPanel.Controls.Add(labelLeft);
            stepperPanel.Controls.Add(stepperLeft);
            stepperPanel.Controls.Add(labelRight);
            stepperPanel.Controls.Add(stepperRight);



            Controls.Add(layout);

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(prompt);
            layout.Controls.Add(inputField);
            layout.Controls.Add(errorField);
            layout.Controls.Add(preview);
            layout.Controls.Add(tripleScreenCheck);
            layout.Controls.Add(stepperPanel);
            layout.Controls.Add(okButton);

            ActiveControl = inputField;
        }

        private void OnStepperFocused(object sender, EventArgs e)
        {
            var stepper = (NumericUpDown)sender;
            stepper.Select(0, stepper.Text.Length);
        }

        private void OnTripleScreenCheckChanged(object sender, EventArgs e)
        {
            bool check = ((CheckBox)sender).Checked;

            // Cause we can't hit enter in NumericUpDown.
            AcceptButton = check ? null : okButton;
            stepperPanel.Enabled = check;
            if (check)
                DrawRectangle();
            else
                preview.ClearRectangle();
        }

        private void DrawRectangle()
        {
            int centerScreenWidth = bitmapWidth - (int)stepperLeft.Value - (int)stepperRight.Value;
            cropRect = new Rectangle((int)stepperLeft.Value, 0, centerScreenWidth, bitmapHeight);
            preview.DrawRectangle((int)stepperLeft.Value, centerScreenWidth);
        }

        private void OnStepperValueChanged(object sender, EventArgs e)
        {
            DrawRectangle();
        }

        internal void SetBitmap(Bitmap bitmap)
        {
            bitmapWidth = (int)bitmap.PhysicalDimension.Width;
            bitmapHeight = (int)bitmap.PhysicalDimension.Height;

            int screenWidth = (int)Math.Round((decimal)bitmapWidth / 3);
            stepperLeft.Value = stepperRight.Value = screenWidth;
            stepperLeft.Maximum = stepperRight.Maximum = bitmapWidth / 2 - 10;

            preview.SetBitmap(bitmap);
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

        private void OnDispose(object sender, EventArgs e)
        {
            if (preview != null)
            {
                preview.Dispose();
                preview = null;
            }
        }
    }
}
