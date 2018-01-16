using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.database;
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
        private readonly R3eDatabase r3EDatabase;

        public InitializeMotecsCommand(LocationModel location, MotecParser parser, CollectionModel<MotecModel> collection, R3eDatabase r3eDatabase)
        {
            this.location = location;
            this.parser = parser;
            this.collection = collection;
            r3EDatabase = r3eDatabase;
        }

        public void Execute()
        {
            List<MotecModel> motecs = parser.Parse(location.MotecXmlFile);
            List<int> carIds = new List<int>();
            // List the cars we need to load in DB.
            foreach(var motec in motecs)
            {
                foreach (var carId in motec.CarIds)
                    if (carIds.Contains(carId))
                        throw new Exception("Two different motecs can't contain same car id.");
                    else
                        carIds.Add(carId);
            }
            Dictionary<int, Car> cars = r3EDatabase.GetCars(carIds.ToArray());
            List<Car> motecCars = new List<Car>();
            // Apply the cars to each motec.
            foreach (var motec in motecs)
            {
                foreach (var carId in motec.CarIds)
                    motecCars.Add(cars[carId]);
                motec.SetCars(motecCars.ToArray());
                motecCars.Clear();
            }

            collection.AddRange(motecs);
        }
    }
}
