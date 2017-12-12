using da2mvc.events;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.contextmenu.view
{
    abstract class AbstractContextMenuView : Button, IEventDispatcher
    {
        private readonly string title;
        private List<ToolStripMenuItem> builtInItems;
        private List<ToolStripMenuItem> regularItems = new List<ToolStripMenuItem>();
        public event EventHandler MvcEventHandler;
        public const string EVENT_ITEM_CLICKED = "itemClicked";
        protected bool DrawArrow { get; set; } = true;
        public int TitleMaxLength { get; protected set; } = 18;

        public AbstractContextMenuView(string title)
        {
            this.title = title;
            InitializeUI();
        }

        public void AddItem(ContextMenuViewItem item)
        {
            ToolStripMenuItem toolItem = new ToolStripMenuItem(item.Name)
            {
                Tag = item.Id,
                Image = item.Image,
            };

            toolItem.Click += ItemClicked;
            regularItems.Add(toolItem);

            regularItems.Sort((x, y) => string.Compare(x.Text, y.Text));
            Redraw();
        }

        public void AddItems(List<ContextMenuViewItem> items)
        {
            foreach (ContextMenuViewItem item in items)
            {
                ToolStripMenuItem toolItem = new ToolStripMenuItem(item.Name)
                {
                    Tag = item.Id,
                    Image = item.Image,
                };

                toolItem.Click += ItemClicked;
                regularItems.Add(toolItem);
            }

            regularItems.Sort((x, y) => string.Compare(x.Text, y.Text));
            Redraw();
        }

        public void RemoveItem(int id)
        {
            foreach (var item in regularItems)
                if ((int)item.Tag == id)
                {
                    regularItems.Remove(item);
                    // No dispose of Image cause it's a cached instance.
                    item.Dispose();
                    break;
                }
            
            regularItems.Sort((x, y) => string.Compare(x.Text, y.Text));
            Redraw();
        }

        public void ClearItems()
        {
            regularItems.Clear();
            Redraw();
        }

        private void Redraw()
        {
            ContextMenuStrip.Items.Clear();
            ContextMenuStrip.Items.AddRange(regularItems.ToArray());
            if(builtInItems.Count > 0 && regularItems.Count > 0)
                ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.AddRange(builtInItems.ToArray());
        }

        virtual public bool SetSelectedItem(string name)
        {
            bool itemChecked = false;

            foreach (ToolStripMenuItem item in regularItems)
            {
                item.Checked = item.Text == name;

                if (item.Checked)
                {
                    UpdateTitle(item.Text);
                    itemChecked = true;
                }
            }

            if (!itemChecked) UpdateTitle(null);

            return itemChecked;
        }

        virtual public bool SetSelectedItem(int id)
        {
            bool itemChecked = false;

            foreach (ToolStripMenuItem item in regularItems)
            {
                item.Checked = (int)item.Tag == id;

                if (item.Checked)
                {
                    UpdateTitle(item.Text);
                    itemChecked = true;
                }
            }

            if (!itemChecked) UpdateTitle(null);

            return itemChecked;
        }

        private void UpdateTitle(string selectedName)
        {
            if(selectedName == null)
            {
                Text = $"{title}";
                return;
            }

            int maxLength = TitleMaxLength;

            if (selectedName.Length > maxLength)
                selectedName = selectedName.Substring(0, maxLength) + "..."; // TODO bug ?

            Text = $"{title}: {selectedName}";
        }

        private void InitializeUI()
        {
            BackColor = Color.LightGray;
            Text = title;
            Width = 140;
            TextAlign = ContentAlignment.MiddleLeft;
            // To avoid a bug when there's several instances, opening one will push focus to the next one, cause of the Enabled=false.
            SetStyle(ControlStyles.Selectable, false);
            // I can't find how to disable word wrapping in Button. If text is in one word and too long, it displays nothing.
            AutoSize = true;

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += MenuOpening;
            ContextMenuStrip.Closed += MenuClosed;

            builtInItems = GetBuiltInItems();
            //foreach (ToolStripMenuItem item in builtInItems)
            //    item.Font = new Font(item.Font, FontStyle.Italic);

            Redraw();
        }

        protected void EnableItem(ToolStripMenuItem item, bool enabled)
        {
            item.Font = new Font(item.Font, enabled ? FontStyle.Regular : FontStyle.Italic);
            item.Enabled = enabled;
        }

        private void ItemClicked(object sender, EventArgs e)
        {
            DispatchEvent(new ContextMenuEventArgs(EVENT_ITEM_CLICKED, (int)((ToolStripMenuItem)sender).Tag, ((ToolStripMenuItem)sender).Text));
        }

        abstract protected List<ToolStripMenuItem> GetBuiltInItems();

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

        private void MenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            Enabled = true;
        }

        private void MenuOpening(object sender, EventArgs e)
        {
            Enabled = false;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if(ContextMenuStrip.Items.Count > 0)
                ContextMenuStrip.Show(this, new Point(Location.X, Height));
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (DrawArrow &&  ContextMenuStrip != null)
            {
                int arrowX = ClientRectangle.Width - 14;
                int arrowY = ClientRectangle.Height / 2 - 1;

                Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
                Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
                pevent.Graphics.FillPolygon(brush, arrows);
            }
        }
    }

    class ContextMenuViewItem
    {
        public ContextMenuViewItem(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public ContextMenuViewItem(int id, string name, Image image)
        {
            Name = name;
            Image = image;
            Id = id;
        }

        public string Name { get; }
        public Image Image { get; }
        public int Id { get; }
    }
}
