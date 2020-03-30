using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerContentExtension
{
    public struct TilemapTileset
    {
        public int FirstGID;

        public string Source;
    }

    public class TilemapContent
    {
        public int TileWidth { get; set; }

        public int TileHeight { get; set; }

        public List<TilemapTileset> TilesetData = new List<TilemapTileset>();

        public List<TilemapLayerContent> Layers = new List<TilemapLayerContent>();

        public List<TilesetContent> Tilesets = new List<TilesetContent>();
    }
}
