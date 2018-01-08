using da2mvc.core.events;
using da2mvc.core.view;
using R3EHUDManager.application.events;
using R3EHUDManager.coordinates;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager_wpf.screen.view;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace R3EHUDManager_wpf.placeholder.view
{
    class PlaceholderView : DrawingVisual, IView, IEventDispatcher
    {
        public event EventHandler Disposed;
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_REQUEST_LAYOUT_FIX = EventId.New();

        public PlaceholderModel Model { get; private set; }
        private Rect screenArea;
        private bool isTripleScreen;
        private ToolTip toolTip;
        private ContextMenu contextMenu;
        private bool isSelected;
        private bool showDecoration;
        private LayoutValidationResult validationResult;
        private Rect labelHitTestRect;
        private MenuItem menuItemFixLayout;

        public PlaceholderView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            toolTip = new ToolTip();
            contextMenu = new ContextMenu();
            menuItemFixLayout = new MenuItem();
            menuItemFixLayout.Click += OnMenuFixLayoutClick;
            contextMenu.Items.Add(menuItemFixLayout);
        }

        private void OnMenuFixLayoutClick(object sender, EventArgs e)
        {
            if (validationResult != null && validationResult.HasFix())
                DispatchEvent(new IntEventArgs(EVENT_REQUEST_LAYOUT_FIX, Model.Id));
        }

        public bool ShowDecoration
        {
            get => showDecoration;
            set
            {
                showDecoration = value;
                Render();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                Render();
            }
        }

        public Rect ScreenArea
        {
            get => screenArea;
            set
            {
                screenArea = value;
            }
        }

        public bool IsTripleScreen
        {
            get => isTripleScreen;
            set
            {
                isTripleScreen = value;
                // TODO invalidate?
            }
        }

        internal void Initialize(PlaceholderModel model, bool isTripleScreen)
        {
            Model = model;
            this.isTripleScreen = isTripleScreen;
            SetValidationResult(model.ValidationResult);
        }

        public void Render()
        {
            if (screenArea == null)
                return;

            PrivateRender(GetRenderRect());
        }

        internal void SetValidationResult(LayoutValidationResult result)
        {
            validationResult = result;

            if (validationResult.Type == ResultType.VALID)
            {
                toolTip.Content = "Layout is valid";
                menuItemFixLayout.Header = "Layout is valid";
                menuItemFixLayout.IsEnabled = false;
                if (toolTip.IsOpen) toolTip.IsOpen = false;
            }
            else
            {
                toolTip.Content = result.Description;

                if (validationResult.HasFix())
                {
                    menuItemFixLayout.Header = "Apply layout fix";
                    menuItemFixLayout.IsEnabled = true;
                }
                else
                {
                    menuItemFixLayout.Header = "No available layout fix";
                    menuItemFixLayout.IsEnabled = false;
                }
            }

            PrivateRender(GetRenderRect()); // TODO est-ce qu'on ne dessinerait pas trop souvent ?
        }

        public void ShowLayoutFixMenu()
        {
            if (validationResult != null && validationResult.Type == ResultType.INVALID)
            {
                toolTip.IsOpen = false;
                contextMenu.IsOpen = true;
            }
        }

        internal void ShowToolTip(bool show)
        {
            if (!show)
            {
                toolTip.IsOpen = false;
                return;
            }

            if (!contextMenu.IsOpen && !toolTip.IsOpen && validationResult != null && validationResult.Type == ResultType.INVALID)
                toolTip.IsOpen = true;
        }

        internal Rect GetLabelHitTestRect()
        {
            if (!showDecoration && !isSelected) return new Rect();
            return labelHitTestRect;
        }

        private Rect GetRenderRect()
        {
            Size size = Model.ResizeRule.GetSize(Model.Size, ScreenView.BASE_RESOLUTION, screenArea.Size, GraphicalAsset.GetPlaceholderSize(Model.Name), isTripleScreen);

            R3ePoint modelLocation = Model.Position.Clone();
            if (isTripleScreen) modelLocation.X /= 3;

            Point anchor = Coordinates.FromR3e(Model.Anchor, size);
            Point location = Coordinates.FromR3e(modelLocation, screenArea.Size);

            location.Offset(-anchor.X + screenArea.Location.X, -anchor.Y + screenArea.Location.Y);

            return new Rect(location.X, location.Y, size.Width, size.Height);
        }

        private void PrivateRender(Rect rect)
        {
            DrawingContext drawingContext = RenderOpen();

            // Background
            if (showDecoration || isSelected)
            {
                drawingContext.DrawRectangle(
                    new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)),
                    null,
                    rect
                    );
            }

            // Image
            drawingContext.DrawImage(GraphicalAsset.GetPlaceholderImage(Model.Name), rect);

            int validationWidth = 3;

            // Label
            if (showDecoration || isSelected)
            {
                Typeface typeface = new Typeface(new FontFamily(), FontStyles.Normal, FontWeights.Bold, new FontStretch());
                FormattedText text = new FormattedText(Model.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 10, new SolidColorBrush(Colors.Black))
                {
                    MaxTextWidth = rect.Width,
                    MaxTextHeight = rect.Height
                };

                labelHitTestRect = new Rect(rect.X + validationWidth, rect.Y, text.Width, text.Height);

                drawingContext.DrawRectangle(
                    new SolidColorBrush(isSelected ? AppColors.PLACEHOLDER_SELECTION : Colors.LightGray),
                    null,
                    labelHitTestRect
                    );

                drawingContext.DrawText(text, new Point(rect.X + validationWidth, rect.Y));

                // Layout validation
                Color validationColor;
                if (validationResult != null && validationResult.Type == ResultType.INVALID)
                    validationColor = validationResult.HasFix() ? AppColors.LAYOUT_NOTIFICATION_FIX : AppColors.LAYOUT_NOTIFICATION_NO_FIX;
                else
                    validationColor = Colors.Gray;

                drawingContext.DrawRectangle(
                    new SolidColorBrush(validationColor),
                    null,
                    new Rect(rect.X, rect.Y, validationWidth, text.Height)
                    );
            }

            // Selection rectangle
            if (isSelected)
            {
                drawingContext.DrawRectangle(null, new Pen(new SolidColorBrush(AppColors.PLACEHOLDER_SELECTION), 1), rect);
            }

            // Anchor
            if (showDecoration || isSelected)
            {
                // Anchor
                double thickness = 4;
                Size anchorArea = new Size(rect.Width - thickness, rect.Height - thickness);
                Point anchorPosition = Coordinates.FromR3e(Model.Anchor, anchorArea);
                anchorPosition.X += rect.X;
                anchorPosition.Y += rect.Y;
                drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(255, 54, 42, 212)), null, new Rect(anchorPosition, new Size(thickness, thickness)));
            }

            drawingContext.Close();
        }

        public void Dispose()
        {
            if (toolTip.IsOpen) toolTip.IsOpen = false;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
