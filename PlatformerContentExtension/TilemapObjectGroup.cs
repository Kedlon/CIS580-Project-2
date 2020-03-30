using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerContentExtension
{
    public class TilemapObjectGroup
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public List<TilemapObject> Objects = new List<TilemapObject>();
    }
}
