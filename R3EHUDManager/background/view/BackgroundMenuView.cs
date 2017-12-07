﻿using da2mvc.injection;
using R3EHUDManager.background.events;
using R3EHUDManager.contextmenu.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class BackgroundMenuView : AbstractContextMenuView
    {
        public const string EVENT_IMPORT_BACKGROUND = "importBackground";

        public BackgroundMenuView() : base("Background")
        {
            Width = 160;
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
            var backgroundManager = (BackgroundManagerView)Injector.GetInstance(typeof(BackgroundManagerView));

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
                var backgroundDialog = (PromptNewBackgroundView)Injector.GetInstance(typeof(PromptNewBackgroundView));
                backgroundDialog.SetBitmap(new Bitmap(fileDialog.FileName));

                if (backgroundDialog.ShowDialog() == DialogResult.OK)
                {
                    DispatchEvent(new ImportBackgroundEventArgs(EVENT_IMPORT_BACKGROUND, backgroundDialog.BackgroundName, fileDialog.FileName, backgroundDialog.CropRect));
                }
                backgroundDialog.Dispose();
            }

            fileDialog.Dispose();
        }
    }
}