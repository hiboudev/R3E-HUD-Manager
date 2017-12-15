using System;
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

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListView : Panel, IEventDispatcher, ICollectionView<PlaceholderModel>
    {
        private ListView list;
        private bool bypassSelectedEvent = false;
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_PLACEHOLDER_SELECTED = EventId.New();
        private Dictionary<int, ListViewItem> items = new Dictionary<int, ListViewItem>();
        private Dictionary<int, ValidationResult> validations = new Dictionary<int, ValidationResult>();

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

                if (validations.ContainsKey(model.Id))
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
            }
        }

        public void Clear()
        {
            list.Items.Clear();
            items.Clear();
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

            if (selected)
            {
                e.Graphics.FillRectangle(Brushes.LightSkyBlue, e.Bounds);
                //e.DrawFocusRectangle();
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
            }

            int modelId = (int)e.Item.Tag;

            if (validations.ContainsKey(modelId) && validations[modelId].Type == ResultType.INVALID)
            {
                int thickness = 3;
                e.Graphics.DrawLine(new Pen(Color.OrangeRed, thickness),
                    new Point(e.Bounds.Right - thickness, e.Bounds.Top),
                    new Point(e.Bounds.Right - thickness, e.Bounds.Bottom));
            }

            e.DrawText(TextFormatFlags.VerticalCenter);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (bypassSelectedEvent) return;

            if (list.SelectedItems.Count > 0)
                DispatchEvent(new IntEventArgs(EVENT_PLACEHOLDER_SELECTED, (int)list.SelectedItems[0].Tag));
        }

        private void InitializeUI()
        {
            Label title = new Label()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Height = 15,
                Text = "Placeholders",
                Dock = DockStyle.Top,
                Margin = new Padding(Margin.Left, Margin.Top, Margin.Right, 4),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGray,
            };

            list = new ListView
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.WhiteSmoke,
                Dock = DockStyle.Fill,
                View = View.Details,
                HeaderStyle = ColumnHeaderStyle.None,
                FullRowSelect = true,
                MultiSelect = false,
                OwnerDraw = true,
                ShowItemToolTips = true,
            };

            ColumnHeader column = new ColumnHeader
            {
                Width = -2, // Autosize... o.O
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

            Controls.Add(list);
            Controls.Add(title);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
