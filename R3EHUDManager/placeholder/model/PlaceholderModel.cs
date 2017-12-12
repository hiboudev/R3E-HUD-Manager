using da2mvc.framework.model;
using R3EHUDManager.coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    class PlaceholderModel : IModel
    {
        public int Id { get; }
        public string Name { get; }
        public R3ePoint Position { get; internal set; }
        public R3ePoint Anchor { get; internal set; }
        public R3ePoint Size { get; internal set; }
        public IResizeRule ResizeRule { get; internal set; }

        //public PlaceholderModel(string name, R3ePoint location, R3ePoint anchor, R3ePoint size)
        //{
        //    Name = name;
        //    Position = location;
        //    Anchor = anchor;
        //    Size = size;
        //}

        public PlaceholderModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
