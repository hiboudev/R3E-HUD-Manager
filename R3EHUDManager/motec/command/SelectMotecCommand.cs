using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.application.events;
using R3EHUDManager.graphics;
using R3EHUDManager.motec.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.motec.command
{
    class SelectMotecCommand : ICommand
    {
        private readonly IntEventArgs args;
        private readonly CollectionModel<MotecModel> collection;
        private readonly GraphicalAssetFactory assetsFactory;

        public SelectMotecCommand(IntEventArgs args, CollectionModel<MotecModel> collection, GraphicalAssetFactory assetsFactory)
        {
            this.args = args;
            this.collection = collection;
            this.assetsFactory = assetsFactory;
        }

        public void Execute()
        {
            foreach (var motec in collection.Items)
                if (motec.Id == args.Value)
                {
                    assetsFactory.SetMotec(motec);
                    break;
                }
        }
    }
}
