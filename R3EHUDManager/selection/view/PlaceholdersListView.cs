﻿using System;
using System.Linq;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using da2mvc.core.events;
using R3EHUDManager.application.events;
using System.Drawing;
using da2mvc.framework.collection.view;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using R3EHUDManager.r3esupport.result;
using System.Reflection;
using R3EHUDManager.graphics;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListView : TableLayoutPanel, IEventDispatcher, ICollectionView<PlaceholderModel>
    {
        private ListView list;
        private bool bypassSelectedEvent = false;
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_PLACEHOLDER_SELECTED = EventId.New();
        public static readonly int EVENT_PLACEHOLDER_UNSELECTED = EventId.New();
        public static readonly int EVENT_REQUEST_LAYOUT_FIX = EventId.New();
        private Dictionary<int, ListViewItem> items = new Dictionary<int, ListViewItem>();
        private Dictionary<int, ValidationResult> validations = new Dictionary<int, ValidationResult>();
        private MenuItem menuItemFixLayout;

        public PlaceholdersListView()
        {
            InitializeUI();
        }

        public void Add(PlaceholderModel[] models)
        {
            foreach (PlaceholderModel model in models.OrderBy(x => x.Name)) // TODO sort sur la liste car là ça ne marcherait pas pour plusieurs ajouts consécutifs
            {
                ListViewItem item = new ListViewItem(model.Name)
                {
                    Tag = model.Id,
                };

                SetValidationResult(model, model.ValidationResult);
                item.ToolTipText = validations[model.Id].Description;
                
                list.Items.Add(item);
                items.Add(model.Id, item);
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

            items[id].Selected = true;
            //if(!list.Focused)
            //    list.Select();

            bypassSelectedEvent = false;
        }

        internal void UnselectPlaceholder(int id)
        {
            bypassSelectedEvent = true;

            items[id].Selected = false;

            bypassSelectedEvent = false;
        }

        internal void SetValidationResult(PlaceholderModel placeholder, ValidationResult result)
        {
            if (validations.ContainsKey(placeholder.Id))
                validations[placeholder.Id] = result;
            else
                validations.Add(placeholder.Id, result);

            if (items.ContainsKey(placeholder.Id))
                items[placeholder.Id].ToolTipText = result.Description;

            list.Invalidate();
        }

        private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            bool selected = list.SelectedIndices.Contains(e.ItemIndex);
            int thickness = 2, spacing = 1;

            e.DrawBackground();

            if (selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Colors.PLACEHOLDER_LIST_SELECTION),
                    new Rectangle(e.Bounds.X + thickness + 1, e.Bounds.Y + spacing, e.Bounds.Width - thickness - 1, e.Bounds.Height - 2 * spacing));
                //e.DrawFocusRectangle();
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
            }

            int modelId = (int)e.Item.Tag;

            if (validations.ContainsKey(modelId) && validations[modelId].Type == ResultType.INVALID)
            {
                Color color = validations[modelId].HasFix() ? Colors.LAYOUT_NOTIFICATION_FIX : Colors.LAYOUT_NOTIFICATION_NO_FIX;

                e.Graphics.DrawLine(new Pen(color, thickness),
                    new Point(e.Bounds.Left + 1, e.Bounds.Top + spacing),
                    new Point(e.Bounds.Left + 1, e.Bounds.Bottom - spacing));
            }

            e.DrawText(TextFormatFlags.VerticalCenter);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (bypassSelectedEvent) return;

            if (list.SelectedItems.Count > 0)
                DispatchEvent(new IntEventArgs(EVENT_PLACEHOLDER_SELECTED, (int)list.SelectedItems[0].Tag));
            else
                DispatchEvent(new BaseEventArgs(EVENT_PLACEHOLDER_UNSELECTED));
        }

        private void InitializeUI()
        {
            AutoSize = true;
            MinimumSize = new Size(100, 100);

            Label title = new Label()
            {
                Height = 15,
                Text = "Placeholders",
                Margin = new Padding(),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGray,
            };

            list = new ListView
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.WhiteSmoke,
                View = View.Details,
                HeaderStyle = ColumnHeaderStyle.None,
                FullRowSelect = true,
                MultiSelect = false,
                OwnerDraw = true,
                ShowItemToolTips = true,
                Margin = new Padding(),
            };

            ColumnHeader column = new ColumnHeader
            {
                Width = 130,
            };
            list.Columns.Add(column);

            list.DrawItem += OnDrawItem;
            list.SelectedIndexChanged += OnSelectedIndexChanged;

            typeof(ListView).InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               list,
               new object[] { true });

            AddToTable(title, SizeType.AutoSize, AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            AddToTable(list, SizeType.Percent, AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);

            InitializeContextMenu();
        }

        private void InitializeContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Popup += OnContextMenuPopup;
            menuItemFixLayout = new MenuItem("Apply layout fixes", OnMenuFixLayoutClick);
            contextMenu.MenuItems.Add(menuItemFixLayout);

            ContextMenu = contextMenu;
        }

        private void OnContextMenuPopup(object sender, EventArgs e)
        {
            if (list.SelectedItems.Count == 0) return;

            int modelId = (int)list.SelectedItems[0].Tag;

            if (!validations.ContainsKey(modelId)) return;

            if (validations[modelId].Type == ResultType.INVALID && validations[modelId].HasFix())
            {
                menuItemFixLayout.Text = "Apply layout fix";
                menuItemFixLayout.Enabled = true;
            }
            else
            {
                menuItemFixLayout.Text = "No available layout fix";
                menuItemFixLayout.Enabled = false;
            }
        }

        private void OnMenuFixLayoutClick(object sender, EventArgs e)
        {
            if (list.SelectedItems.Count == 0) return;

            int modelId = (int)list.SelectedItems[0].Tag;

            if (!validations.ContainsKey(modelId)) return;

            ValidationResult validationResult = validations[modelId];

            if (validationResult != null && validationResult.HasFix())
                DispatchEvent(new IntEventArgs(EVENT_REQUEST_LAYOUT_FIX, modelId));
        }

        private void AddToTable(Control control, SizeType sizeType, AnchorStyles anchor)
        {
            control.Anchor = anchor;
            RowStyles.Add(new RowStyle(sizeType));
            Controls.Add(control);
        }

        private void AddToTable(Control control, SizeType sizeType, int height, AnchorStyles anchor)
        {
            control.Anchor = anchor;
            RowStyles.Add(new RowStyle(sizeType, height));
            Controls.Add(control);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
