using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dodgeball
{
    public class FieldLine
    {
        Game1 game;
        
        public BoundingRectangle Bounds;
        
        Texture2D texture;

        /// <summary>
        /// Creates a player
        /// </summary>
        /// <param name="game">The game this player belongs to</param>
        public FieldLine(Game1 game)
        {
            this.game = game;
        }

        /// <summary>
        /// sets the inital size and position of the field line.
        /// </summary>
        public void Initialize()
        {
            Bounds.Width = 25;
            Bounds.Height = game.GraphicsDevice.Viewport.Height;
            Bounds.X = game.GraphicsDevice.Viewport.Width/ 2;
            Bounds.Y = game.GraphicsDevice.Viewport.Height / 2 - Bounds.Height / 2;
        }

        /// <summary>
        /// loads the content related to the field line
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("pixel");
        }

        /// <summary>
        /// Draws the field line
        /// </summary>
        /// <param name="spriteBatch">
        /// The SpriteBatch to draw the FieldLine with.  This method should 
        /// be invoked between SpriteBatch.Begin() and SpriteBatch.End() calls.
        /// </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.Black);
        }
    }
}
