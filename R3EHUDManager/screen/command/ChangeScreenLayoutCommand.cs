using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.coordinates;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.screen.command
{
    class ChangeScreenLayoutCommand : ICommand
    {
        private readonly ContextMenuEventArgs args;
        private readonly ScreenModel screenModel;
        private readonly PlaceHolderCollectionModel collectionModel;

        public ChangeScreenLayoutCommand(ContextMenuEventArgs args, ScreenModel screenModel, PlaceHolderCollectionModel collectionModel)
        {
            this.args = args;
            this.screenModel = screenModel;
            this.collectionModel = collectionModel;
        }

        public void Execute()
        {
            ScreenLayoutType layout = (ScreenLayoutType)args.ItemId;
            
            if (screenModel.Layout == layout) return;

            if (layout == ScreenLayoutType.SINGLE)
                CheckOutsidePlaceholder();
            
            screenModel.SetLayout(layout);
        }

        private void CheckOutsidePlaceholder()
        {
            bool outsidePlaceholder = false;

            foreach (PlaceholderModel placeholder in collectionModel.Placeholders)
            {
                ScreenPositionType screen = ScreenUtils.GetScreen(placeholder);
                if (screen != ScreenPositionType.CENTER)
                {
                    outsidePlaceholder = true;
                    break;
                }
            }

            if (outsidePlaceholder)
            {
                DialogResult result = MessageBox.Show("Some placeholders are outside the center screen, move them to center screen?", "Placeholders outside of center screen", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    foreach (PlaceholderModel placeholder in collectionModel.Placeholders)
                    {
                        ScreenPositionType screen = ScreenUtils.GetScreen(placeholder);
                        if (screen != ScreenPositionType.CENTER)
                        {
                            R3ePoint offset = ScreenUtils.ToScreenOffset(placeholder, ScreenPositionType.CENTER);
                            R3ePoint newPosition = new R3ePoint(placeholder.Position.X + offset.X, placeholder.Position.Y + offset.Y);
                            collectionModel.UpdatePlaceholderPosition(placeholder.Name, newPosition);
                        }
                    }
                }
            }
        }
    }
}
