//using System;
//using System.Linq;
//using System.Windows.Forms;
//using R3EHUDManager.placeholder.model;
//using da2mvc.core.events;
//using R3EHUDManager.application.events;
//using System.Drawing;
//using da2mvc.framework.collection.view;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing.Drawing2D;
//using R3EHUDManager.r3esupport.result;
//using System.Reflection;
//using R3EHUDManager.graphics;

//namespace R3EHUDManager.selection.view
//{
//    class PlaceholdersListView : TableLayoutPanel, IEventDispatcher, ICollectionView<PlaceholderModel>
//    {


//        private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
//        {
//            bool selected = list.SelectedIndices.Contains(e.ItemIndex);
//            int thickness = 2, spacing = 1;

//            e.DrawBackground();

//            if (selected)
//            {
//                e.Graphics.FillRectangle(new SolidBrush(Colors.PLACEHOLDER_LIST_SELECTION),
//                    new Rectangle(e.Bounds.X + thickness + 1, e.Bounds.Y + spacing, e.Bounds.Width - thickness - 1, e.Bounds.Height - 2 * spacing));
//                //e.DrawFocusRectangle();
//            }
//            else
//            {
//                e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
//            }

//            int modelId = (int)e.Item.Tag;

//            if (validations.ContainsKey(modelId) && validations[modelId].Type == ResultType.INVALID)
//            {
//                Color color = validations[modelId].HasFix() ? Colors.LAYOUT_NOTIFICATION_FIX : Colors.LAYOUT_NOTIFICATION_NO_FIX;

//                e.Graphics.DrawLine(new Pen(color, thickness),
//                    new Point(e.Bounds.Left + 1, e.Bounds.Top + spacing),
//                    new Point(e.Bounds.Left + 1, e.Bounds.Bottom - spacing));
//            }

//            e.DrawText(TextFormatFlags.VerticalCenter);
//        }

//        private void OnSelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (bypassSelectedEvent) return;

//            if (list.SelectedItems.Count > 0)
//                DispatchEvent(new IntEventArgs(EVENT_PLACEHOLDER_SELECTED, (int)list.SelectedItems[0].Tag));
//            else
//                DispatchEvent(new BaseEventArgs(EVENT_PLACEHOLDER_UNSELECTED));
//        }



//            InitializeContextMenu();
//        }

//        private void AddToTable(Control control, SizeType sizeType, AnchorStyles anchor)
//        {
//            control.Anchor = anchor;
//            RowStyles.Add(new RowStyle(sizeType));
//            Controls.Add(control);
//        }

//        private void AddToTable(Control control, SizeType sizeType, int height, AnchorStyles anchor)
//        {
//            control.Anchor = anchor;
//            RowStyles.Add(new RowStyle(sizeType, height));
//            Controls.Add(control);
//        }

//        public void DispatchEvent(BaseEventArgs args)
//        {
//            MvcEventHandler?.Invoke(this, args);
//        }
//    }
//}
