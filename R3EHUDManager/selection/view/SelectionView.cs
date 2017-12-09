using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using da2mvc.events;
using System.Diagnostics;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.coordinates;
using System.Threading;
using System.Globalization;
using System.Drawing;
using R3EHUDManager.application.events;

namespace R3EHUDManager.selection.view
{
    class SelectionView : FlowLayoutPanel, IEventDispatcher
    {
        private NumericUpDown stepperX;
        private NumericUpDown stepperY;
        private NumericUpDown stepperSize;
        private Label nameField;

        public event EventHandler MvcEventHandler;
        public const string EVENT_PLACEHOLDER_MOVED = "placeholderMoved";
        public const string EVENT_ANCHOR_MOVED = "anchorMoved";
        public const string EVENT_PLACEHOLDER_RESIZED = "placeholderResized";
        private ComboBox anchorPresets;
        private ComboBox positionPresets;
        private CheckBox linkAnchorsCheck;

        public PlaceholderModel Selection { get; private set; }

        private bool holdStepperEvent = false;

        public SelectionView()
        {
            InitializeComponent();
            InitializeUI();
        }

        internal void UpdateData()
        {
            nameField.Text = Selection.Name;

            holdStepperEvent = true;
            stepperX.Value = (decimal)Selection.Position.X;
            stepperY.Value = (decimal)Selection.Position.Y;
            if ((decimal)Selection.Size.X > stepperSize.Minimum)
                stepperSize.Value = (decimal)Selection.Size.X;
            holdStepperEvent = false;

            SelectAnchorPreset();
            SelectPositionPreset();
        }

        internal void SetSelected(PlaceholderModel placeholder)
        {
            Selection = placeholder;
            //TODO replace double by decimal in project?
            UpdateData();

            Enabled = true;
        }

        internal void Unselect()
        {
            Selection = null;
            nameField.Text = "";
            holdStepperEvent = true;
            stepperX.Value = 0;
            stepperY.Value = 0;
            stepperSize.Value = 1;
            holdStepperEvent = false;

            anchorPresets.SelectedItem = null;
            positionPresets.SelectedItem = null;

            Enabled = false;
        }

        private void InitializeUI()
        {
            Enabled = false;

            linkAnchorsCheck.Checked = true;

            stepperX.DecimalPlaces = stepperY.DecimalPlaces = stepperSize.DecimalPlaces = 3;
            stepperX.Minimum = stepperY.Minimum = stepperSize.Minimum = decimal.MinValue;
            stepperX.Maximum = stepperY.Maximum = stepperSize.Maximum = decimal.MaxValue;

            stepperSize.Minimum = (decimal)0.1;

            stepperX.ValueChanged += OnValueChanged;
            stepperY.ValueChanged += OnValueChanged;
            stepperSize.ValueChanged += OnValueChanged;

            stepperX.Increment = stepperY.Increment = stepperSize.Increment = (decimal)0.001;

            stepperX.GotFocus += SelectStepperText;
            stepperY.GotFocus += SelectStepperText;
            stepperSize.GotFocus += SelectStepperText;

            foreach (string presetName in R3ePointPreset.presets.Keys)
            {
                anchorPresets.Items.Add(presetName);
                positionPresets.Items.Add(presetName);
            }
            anchorPresets.SelectionChangeCommitted += OnAnchorPresetSelected;
            positionPresets.SelectionChangeCommitted += OnPositionPresetSelected;
        }

        private void SelectStepperText(object sender, EventArgs e)
        {
            //stepperX.Select();
            var stepper = (NumericUpDown)sender;
            stepper.Select(0, stepper.Text.Length);
        }

        private void OnAnchorPresetSelected(object sender, EventArgs e)
        {
            string name = anchorPresets.SelectedItem.ToString();
            R3ePoint anchor = R3ePointPreset.GetPreset(name);
            if(anchor != null)
            {
                DispatchEvent(new AnchorMovedEventArgs(EVENT_ANCHOR_MOVED, Selection.Name, anchor));
            }
        }

        private void OnPositionPresetSelected(object sender, EventArgs e)
        {
            string name = positionPresets.SelectedItem.ToString();
            R3ePoint position = R3ePointPreset.GetPreset(name);
            if (position != null)
            {
                DispatchEvent(new PlaceHolderMovedEventArgs(EVENT_PLACEHOLDER_MOVED, Selection.Name, position));
            }
            if (linkAnchorsCheck.Checked)
            {
                anchorPresets.SelectedItem = position;
                DispatchEvent(new AnchorMovedEventArgs(EVENT_ANCHOR_MOVED, Selection.Name, position));
            }
        }

        private void SelectAnchorPreset()
        {
            anchorPresets.SelectedItem = R3ePointPreset.GetPresetName(Selection.Anchor);
        }

        private void SelectPositionPreset()
        {
            positionPresets.SelectedItem = R3ePointPreset.GetPresetName(Selection.Position);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (holdStepperEvent) return;

            if(sender == stepperX)
            {
                DispatchEvent(new PlaceHolderMovedEventArgs(EVENT_PLACEHOLDER_MOVED, Selection.Name, new R3ePoint((double)stepperX.Value, Selection.Position.Y)));
            }
            else if (sender == stepperY)
            {
                DispatchEvent(new PlaceHolderMovedEventArgs(EVENT_PLACEHOLDER_MOVED, Selection.Name, new R3ePoint(Selection.Position.X, (double)stepperY.Value)));
            }
            else if (sender == stepperSize)
            {
                DispatchEvent(new PlaceHolderResizedEventArgs(EVENT_PLACEHOLDER_RESIZED, Selection.Name, stepperSize.Value));
            }
        }

        private void InitializeComponent()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            WrapContents = false;
            BackColor = Color.LightGray;

            nameField = new Label()
            {
                AutoSize = false,
                Size = new Size(120, 18),
                Font = new Font(Font, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Label labelX = NewSimpleLabel("X");
            Label labelY = NewSimpleLabel("Y");
            Label labelSize = NewSimpleLabel("Size");
            Label labelAnchor = NewSimpleLabel("Anchor preset");
            Label labelPosition = NewSimpleLabel("Position preset");

            stepperX = NewStepper();
            stepperY = NewStepper();
            stepperSize = NewStepper();
            anchorPresets = NewComboBox();
            positionPresets = NewComboBox();

            linkAnchorsCheck = GetAnchorsLinkBox();

            Panel comboX = NewHCombo(labelX, stepperX);
            Panel comboY = NewHCombo(labelY, stepperY);
            Panel comboSize = NewHCombo(labelSize, stepperSize);

            comboSize.Margin = new Padding(comboSize.Margin.Left, comboSize.Margin.Top, comboSize.Margin.Right, 8);

            Controls.AddRange(new Control[] { nameField, comboX, comboX, comboY, comboSize, labelPosition, positionPresets, linkAnchorsCheck, labelAnchor, anchorPresets });
        }

        private CheckBox GetAnchorsLinkBox()
        {
            return new CheckBox()
            {
                Text = "Copy to anchor",
                Margin = new Padding(18, 0, 0, 0),
                ForeColor = Color.FromArgb(60, 60, 60),
                Font = new Font(Font.FontFamily, 7),
                AutoSize = true,
            };
        }

        private Panel NewHCombo(Label label, NumericUpDown stepper)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                WrapContents = false,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };

            label.Anchor= AnchorStyles.Top| AnchorStyles.Bottom;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSize = false;
            label.Width = 30;

            stepper.Width = 100;

            panel.Controls.AddRange(new Control[] { label, stepper });
            return panel;
        }

        private ComboBox NewComboBox()
        {
            return new ComboBox()
            {
                Size = new Size(110, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(Margin.Left, Margin.Top, Margin.Right, 4),
        };
        }

        private NumericUpDown NewStepper()
        {
            return new NumericUpDown()
            {
            };
        }

        private static Label NewSimpleLabel(string name)
        {
            return new Label()
            {
                AutoSize = true,
                Text = name,
            };
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
