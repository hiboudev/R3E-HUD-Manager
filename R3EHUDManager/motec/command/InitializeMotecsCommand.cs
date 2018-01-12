using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.location.model;
using R3EHUDManager.motec.model;
using R3EHUDManager.motec.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.motec.command
{
    class InitializeMotecsCommand : ICommand
    {
        private readonly LocationModel location;
        private readonly MotecParser parser;
        private readonly CollectionModel<MotecModel> collection;

        public InitializeMotecsCommand(LocationModel location, MotecParser parser, CollectionModel<MotecModel> collection)
        {
            this.location = location;
            this.parser = parser;
            this.collection = collection;
        }

        public void Execute()
        {
            collection.AddRange(parser.Parse(location.MotecXmlFile));
        }
    }
}
