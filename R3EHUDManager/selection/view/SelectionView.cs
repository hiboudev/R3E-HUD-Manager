using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using da2mvc.core.events;
using System.Diagnostics;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.coordinates;
using System.Threading;
using System.Globalization;
using System.Drawing;
using R3EHUDManager.application.events;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.events;
using R3EHUDManager.placeholder.command;
using R3EHUDManager.screen.utils;
using R3EHUDManager.selection.events;

namespace R3EHUDManager.selection.view
{
    class SelectionView : FlowLayoutPanel, IEventDispatcher
    {
        private NumericUpDown stepperX;
        private NumericUpDown stepperY;
        private NumericUpDown stepperSize;
        private Label nameField;

        public event EventHandler MvcEventHandler;

        public static readonly int EVENT_PLACEHOLDER_MOVED = EventId.New();
        public static readonly int EVENT_ANCHOR_MOVED = EventId.New();
        public static readonly int EVENT_PLACEHOLDER_RESIZED = EventId.New();
        public static readonly int EVENT_MOVE_TO_SCREEN = EventId.New();

        private ComboBox anchorPresets;
        private ComboBox positionPresets;
        private CheckBox linkAnchorsCheck;

        public PlaceholderModel Selection { get; private set; }

        private bool holdStepperEvent = false;
        private bool holdScreenEvent = false;

        private RadioButton screenLeftRadio;
        private RadioButton screenCenterRadio;
        private RadioButton screenRightRadio;
        private Panel screenPanel;
        private bool isTripleScreen;

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
            stepperSize.Value = (decimal)Selection.Size.X;

            holdStepperEvent = false;

            SelectAnchorPreset();
            SelectPositionPreset();

            UpdateTripleScreenUI();
        }

        internal void TripleScreenChanged(ScreenLayoutType layout)
        {
            isTripleScreen = layout == ScreenLayoutType.TRIPLE;

            UpdateTripleScreenUI();
        }

        internal void SetSelected(PlaceholderModel placeholder)
        {
            Selection = placeholder;
            UpdateData();
            UpdateTripleScreenUI();

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

        private void UpdateTripleScreenUI()
        {
            screenPanel.Visible = isTripleScreen;

            if (Selection == null) return;

            holdScreenEvent = true;
            switch (ScreenUtils.GetScreen(Selection))
            {
                case ScreenPositionType.LEFT:
                    screenLeftRadio.Checked = true;
                    break;
                case ScreenPositionType.CENTER:
                    screenCenterRadio.Checked = true;
                    break;
                case ScreenPositionType.RIGHT:
                    screenRightRadio.Checked = true;
                    break;
                case ScreenPositionType.OUTSIDE:
                    screenRightRadio.Checked = screenCenterRadio.Checked = screenLeftRadio.Checked = false;
                    break;
            }
            holdScreenEvent = false;
        }

        private void InitializeUI()
        {
            Enabled = false;

            linkAnchorsCheck.Checked = true;

            stepperX.DecimalPlaces = stepperY.DecimalPlaces = stepperSize.DecimalPlaces = 3;
            stepperX.Minimum = stepperY.Minimum = stepperSize.Minimum = decimal.MinValue;
            stepperX.Maximum = stepperY.Maximum = stepperSize.Maximum = decimal.MaxValue;

            stepperX.ValueChanged += OnValueChanged;
            stepperY.ValueChanged += OnValueChanged;
            stepperSize.ValueChanged += OnValueChanged;

            stepperX.Increment = stepperY.Increment = stepperSize.Increment = (decimal)0.001;

            stepperX.GotFocus += SelectStepperText;
            stepperY.GotFocus += SelectStepperText;
            stepperSize.GotFocus += SelectStepperText;

            stepperX.MouseWheel += OnStepperMouseWheel;
            stepperY.MouseWheel += OnStepperMouseWheel;
            stepperSize.MouseWheel += OnStepperMouseWheel;

            foreach (string presetName in R3ePointPreset.presets.Keys)
            {
                anchorPresets.Items.Add(presetName);
                positionPresets.Items.Add(presetName);
            }
            anchorPresets.SelectionChangeCommitted += OnAnchorPresetSelected;
            positionPresets.SelectionChangeCommitted += OnPositionPresetSelected;

            screenPanel.Visible = false;
            screenLeftRadio.CheckedChanged += ScreenRadioChanged;
            screenCenterRadio.CheckedChanged += ScreenRadioChanged;
            screenRightRadio.CheckedChanged += ScreenRadioChanged;
        }

        private void OnStepperMouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs handledArgs = (HandledMouseEventArgs) e;
            handledArgs.Handled = true;
            var stepper = (NumericUpDown)sender;
            stepper.Value += (e.Delta > 0 ? 1 : -1) * stepper.Increment * 20;
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
                DispatchEvent(new SelectionViewEventArgs(EVENT_ANCHOR_MOVED, UpdateType.ANCHOR, anchor));
            }
        }

        private void OnPositionPresetSelected(object sender, EventArgs e)
        {
            string name = positionPresets.SelectedItem.ToString();
            R3ePoint position = isTripleScreen ? R3ePointPreset.GetPreset(name, ScreenUtils.GetScreen(Selection)) : R3ePointPreset.GetPreset(name);

            DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, UpdateType.POSITION, position));

            if (linkAnchorsCheck.Checked)
            {
                anchorPresets.SelectedItem = position;
                DispatchEvent(new SelectionViewEventArgs(EVENT_ANCHOR_MOVED, UpdateType.ANCHOR, R3ePointPreset.GetPreset(name)));
            }
        }

        private void ScreenRadioChanged(object sender, EventArgs e)
        {
            if (holdScreenEvent) return;

            ScreenPositionType screenType = (ScreenPositionType)((RadioButton)sender).Tag;
            DispatchEvent(new PlaceholderScreenEventArgs(EVENT_MOVE_TO_SCREEN, Selection.Name, screenType));
        }

        private void SelectAnchorPreset()
        {
            anchorPresets.SelectedItem = R3ePointPreset.GetPresetName(Selection.Anchor);
        }

        private void SelectPositionPreset()
        {
            positionPresets.SelectedItem = R3ePointPreset.GetPresetName(Selection.Position, ScreenUtils.GetScreen(Selection));
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (holdStepperEvent) return;

            if(sender == stepperX)
            {
                DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, UpdateType.POSITION, new R3ePoint((double)stepperX.Value, Selection.Position.Y)));
            }
            else if (sender == stepperY)
            {
                DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, UpdateType.POSITION, new R3ePoint(Selection.Position.X, (double)stepperY.Value)));
            }
            else if (sender == stepperSize)
            {
                DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_RESIZED, UpdateType.SIZE, new R3ePoint((double)stepperSize.Value, (double)stepperSize.Value)));
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

            screenLeftRadio = NewScreenRadio("L", ScreenPositionType.LEFT);
            screenCenterRadio = NewScreenRadio("C", ScreenPositionType.CENTER);
            screenRightRadio = NewScreenRadio("R", ScreenPositionType.RIGHT);

            screenPanel = NewScreenPanel(screenLeftRadio, screenCenterRadio, screenRightRadio);

            Controls.AddRange(new Control[] { nameField, comboX, comboX, comboY, comboSize, screenPanel, labelPosition, positionPresets, linkAnchorsCheck, labelAnchor, anchorPresets });
        }

        private Panel NewScreenPanel(RadioButton screenLeftRadio, RadioButton screenCenterRadio, RadioButton screenRightRadio)
        {
            Label label = NewSimpleLabel("Screen");

            FlowLayoutPanel vPanel = new FlowLayoutPanel()
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
            };

            TableLayoutPanel panel = new TableLayoutPanel()
            {
                RowCount = 1,
                ColumnCount = 3,
                AutoSize = true,
                Margin = new Padding(Margin.Left, 0, 0, 0),
            };

            panel.Controls.AddRange(new Control[] { screenLeftRadio, screenCenterRadio, screenRightRadio });

            vPanel.Controls.Add(label);
            vPanel.Controls.Add(panel);

            return vPanel;
        }

        private RadioButton NewScreenRadio(string text, ScreenPositionType screenType)
        {
            return new RadioButton()
            {
                Text = text,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 7),
                Margin = new Padding(Margin.Left, 0, Margin.Right, Margin.Bottom),
                Tag = screenType
            };
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
