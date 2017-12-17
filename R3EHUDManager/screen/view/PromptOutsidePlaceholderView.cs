using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.screen.view
{
    class PromptOutsidePlaceholderView : BaseModalForm, IEventDispatcher
    {
        private CheckBox checkBox;

        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_REMEMBER_CHOICE = EventId.New();

        public PromptOutsidePlaceholderView() : base("Placeholder(s) outside of center screen")
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Size = new Size(300, 150);
            //FormBorderStyle = FormBorderStyle.Sizable;

            Panel layout = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                //BackColor = Color.Green,
                WrapContents = false,
            };

            Label label = new Label()
            {
                Text = "Some placeholders are now outside of the screen, move them to center screen?",
                //BackColor = Color.Blue,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0,0,0,8)
            };

            checkBox = new CheckBox()
            {
                Text = "Remember my choice",
                AutoSize = true,
                //Anchor = AnchorStyles.Right,
            };

            Button yesButton = new Button()
            {
                Text = "Yes",
                Anchor = AnchorStyles.Right,
            };
            yesButton.Click += OnYesClicked;

            Button noButton = new Button()
            {
                Text = "No",
                Anchor = AnchorStyles.Right,
            };
            noButton.Click += OnNoClicked;

            Panel buttonLayout = new TableLayoutPanel()
            {
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1,
                //BackColor = Color.Red,
                Anchor = AnchorStyles.Right,
            };

            buttonLayout.Controls.Add(yesButton);
            buttonLayout.Controls.Add(noButton);

            layout.Controls.Add(label);
            layout.Controls.Add(checkBox);
            layout.Controls.Add(buttonLayout);

            Controls.Add(layout);

            noButton.Select();
        }

        private void OnYesClicked(object sender, EventArgs e)
        {
            if (checkBox.Checked)
                DispatchEvent(new IntEventArgs(EVENT_REMEMBER_CHOICE, (int)OutsidePlaceholdersPrefType.MOVE));

            DialogResult = DialogResult.Yes;
        }

        private void OnNoClicked(object sender, EventArgs e)
        {
            if (checkBox.Checked)
                DispatchEvent(new IntEventArgs(EVENT_REMEMBER_CHOICE, (int)OutsidePlaceholdersPrefType.DO_NOTHING));

            DialogResult = DialogResult.No;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
