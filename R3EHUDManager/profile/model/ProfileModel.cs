using da2mvc.framework.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.model
{
    public class ProfileModel : IModel
    {
        public ProfileModel(int id, string name, int backgroundId, string fileName, int motecId)
        {
            Id = id;
            Name = name;
            BackgroundId = backgroundId;
            FileName = fileName;
            MotecId = motecId;
        }

        public int Id { get; }
        public string Name { get; }
        public int BackgroundId { get; internal set; }
        public string FileName { get; }
        public int MotecId { get; }
    }
}
