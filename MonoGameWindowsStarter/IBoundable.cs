using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dodgeball
{
    public interface IBoundable
    {
        /// <summary>
        /// An interface defining game objects with bounds
        /// </summary>
        BoundingRectangle Bounds { get; }
    }
}
