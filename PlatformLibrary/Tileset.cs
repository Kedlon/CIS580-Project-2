using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformLibrary
{
    /// <summary>
    /// A class representing a tileset created in Tiled
    /// </summary>
    public class Tileset
    {
        Tile[] tiles;

        /// <summary>
        /// Constructs a new instance of Tileset
        /// </summary>
        /// <param name="tiles">The tiles in this set</param>
        public Tileset(Tile[] tiles)
        {
            this.tiles = tiles;
        }

        /// <summary>
        /// An indexer for accessing individual tiles in the set
        /// </summary>
        /// <param name="index">The index of the tile</param>
        /// <returns>The sprite</returns>
        public Tile this[int index]
        {
            get => tiles[index];
        }

        /// <summary>
        /// The number of tiles in the set
        /// </summary>
        public int Count => tiles.Length;

    }
}
