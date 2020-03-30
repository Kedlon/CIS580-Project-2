using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PlatformLibrary
{
    public class TilemapObject
    {
        public string Type;

        public float X;

        public float Y;

        public float Width;

        public float Height;

        public TilemapObject(string type, float x, float y, float width, float height)
        {
            Type = type;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
