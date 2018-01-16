using da2mvc.framework.model;
using R3EHUDManager.database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.motec.model
{
    public class MotecModel:IModel
    {
        private string carList;

        public MotecModel(int id, string fileName, int[] carIds)
        {
            Id = id;
            FileName = fileName;
            CarIds = carIds;
        }

        internal void SetCars(Car[] cars)
        {
            Cars = cars;
            carList = "";
            foreach (var car in cars)
            {
                carList += $"{car.ClassName} {car.Name}, ";
            }
            carList = carList.Substring(0, carList.Length - 2);
        }

        public string FilePath { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_graphical_assets", "motec", FileName + ".png"); } // TODO use LocationModel
        public string CarList { get => carList; }

        public string FileName { get; }
        public int[] CarIds { get; }

        public int Id { get; }
        public string Name => FileName;

        public Car[] Cars { get; private set; }
    }
}
