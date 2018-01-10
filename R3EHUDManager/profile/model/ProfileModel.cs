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
        public ProfileModel(int id, string name, int backgroundId, string fileName)
        {
            Id = id;
            Name = name;
            BackgroundId = backgroundId;
            this.FileName = fileName;
        }

        public int Id { get; }
        public string Name { get; }
        public int BackgroundId { get; internal set; }
        public string FileName { get; }
    }
}
