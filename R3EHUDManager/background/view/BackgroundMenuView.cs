using da2mvc.core.injection;
using da2mvc.framework.menubutton.view;
using R3EHUDManager.background.events;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class BackgroundMenuView : MenuButtonView<BackgroundModel>
    {
        public const string EVENT_IMPORT_BACKGROUND = "importBackground";

        public BackgroundMenuView()
        {
            Width = 190;
        }

        protected override string Title => "Background";

        protected override ToolStripMenuItem ModelToItem(BackgroundModel model)
        {
            var item = base.ModelToItem(model);
            item.Image = GraphicalAsset.GetLayoutIcon(model.Layout);
            return item;
        }

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            List<ToolStripMenuItem> builtInItems = new List<ToolStripMenuItem>();

            ToolStripMenuItem itemImport = new ToolStripMenuItem("<Import new background>");
            itemImport.Click += OnImportBackgroundClicked;

            ToolStripMenuItem itemManage = new ToolStripMenuItem("<Manage backgrounds>");
            itemManage.Click += OnManageBackgroundClicked;

            builtInItems.Add(itemImport);
            builtInItems.Add(itemManage);
            return builtInItems;
        }

        private void OnManageBackgroundClicked(object sender, EventArgs e)
        {
            var backgroundManager = Injector.GetInstance< BackgroundManagerView>();

            backgroundManager.ShowDialog();

            backgroundManager.Dispose();
        }

        private void OnImportBackgroundClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp",
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var backgroundDialog = Injector.GetInstance< PromptNewBackgroundView>();
                backgroundDialog.SetBitmap(new Bitmap(fileDialog.FileName));

                if (backgroundDialog.ShowDialog() == DialogResult.OK)
                {
                    DispatchEvent(new ImportBackgroundEventArgs(EVENT_IMPORT_BACKGROUND, backgroundDialog.BackgroundName, backgroundDialog.BackgroundLayout, backgroundDialog.CropRect, fileDialog.FileName));
                }
                backgroundDialog.Dispose();
            }

            fileDialog.Dispose();
        }
    }
}
