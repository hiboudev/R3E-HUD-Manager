using da2mvc.framework.collection.model;
using R3EHUDManager.application.view;
using R3EHUDManager.background.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class PromptNewBackgroundView : BaseModalForm
    {
        private TextBox inputField;
        private HashSet<string> usedNames;
        private Label errorField;
        private Button okButton;
        private BackgroundPreviewView preview;
        private FlowLayoutPanel stepperPanel;
        private NumericUpDown stepperLeft;
        private NumericUpDown stepperRight;
        private Rectangle cropRect;
        private TableLayoutPanel layout;
        private Size bitmapSize;
        private Label radioLabel;
        private RadioButton radioSingle;
        private RadioButton radioTriple;
        private RadioButton radioCrop;

        public Rectangle CropRect { get => radioCrop.Checked ? cropRect : new Rectangle(); }

        public ScreenLayoutType BackgroundLayout {
            get
            {
                if (radioTriple.Checked) return ScreenLayoutType.TRIPLE;
                return ScreenLayoutType.SINGLE;
            }
        }

        public string BackgroundName { get => inputField.Text; }

        public PromptNewBackgroundView(CollectionModel<BackgroundModel> collectionModel):base("Import background")
        {
            usedNames = new HashSet<string>();

            foreach (BackgroundModel background in collectionModel.Items)
                usedNames.Add(background.Name);

            InitializeUI();
            Disposed += OnDispose;
        }

        internal void SetBitmap(Bitmap bitmap)
        {
            bitmapSize = new Size((int)bitmap.PhysicalDimension.Width, (int)bitmap.PhysicalDimension.Height);

            int screenWidth = (int)Math.Round((decimal)bitmapSize.Width / 3);
            stepperLeft.Value = stepperRight.Value = screenWidth;
            stepperLeft.Maximum = stepperRight.Maximum = bitmapSize.Width / 2 - 10;

            preview.SetBitmap(bitmap);

            radioSingle.Checked = true;
        }

        private void OnStepperValueChanged(object sender, EventArgs e)
        {
            DrawRectangle(false);
        }

        private void OnRadioLayoutChanged(object sender, EventArgs e)
        {
            bool check = ((RadioButton)sender).Checked;
            int centerScreenWidth;

            if ((RadioButton)sender == radioCrop)
            {
                // Cause we can't hit enter in NumericUpDown if AcceptButton is defined.
                //AcceptButton = check ? null : okButton;

                //stepperPanel.Visible = check;

                if (check)
                {
                    centerScreenWidth = DrawRectangle(false);
                    radioLabel.Text = $"Single screen layout: 1 x [{centerScreenWidth}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio(centerScreenWidth, bitmapSize.Height)})";
                }
                else
                    preview.ClearRectangle();
            }
            else if (check && (RadioButton)sender == radioSingle)
            {
                radioLabel.Text = $"Single screen layout: 1 x [{bitmapSize.Width}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio(bitmapSize)})";
            }
            else if ((RadioButton)sender == radioTriple)
            {
                if(check)
                    radioLabel.Text = $"Triple screen layout: 3 x [{bitmapSize.Width / 3}x{bitmapSize.Height}] ({ScreenUtils.GetFormattedAspectRatio(bitmapSize.Width / 3, bitmapSize.Height)})";

                if (check)
                    DrawRectangle(true);
                else
                    preview.ClearRectangle();
            }
        }

        private int DrawRectangle(bool lineStyle)
        {
            int centerScreenWidth = bitmapSize.Width - (int)stepperLeft.Value - (int)stepperRight.Value;

            if (!lineStyle)
            {
                cropRect = new Rectangle((int)stepperLeft.Value, 0, centerScreenWidth, bitmapSize.Height);
            }
            preview.DrawRectangle((int)stepperLeft.Value, centerScreenWidth, lineStyle);

            return centerScreenWidth;
        }

        private void CheckText(object sender, EventArgs e)
        {
            bool nameIsValid = Regex.Replace(inputField.Text, @"\s+", "").Length > 0;
            bool nameIsAvailable = !usedNames.Contains(inputField.Text);

            okButton.Enabled = nameIsValid && nameIsAvailable;

            if (!nameIsAvailable)
                errorField.Text = "This name is already used by another background.";
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

        private void InitializeUI()
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            Size = new Size(500, 450);

            layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
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
            };
            okButton.Click += (sender, args) => this.DialogResult = DialogResult.OK;
            okButton.Enabled = false;
            AcceptButton = okButton;

            preview = new BackgroundPreviewView
            {
                Dock = DockStyle.Fill
            };

            Panel radioPanel = new FlowLayoutPanel() { FlowDirection = FlowDirection.TopDown, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Fill };

            radioLabel = new Label() { AutoSize = true, Margin = new Padding(Margin.Left, Margin.Top, Margin.Right, 5), Font = new Font(FontFamily.GenericMonospace, 9) };

            radioSingle = NewRadioButton("Single screen");
            radioTriple = NewRadioButton("Triple screen");
            radioCrop = NewRadioButton("Crop triple -> single screen");

            radioSingle.CheckedChanged += OnRadioLayoutChanged;
            radioTriple.CheckedChanged += OnRadioLayoutChanged;
            radioCrop.CheckedChanged += OnRadioLayoutChanged;

            radioPanel.Controls.AddRange(new Control[] { radioLabel, radioSingle, radioTriple, radioCrop });

            stepperPanel = new FlowLayoutPanel()
            {
                AutoSize = true,
                WrapContents = false,
                Visible = false,
            };

            stepperPanel.Controls.Add(NewResolutionLabel("Left resolution"));
            stepperPanel.Controls.Add(stepperLeft = NewStepper());
            stepperPanel.Controls.Add(NewResolutionLabel("Right resolution"));
            stepperPanel.Controls.Add(stepperRight = NewStepper());


            AddControl(prompt, SizeType.AutoSize);
            AddControl(inputField, SizeType.AutoSize);
            AddControl(errorField, SizeType.AutoSize);
            AddControl(preview, SizeType.Percent, 100);
            AddControl(radioPanel, SizeType.AutoSize);
            AddControl(stepperPanel, SizeType.AutoSize);
            AddControl(okButton, SizeType.AutoSize); // TODO Généraliser ça

            Controls.Add(layout);

            ActiveControl = inputField;
        }

        private static RadioButton NewRadioButton(string text)
        {
            return new RadioButton() {
                AutoSize = true,
                Text = text,
                Margin = new Padding(0),
            };
        }

        private NumericUpDown NewStepper()
        {
            var stepper = new NumericUpDown()
            {
                Minimum = 10,
                Maximum = decimal.MaxValue,
            };
            stepper.ValueChanged += OnStepperValueChanged;
            stepper.GotFocus += OnStepperFocused;
            return stepper;
        }

        private static Label NewResolutionLabel(string text)
        {
            return new Label()
            {
                Text = text,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                TextAlign = ContentAlignment.MiddleLeft,
            };
        }

        private void AddControl(Control control, SizeType sizeType)
        {
            layout.RowStyles.Add(new RowStyle(sizeType));
            layout.Controls.Add(control);
        }

        private void AddControl(Control control, SizeType sizeType, int height)
        {
            layout.RowStyles.Add(new RowStyle(sizeType, height));
            layout.Controls.Add(control);
        }

        private void OnStepperFocused(object sender, EventArgs e)
        {
            var stepper = (NumericUpDown)sender;
            stepper.Select(0, stepper.Text.Length);
        }
    }
}
