using da2mvc.framework.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.motec.model
{
    class MotecModel:IModel
    {
        public MotecModel(int id, string fileName, int[] cardIds)
        {
            Id = id;
            FileName = fileName;
            CardIds = cardIds;
        }

        public string FileName { get; }
        public int[] CardIds { get; }

        public int Id { get; }
        public string Name => FileName;
    }
}
