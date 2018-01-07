using da2mvc.core.events;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.events;
using R3EHUDManager.graphics;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace R3EHUDManager_wpf.selection.view
{
    /// <summary>
    /// Logique d'interaction pour PlaceholderListView.xaml
    /// </summary>
    public partial class PlaceholderListView : UserControl, IEventDispatcher, ICollectionView<PlaceholderModel>
    {
        public event EventHandler MvcEventHandler;
        public event EventHandler Disposed;

        public static readonly int EVENT_PLACEHOLDER_SELECTED = EventId.New();
        public static readonly int EVENT_PLACEHOLDER_UNSELECTED = EventId.New();
        public static readonly int EVENT_REQUEST_LAYOUT_FIX = EventId.New();

        private bool bypassSelectedEvent = false;
        private Dictionary<int, PlaceholderListItem> items = new Dictionary<int, PlaceholderListItem>();
        private Dictionary<int, LayoutValidationResult> validations = new Dictionary<int, LayoutValidationResult>();
        private MenuItem menuItemFixLayout;

        public PlaceholderListView()
        {
            InitializeComponent();
            InitializeUI();
        }

        public void Add(PlaceholderModel[] models)
        {
            foreach (PlaceholderModel model in models.OrderBy(x => x.Name)) // TODO sort sur la liste car là ça ne marcherait pas pour plusieurs ajouts consécutifs
            {
                PlaceholderListItem item = new PlaceholderListItem()
                {
                    Content = model.Name,
                    Tag = model.Id,
                };

                list.Items.Add(item);
                items.Add(model.Id, item);

                SetValidationResult(model, model.ValidationResult);
                item.ToolTip = validations[model.Id].Description == "" ? null : validations[model.Id].Description;
            }
        }

        public void Remove(PlaceholderModel[] models)
        {
            foreach (PlaceholderModel model in models)
            {
                list.Items.Remove(items[model.Id]);
                items.Remove(model.Id);
                validations.Remove(model.Id);
            }
        }

        public void Clear()
        {
            list.Items.Clear();
            items.Clear();
            validations.Clear();
        }

        internal void SelectPlaceholder(int id)
        {
            bypassSelectedEvent = true;

            items[id].IsSelected = true;
            //if(!list.Focused)
            //    list.Select();

            bypassSelectedEvent = false;
        }

        internal void UnselectPlaceholder(int id)
        {
            bypassSelectedEvent = true;

            items[id].IsSelected = false;

            bypassSelectedEvent = false;
        }

        internal void SetValidationResult(PlaceholderModel placeholder, LayoutValidationResult result)
        {
            if (validations.ContainsKey(placeholder.Id))
                validations[placeholder.Id] = result;
            else
                validations.Add(placeholder.Id, result);

            if (items.ContainsKey(placeholder.Id))
            {
                items[placeholder.Id].ToolTip = result.Description == "" ? null : result.Description;
                items[placeholder.Id].SetValidationResult(result);
            }

            list.InvalidateVisual();
        }

        private void InitializeUI()
        {
            // TODO paramétrer la liste
            //                BorderStyle = BorderStyle.None,
            //                BackColor = Color.WhiteSmoke,
            //                View = View.Details,
            //                HeaderStyle = ColumnHeaderStyle.None,
            //                FullRowSelect = true,
            //                MultiSelect = false,
            //                OwnerDraw = true,
            //                ShowItemToolTips = true,
            //                Margin = new Padding(),
            //            list.DrawItem += OnDrawItem;

            InitializeContextMenu();


            list.SelectionChanged += OnSelectionChanged;
        }

        private void InitializeContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            ContextMenuOpening += OnContextMenuPopup;
            menuItemFixLayout = new MenuItem()
            {
                Header = "Apply layout fixes",
            };
            menuItemFixLayout.Click += OnMenuFixLayoutClick;
            contextMenu.Items.Add(menuItemFixLayout);

            ContextMenu = contextMenu;
        }

        private void OnContextMenuPopup(object sender, EventArgs e)
        {
            if (list.SelectedItems.Count == 0) return;

            int modelId = (int)((PlaceholderListItem)list.SelectedItems[0]).Tag;

            if (!validations.ContainsKey(modelId)) return;

            if (validations[modelId].Type == ResultType.INVALID && validations[modelId].HasFix())
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

        private void OnMenuFixLayoutClick(object sender, EventArgs e)
        {
            if (list.SelectedItems.Count == 0) return;

            int modelId = (int)((PlaceholderListItem)list.SelectedItems[0]).Tag;

            if (!validations.ContainsKey(modelId)) return;

            LayoutValidationResult validationResult = validations[modelId];

            if (validationResult != null && validationResult.HasFix())
                DispatchEvent(new IntEventArgs(EVENT_REQUEST_LAYOUT_FIX, modelId));
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bypassSelectedEvent) return;

            if (list.SelectedItems.Count > 0)
                DispatchEvent(new IntEventArgs(EVENT_PLACEHOLDER_SELECTED, (int)((PlaceholderListItem)list.SelectedItems[0]).Tag));
            else
                DispatchEvent(new BaseEventArgs(EVENT_PLACEHOLDER_UNSELECTED));
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }



        class PlaceholderListItem : ListBoxItem
        {
            private LayoutValidationResult validationResult;

            internal void SetValidationResult(LayoutValidationResult result)
            {
                // TODO ramener le code de tooltip+contextMenu ici.
                validationResult = result;
                InvalidateVisual();
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (validationResult != null && validationResult.Type == ResultType.INVALID)
                {
                    int validationWidth = 3;
                    Color validationColor;

                    if (validationResult != null && validationResult.Type == ResultType.INVALID)
                        validationColor = validationResult.HasFix() ? AppColors.LAYOUT_NOTIFICATION_FIX : AppColors.LAYOUT_NOTIFICATION_NO_FIX;
                    else
                        validationColor = Colors.Gray;

                    drawingContext.DrawRectangle(
                        new SolidColorBrush(validationColor),
                        null,
                        new Rect(1, 0, validationWidth, RenderSize.Height)
                        );
                }
            }
        }
    }

}
