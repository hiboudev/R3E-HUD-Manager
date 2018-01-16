using da2mvc.core.events;
using da2mvc.core.injection;
using da2mvc.framework.menubutton.view;
using R3EHUDManager.background.events;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;
using R3EHUDManager.background.view;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace R3EHUDManager.background.view
{
    class BackgroundMenuView : MenuButtonView<BackgroundModel>
    {
        public static readonly int EVENT_IMPORT_BACKGROUND = EventId.New();
        private readonly GraphicalAssetFactory assetsFactory;

        public BackgroundMenuView(GraphicalAssetFactory assetsFactory)
        { 
            this.assetsFactory = assetsFactory;
            Width = 200;
        }

        protected override string Title => "Background";

        protected override MenuItem ModelToItem(BackgroundModel model)
        {
            var item = base.ModelToItem(model);
            item.Icon = new Image() { Source = assetsFactory.GetLayoutIcon(model.Layout) };

            return item;
        }

        protected override List<MenuItem> GetBuiltInItems()
        {
            List<MenuItem> builtInItems = new List<MenuItem>();

            MenuItem itemImport = new MenuItem
            {
                Header = "<Import new background>"
            };
            itemImport.Click += OnImportBackgroundClicked;

            MenuItem itemManage = new MenuItem
            {
                Header = "<Manage backgrounds>"
            };
            itemManage.Click += OnManageBackgroundClicked;

            builtInItems.Add(itemImport);
            builtInItems.Add(itemManage);
            return builtInItems;
        }

        private void OnManageBackgroundClicked(object sender, EventArgs e)
        {
            var backgroundManager = Injector.GetInstance<BackgroundManagerView>();
            backgroundManager.ShowDialog();
            backgroundManager.Dispose();
        }

        private void OnImportBackgroundClicked(object sender, EventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp",
            };
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var backgroundDialog = Injector.GetInstance<BackgroundImporterView>();
                backgroundDialog.SetBitmap(new BitmapImage(new Uri(fileDialog.FileName)));

                if (backgroundDialog.ShowDialog() == true)
                {
                    DispatchEvent(new ImportBackgroundEventArgs(EVENT_IMPORT_BACKGROUND, backgroundDialog.BackgroundName,
                        backgroundDialog.BackgroundLayout, backgroundDialog.CropRect, fileDialog.FileName));
                }
            }

            fileDialog.Dispose();
        }
    }
}
