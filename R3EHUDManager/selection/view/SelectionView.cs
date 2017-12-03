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

namespace R3EHUDManager.selection.view
{
    class SelectionView : Panel, IEventDispatcher
    {
        private NumericUpDown stepperX;
        private Label label1;
        private Label label2;
        private NumericUpDown stepperY;
        private Label label3;
        private NumericUpDown stepperSize;
        private Label nameField;

        public event EventHandler MvcEventHandler;
        public const string EVENT_PLACEHOLDER_MOVED = "placeholderMoved";
        public const string EVENT_ANCHOR_MOVED = "anchorMoved";
        public const string EVENT_PLACEHOLDER_RESIZED = "placeholderResized";
        private ComboBox anchorPresets;
        private Label label4;

        public PlaceholderModel Selection { get; private set; }
        private bool holdChangeEvent = false;

        public SelectionView()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Enabled = false;

            stepperX.DecimalPlaces = stepperY.DecimalPlaces = stepperSize.DecimalPlaces = 3;
            stepperX.Minimum = stepperY.Minimum = stepperSize.Minimum = decimal.MinValue;
            stepperX.Maximum = stepperY.Maximum = stepperSize.Maximum = decimal.MaxValue;
            //stepperX.Minimum = -3;
            //stepperX.Maximum = 3;
            //stepperY.Minimum = -1;
            //stepperY.Maximum = 1;
            stepperSize.Minimum = (decimal)0.1;
            //stepperSize.Maximum = 4;

            stepperX.ValueChanged += OnValueChanged;
            stepperY.ValueChanged += OnValueChanged;
            stepperSize.ValueChanged += OnValueChanged;

            stepperX.Increment = stepperY.Increment = stepperSize.Increment = (decimal)0.001;


            anchorPresets.DropDownStyle = ComboBoxStyle.DropDownList;

            foreach (string presetName in AnchorPreset.presets.Keys)
            {
                anchorPresets.Items.Add(presetName);
            }
            anchorPresets.SelectionChangeCommitted += OnAnchorPresetSelected;
        }

        private void OnAnchorPresetSelected(object sender, EventArgs e)
        {
            string name = anchorPresets.SelectedItem.ToString();
            R3ePoint anchor = AnchorPreset.GetPreset(name);
            if(anchor != null)
            {
                DispatchEvent(new AnchorMovedEventArgs(EVENT_ANCHOR_MOVED, Selection.Name, anchor));
            }
        }

        private void SelectAnchorPreset()
        {
            string presetName = AnchorPreset.GetPresetName(Selection.Anchor);
            if (presetName != null)
                anchorPresets.SelectedItem = presetName;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (holdChangeEvent) return;

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

        internal void UpdateData()
        {
            nameField.Text = Selection.Name;

            holdChangeEvent = true;
            stepperX.Value = (decimal)Selection.Position.X;
            stepperY.Value = (decimal)Selection.Position.Y;
            if((decimal)Selection.Size.X > stepperSize.Minimum)
                stepperSize.Value = (decimal)Selection.Size.X;
            holdChangeEvent = false;

            SelectAnchorPreset();
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
            holdChangeEvent = true;
            stepperX.Value = 0;
            stepperY.Value = 0;
            stepperSize.Value = 1;
            holdChangeEvent = false;

            anchorPresets.SelectedItem = null;

            Enabled = false;
        }

        private void InitializeComponent()
        {
            this.nameField = new System.Windows.Forms.Label();
            this.stepperX = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.stepperY = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.stepperSize = new System.Windows.Forms.NumericUpDown();
            this.anchorPresets = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.stepperX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepperY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepperSize)).BeginInit();
            this.SuspendLayout();
            // 
            // nameField
            // 
            this.nameField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameField.Location = new System.Drawing.Point(12, 9);
            this.nameField.Name = "nameField";
            this.nameField.Size = new System.Drawing.Size(120, 18);
            this.nameField.TabIndex = 0;
            // 
            // stepperX
            // 
            this.stepperX.Location = new System.Drawing.Point(156, 7);
            this.stepperX.Name = "stepperX";
            this.stepperX.Size = new System.Drawing.Size(68, 20);
            this.stepperX.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // stepperY
            // 
            this.stepperY.Location = new System.Drawing.Point(248, 7);
            this.stepperY.Name = "stepperY";
            this.stepperY.Size = new System.Drawing.Size(68, 20);
            this.stepperY.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Size";
            // 
            // stepperSize
            // 
            this.stepperSize.Location = new System.Drawing.Point(355, 7);
            this.stepperSize.Name = "stepperSize";
            this.stepperSize.Size = new System.Drawing.Size(68, 20);
            this.stepperSize.TabIndex = 7;
            // 
            // anchorList
            // 
            this.anchorPresets.FormattingEnabled = true;
            this.anchorPresets.Location = new System.Drawing.Point(476, 6);
            this.anchorPresets.Name = "anchorList";
            this.anchorPresets.Size = new System.Drawing.Size(121, 21);
            this.anchorPresets.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(429, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Anchor";
            // 
            // SelectionView
            // 
            this.ClientSize = new System.Drawing.Size(613, 380);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.anchorPresets);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.stepperSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stepperY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stepperX);
            this.Controls.Add(this.nameField);
            this.Name = "SelectionView";
            ((System.ComponentModel.ISupportInitialize)(this.stepperX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepperY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepperSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
