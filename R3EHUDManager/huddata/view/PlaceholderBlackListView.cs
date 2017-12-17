using da2mvc.core.events;
using R3EHUDManager.application.view;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.huddata.view
{
    class PlaceholderBlackListView : BaseModalForm, IEventDispatcher
    {
        private TableLayoutPanel layout;
        private ListView list;

        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_FILTERS_CHANGED = EventId.New();


        public PlaceholderBlackListView(PlaceholderBlackListModel blackList) : base("Filtered placeholders")
        {
            InitializeUI(blackList);
        }

        private void InitializeUI(PlaceholderBlackListModel blackList)
        {
            Size = new Size(300, 300);
            FormBorderStyle = FormBorderStyle.Sizable;

            layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
            };

            Label label = new Label()
            {
                Text = "Select placeholders to display.\nChanges will be applied on next loaded layout.",
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = true,
            };

            list = new ListView()
            {
                CheckBoxes = true,
                Dock = DockStyle.Fill,
                View = View.Details,
                HeaderStyle = ColumnHeaderStyle.None,
            };
            list.Columns.Add(new ColumnHeader()
            {
                Width = -1,
            });

            Button okButton = new Button()
            {
                Text = "OK",
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            };
            okButton.Click += OnOkClicked;

            AcceptButton = okButton;

            foreach(string placeholderName in PlaceholderName.GetAll())
            {
                ListViewItem item = new ListViewItem(placeholderName)
                {
                    Checked = !blackList.IsFiltered(placeholderName),
                };
                list.Items.Add(item);
            }


            AddControl(label, SizeType.AutoSize);
            AddControl(list, SizeType.Percent, 100);
            AddControl(okButton, SizeType.AutoSize);

            Controls.Add(layout);
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            DispatchEvent(new FiltersChangedEventArgs(EVENT_FILTERS_CHANGED, GetFilters()));
            DialogResult = DialogResult.OK;
        }

        private Dictionary<string, bool> GetFilters()
        {
            Dictionary<string, bool> filters = new Dictionary<string, bool>();
            foreach(ListViewItem item in list.Items)
            {
                filters.Add(item.Text, item.Checked);
            }
            return filters;
        }

        private void AddControl(Control control, SizeType sizeType)
        {
            layout.RowStyles.Add(new RowStyle(sizeType));
            layout.Controls.Add(control);
        }

        private void AddControl(Control control, SizeType sizeType, int height)
        {
            layout.RowStyles.Add(new RowStyle(sizeType, height));
            layout.Controls.Add(control);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
