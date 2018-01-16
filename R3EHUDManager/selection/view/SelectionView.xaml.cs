using da2mvc.core.events;
using da2mvc.core.view;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;
using R3EHUDManager.selection.events;
using R3EHUDManager.selection.view;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace R3EHUDManager.selection.view
{
    /// <summary>
    /// Logique d'interaction pour SelectionView.xaml
    /// </summary>
    public partial class SelectionView : UserControl, IView, IEventDispatcher
    {

        public static readonly int EVENT_PLACEHOLDER_MOVED = EventId.New();
        public static readonly int EVENT_ANCHOR_MOVED = EventId.New();
        public static readonly int EVENT_PLACEHOLDER_RESIZED = EventId.New();
        public static readonly int EVENT_MOVE_TO_SCREEN = EventId.New();

        private bool holdStepperEvent = false;
        private bool holdScreenEvent = false;
        private bool holdPresetEvent = false;

        internal PlaceholderModel Selection { get; private set; }
        private bool isTripleScreen;

        public SelectionView()
        {
            InitializeComponent();
            InitializeUI();
        }

        internal void UpdateData()
        {
            nameField.Content = Selection.Name;

            holdStepperEvent = true;
            stepperX.Value = Selection.Position.X;
            stepperY.Value = Selection.Position.Y;
            stepperSize.Value = Selection.Size.X;
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
            UpdateTripleScreenUI(); // TODO Is it necessary?

            IsEnabled = true;
        }

        internal void Unselect()
        {
            Selection = null;
            nameField.Content = "";

            holdStepperEvent = true;
            stepperX.Value = null;
            stepperY.Value = null;
            stepperSize.Value = null;
            holdStepperEvent = false;

            holdPresetEvent = true;
            anchorPresets.SelectedItem = null;
            positionPresets.SelectedItem = null;
            holdPresetEvent = false;

            IsEnabled = false;
        }

        internal void RefreshCulture()
        {
            stepperX.CultureInfo = stepperY.CultureInfo = stepperSize.CultureInfo = CultureInfo.DefaultThreadCurrentCulture;
        }

        private void UpdateTripleScreenUI()
        {
            screenPanel.Visibility = isTripleScreen ? Visibility.Visible : Visibility.Collapsed;

            if (Selection == null) return;

            holdScreenEvent = true;
            switch (ScreenUtils.GetScreen(Selection))
            {
                case ScreenPositionType.LEFT:
                    screenLeftRadio.IsChecked = true;
                    break;
                case ScreenPositionType.CENTER:
                    screenCenterRadio.IsChecked = true;
                    break;
                case ScreenPositionType.RIGHT:
                    screenRightRadio.IsChecked = true;
                    break;
                case ScreenPositionType.OUTSIDE:
                    screenRightRadio.IsChecked = screenCenterRadio.IsChecked = screenLeftRadio.IsChecked = false;
                    break;
            }
            holdScreenEvent = false;
        }

        private void InitializeUI()
        {
            IsEnabled = false;

            // Force autosize now.
            nameField.Content = "";

            linkAnchorsCheck.IsChecked = true;

            //stepperX.DecimalPlaces = stepperY.DecimalPlaces = stepperSize.DecimalPlaces = 3;
            stepperX.Minimum = stepperY.Minimum = stepperSize.Minimum = double.MinValue;
            stepperX.Maximum = stepperY.Maximum = stepperSize.Maximum = double.MaxValue;

            stepperX.ValueChanged += OnStepperValueChanged;
            stepperY.ValueChanged += OnStepperValueChanged;
            stepperSize.ValueChanged += OnStepperValueChanged;

            stepperX.Increment = stepperY.Increment = stepperSize.Increment = 0.001;

            stepperX.MouseWheelActiveTrigger =
                stepperY.MouseWheelActiveTrigger =
                stepperSize.MouseWheelActiveTrigger = MouseWheelActiveTrigger.Disabled;
            stepperX.MouseWheel += OnStepperMouseWheel;
            stepperY.MouseWheel += OnStepperMouseWheel;
            stepperSize.MouseWheel += OnStepperMouseWheel;

            foreach (string presetName in R3ePointPreset.presets.Keys)
            {
                anchorPresets.Items.Add(presetName);
                positionPresets.Items.Add(presetName);
            }
            anchorPresets.SelectionChanged += OnAnchorPresetSelected;
            positionPresets.SelectionChanged += OnPositionPresetSelected;
            positionPresets.MouseEnter += OnComboBoxMouseEnter;
            anchorPresets.MouseEnter += OnComboBoxMouseEnter;

            screenPanel.Visibility = Visibility.Collapsed;
            screenLeftRadio.Checked += ScreenRadioChecked;
            screenCenterRadio.Checked += ScreenRadioChecked;
            screenRightRadio.Checked += ScreenRadioChecked;

            screenLeftRadio.Tag = ScreenPositionType.LEFT;
            screenCenterRadio.Tag = ScreenPositionType.CENTER;
            screenRightRadio.Tag = ScreenPositionType.RIGHT;

            screenPanel.MouseWheel += OnRadioMouseWheel;
        }

        private void OnComboBoxMouseEnter(object sender, MouseEventArgs e)
        {
            // Focus on mouseEnter so we can scroll it with mouseWheel without to click beforehand.
            (sender as ComboBox).Focus();
        }

        private void SelectAnchorPreset()
        {
            holdPresetEvent = true;
            anchorPresets.SelectedItem = R3ePointPreset.GetPresetName(Selection.Anchor);
            holdPresetEvent = false;
        }

        private void SelectPositionPreset()
        {
            holdPresetEvent = true;
            positionPresets.SelectedItem = R3ePointPreset.GetPresetName(Selection.Position, ScreenUtils.GetScreen(Selection));
            holdPresetEvent = false;
        }

        private void OnRadioMouseWheel(object sender, MouseWheelEventArgs e)
        {
            RadioButton[] buttons = new RadioButton[] { screenLeftRadio, screenCenterRadio, screenRightRadio };
            int checkedIndex = Array.FindIndex(buttons, x => x.IsChecked == true);

            checkedIndex += e.Delta > 0 ? -1 : 1;

            if (checkedIndex >= 0 && checkedIndex < 3)
                buttons[checkedIndex].IsChecked = true;
        }

        private void ScreenRadioChecked(object sender, RoutedEventArgs e)
        {
            if (holdScreenEvent) return;

            ScreenPositionType screenType = (ScreenPositionType)((RadioButton)sender).Tag;
            DispatchEvent(new PlaceholderScreenEventArgs(EVENT_MOVE_TO_SCREEN, Selection.Id, screenType));
        }

        private void OnPositionPresetSelected(object sender, SelectionChangedEventArgs e)
        {
            if (holdPresetEvent) return;

            string name = positionPresets.SelectedItem.ToString();
            R3ePoint position = isTripleScreen ? R3ePointPreset.GetPreset(name, ScreenUtils.GetScreen(Selection)) : R3ePointPreset.GetPreset(name);

            DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, position));

            if (linkAnchorsCheck.IsChecked == true)
            {
                anchorPresets.SelectedItem = position;
                DispatchEvent(new SelectionViewEventArgs(EVENT_ANCHOR_MOVED, R3ePointPreset.GetPreset(name)));
            }
        }

        private void OnAnchorPresetSelected(object sender, SelectionChangedEventArgs e)
        {
            if (holdPresetEvent) return;

            string name = anchorPresets.SelectedItem.ToString();
            R3ePoint anchor = R3ePointPreset.GetPreset(name);
            if (anchor != null)
            {
                DispatchEvent(new SelectionViewEventArgs(EVENT_ANCHOR_MOVED, anchor));
            }
        }

        private void OnStepperValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (holdStepperEvent) return;

            if (sender == stepperX)
            {
                DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, new R3ePoint(Convert.ToDouble(stepperX.Value), Selection.Position.Y)));
            }
            else if (sender == stepperY)
            {
                DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, new R3ePoint(Selection.Position.X, Convert.ToDouble(stepperY.Value))));
            }
            else if (sender == stepperSize)
            {
                DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_RESIZED, new R3ePoint(Convert.ToDouble(stepperSize.Value), Convert.ToDouble(stepperSize.Value))));
            }
        }

        //private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    if (holdStepperEvent) return;

        //    if (sender == stepperX)
        //    {
        //        DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, UpdateType.POSITION, new R3ePoint((double)stepperX.Value, Selection.Position.Y)));
        //    }
        //    else if (sender == stepperY)
        //    {
        //        DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_MOVED, UpdateType.POSITION, new R3ePoint(Selection.Position.X, (double)stepperY.Value)));
        //    }
        //    else if (sender == stepperSize)
        //    {
        //        DispatchEvent(new SelectionViewEventArgs(EVENT_PLACEHOLDER_RESIZED, UpdateType.SIZE, new R3ePoint((double)stepperSize.Value, (double)stepperSize.Value)));
        //    }
        //}

        private void OnStepperMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var stepper = (DoubleUpDown)sender;
            stepper.Value += (e.Delta > 0 ? 1 : -1) * stepper.Increment * 20;
            e.Handled = true;
        }

        public event EventHandler Disposed;
        public event EventHandler MvcEventHandler;
        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
