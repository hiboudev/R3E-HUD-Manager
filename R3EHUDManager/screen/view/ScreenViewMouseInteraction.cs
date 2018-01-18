using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.placeholder.events;
using R3EHUDManager.r3esupport.result;
using R3EHUDManager.placeholder.view;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace R3EHUDManager.screen.view
{
    public class ScreenViewMouseInteraction : IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_REQUEST_MOVE = EventId.New();
        public static readonly int EVENT_REQUEST_SELECTION = EventId.New();
        public static readonly int EVENT_REQUEST_DESELECTION = EventId.New();
        private DragManager dragManager;
        private ToolTipManager toolTipManager;
        private LayoutFixManager layoutFixManager;

        internal void Initialize(ScreenView screenView)
        {
            dragManager = new DragManager(screenView, this);
            toolTipManager = new ToolTipManager(screenView, this);
            layoutFixManager = new LayoutFixManager(screenView, this);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

        class LayoutFixManager
        {
            private readonly ScreenView screenView;
            private readonly ScreenViewMouseInteraction interaction;

            public LayoutFixManager(ScreenView screenView, ScreenViewMouseInteraction interaction)
            {
                this.screenView = screenView;
                this.interaction = interaction;
                screenView.MouseRightButtonDown += RightButtonClicked;
            }

            private void RightButtonClicked(object sender, MouseButtonEventArgs e)
            {
                PlaceholderView view = screenView.GetViewUnder(Mouse.GetPosition(screenView));
                if (view == null) return;

                view.ShowLayoutFixMenu();
            }
        }

        class ToolTipManager
        {
            private readonly ScreenView screenView;
            private readonly ScreenViewMouseInteraction interaction;
            private readonly DispatcherTimer timer;
            private PlaceholderView toolTipView;
            private Point mouseTriggerPosition;

            public ToolTipManager(ScreenView screenView, ScreenViewMouseInteraction interaction)
            {
                this.screenView = screenView;
                this.interaction = interaction;

                timer = new DispatcherTimer()
                {
                    Interval = new TimeSpan(0, 0, 0, 1, 200),
                    IsEnabled = true,
                };
                timer.Tick += TimerTick;

                screenView.MouseMove += OnMouseMove;
            }

            private void TimerTick(object sender, EventArgs e)
            {
                timer.Stop();
                DisplayTooltip();
            }

            private void OnMouseMove(object sender, EventArgs e)
            {
                if (toolTipView != null)
                {
                    double distance = Point.Subtract(Mouse.GetPosition(screenView), mouseTriggerPosition).Length;
                    if (distance > 10)
                    {
                        toolTipView.ShowToolTip(false);
                        toolTipView = null;
                    }
                }

                timer.Stop();
                timer.Start();
            }

            private void DisplayTooltip()
            {
                PlaceholderView view = screenView.GetViewUnder(Mouse.GetPosition(screenView));
                if (view == null) return;

                if (!view.GetLabelHitTestRect().Contains(Mouse.GetPosition(screenView))) return;

                if (toolTipView != null)
                {
                    toolTipView.ShowToolTip(false);
                    toolTipView = null;
                }

                toolTipView = view;
                mouseTriggerPosition = Mouse.GetPosition(screenView);
                view.ShowToolTip(true);
            }
        }

        class DragManager
        {
            private readonly ScreenView screenView;
            private readonly ScreenViewMouseInteraction interaction;
            private PlaceholderView draggingView;
            private Point dragMouseStart;
            private Point dragViewOffset;

            public DragManager(ScreenView screenView, ScreenViewMouseInteraction interaction)
            {
                this.screenView = screenView;
                this.interaction = interaction;
                screenView.MouseLeftButtonDown += OnMouseDown;
            }

            private void OnMouseDown(object sender, MouseButtonEventArgs e)
            {
                Point mousePosition = e.GetPosition(screenView);
                PlaceholderView view = screenView.GetViewUnder(mousePosition);

                if (view == null)
                {
                    interaction.DispatchEvent(new BaseEventArgs(ScreenViewMouseInteraction.EVENT_REQUEST_DESELECTION));
                    return;
                }

                if (view.IsSelected)
                    StartDrag(e, view);
                else
                    interaction.DispatchEvent(new IntEventArgs(ScreenViewMouseInteraction.EVENT_REQUEST_SELECTION, view.Model.Id));
            }

            private void StartDrag(MouseButtonEventArgs e, PlaceholderView view)
            {
                draggingView = view;

                dragMouseStart = e.GetPosition(screenView); // TODO sert à rien de le stocker
                dragViewOffset = new Point(dragMouseStart.X - view.ContentBounds.X, dragMouseStart.Y - view.ContentBounds.Y);

                screenView.CaptureMouse();
                screenView.MouseMove += OnMouseMove;
                screenView.MouseUp += OnMouseUp;
            }

            private void OnMouseMove(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    // In case we release button outside of the area.
                    ReleaseDrag();
                    return;
                }

                Point newPosition = e.GetPosition(screenView);
                newPosition.Offset(-dragViewOffset.X, -dragViewOffset.Y);

                interaction.DispatchEvent(new PlaceholderViewEventArgs(ScreenViewMouseInteraction.EVENT_REQUEST_MOVE, draggingView, newPosition));
            }

            private void OnMouseUp(object sender, MouseButtonEventArgs e)
            {
                ReleaseDrag();
            }

            private void ReleaseDrag()
            {
                screenView.ReleaseMouseCapture();
                screenView.MouseMove -= OnMouseMove;
                screenView.MouseUp -= OnMouseUp;
                draggingView = null;
            }
        }
    }
}
