using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.model
{
    class BackgroundModel
    {
        public BackgroundModel(int id, string name, string filePath)
        {
            Id = id;
            Name = name;
            FilePath = filePath;
        }

        public int Id { get; }
        public string Name { get; }
        public string FilePath { get; }
    }
}
