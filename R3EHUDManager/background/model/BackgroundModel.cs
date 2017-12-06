using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.model
{
    class BackgroundModel
    {
        public BackgroundModel(int id, string name, string fileName, BaseDirectoryType directoryType)
        {
            Id = id;
            Name = name;
            FileName = fileName;
            DirectoryType = directoryType;
        }

        public int Id { get; }
        public string Name { get; }
        public string FileName { get; }
        public BaseDirectoryType DirectoryType { get; }
    }
}
