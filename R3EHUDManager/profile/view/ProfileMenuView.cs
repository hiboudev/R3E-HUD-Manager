using da2mvc.injection;
using R3EHUDManager.contextmenu.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuView : AbstractContextMenuView
    {
        public ProfileMenuView() : base("Profile")
        {
        }

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            var list = new List<ToolStripMenuItem>();

            ToolStripMenuItem itemSaveProfile = new ToolStripMenuItem("<Save profile>");
            itemSaveProfile.Click += OnSaveProfileClicked;

            ToolStripMenuItem itemSaveToNewProfile = new ToolStripMenuItem("<Save to new profile>");
            itemSaveToNewProfile.Click += OnSaveToNewProfileClicked;

            list.Add(itemSaveProfile);
            list.Add(itemSaveToNewProfile);

            return list;
        }

        private void OnSaveToNewProfileClicked(object sender, EventArgs e)
        {
            var newProfileDialog = (PromptNewProfileView)Injector.GetInstance(typeof(PromptNewProfileView));

            if(newProfileDialog.ShowDialog() == DialogResult.OK)
            {

            }

            newProfileDialog.Dispose();
        }

        private void OnSaveProfileClicked(object sender, EventArgs e)
        {
        }
    }
}
