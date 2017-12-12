using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.model
{
    class R3eDirectoryModel
    {
        public R3eDirectoryModel(int id, string name, string path)
        {
            Id = id;
            Name = name;
            Path = path;
        }

        public int Id { get; }
        public string Name { get; }
        public string Path { get; }
    }
}
