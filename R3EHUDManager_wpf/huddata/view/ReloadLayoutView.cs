//using da2mvc.core.events;
//using R3EHUDManager.graphics;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace R3EHUDManager.huddata.view
//{
//    class ReloadLayoutView : TableLayoutPanel, IEventDispatcher
//    {
//        public event EventHandler MvcEventHandler;
//        public static readonly int EVENT_SAVE_CLICKED = EventId.New();
//        public static readonly int EVENT_RELOAD_CLICKED = EventId.New();
//        public static readonly int EVENT_RELOAD_DEFAULT_CLICKED = EventId.New();
//        private Button saveButton;

//        public ReloadLayoutView()
//        {
//            InitializeUI();
//        }

//        internal void SetSaveStatus(bool isSaved)
//        {
//            if (isSaved)
//                saveButton.Font = Fonts.SAVED_FONT;
//            else
//                saveButton.Font = Fonts.UNSAVED_FONT;
//        }

//        private void InitializeUI()
//        {
//            //Dock = DockStyle.Fill;
//            RowCount = 3;
//            ColumnCount = 1;
//            AutoSize = true;
//            //Padding = new Padding();

//            Controls.AddRange(new Control[]
//            {
//                NewButton("Apply to R3E", EVENT_SAVE_CLICKED, ref saveButton),
//                NewButton("Reload from R3E", EVENT_RELOAD_CLICKED),
//                NewButton("Reload original", EVENT_RELOAD_DEFAULT_CLICKED),
//            });
//        }

//        private Button NewButton(string text, int eventId)
//        {
//            Button button = new Button()
//            {
//                Text = text,
//                Anchor = AnchorStyles.Left | AnchorStyles.Right,
//                //Dock = DockStyle.Fill,

//                Margin = new Padding(),
//                Padding = new Padding(),
//            };
//            button.Click += (sender, args) => DispatchEvent(new BaseEventArgs(eventId));

//            return button;
//        }

//        private Button NewButton(string text, int eventId, ref Button field)
//        {
//            field = NewButton(text, eventId);
//            return field;
//        }

//        public void DispatchEvent(BaseEventArgs args)
//        {
//            MvcEventHandler?.Invoke(this, args);
//        }
//    }
//}
