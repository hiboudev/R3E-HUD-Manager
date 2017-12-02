using R3EHUDManager.data.parser;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.command;
using System.Windows.Forms;

namespace R3EHUDManager.data.command
{
    class LoadHudDataCommand : ICommand
    {
        private readonly LocationModel locationModel;
        private readonly HudOptionsParser parser;
        private readonly PlaceHolderCollectionModel placeHolderCollection;

        public LoadHudDataCommand(LocationModel locationModel, HudOptionsParser parser, PlaceHolderCollectionModel placeHolderCollection)
        {
            this.locationModel = locationModel;
            this.parser = parser;
            this.placeHolderCollection = placeHolderCollection;
        }

        public void Execute()
        {
            List<PlaceholderModel> placeholders = null;
            try
            {
                placeholders = parser.Parse(locationModel.HudOptionsPath);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while parsing HUD xml file, application will exit.\n" + e.Message, "Error");
                Environment.Exit(0);
            }

            placeHolderCollection.AddRange(placeholders);
        }
    }
}
