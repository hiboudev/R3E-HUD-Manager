using da2mvc.framework.model;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.model
{
    public class BackgroundModel : IModel
    {
        public BackgroundModel(int id, string name, string fileName, BaseDirectoryType directoryType, bool IsBuiltInt, ScreenLayoutType layout)
        {
            Id = id;
            Name = name;
            FileName = fileName;
            DirectoryType = directoryType;
            this.IsBuiltInt = IsBuiltInt;
            Layout = layout;
        }

        public int Id { get; }
        public string Name { get; }
        public string FileName { get; }
        public BaseDirectoryType DirectoryType { get; }
        public bool IsBuiltInt { get; }
        public ScreenLayoutType Layout { get; }
    }
}
