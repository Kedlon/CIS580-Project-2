﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Dodgeball
{
    public class Ball
    {
        /// <summary>
        /// The game object
        /// </summary>
        Game1 game;

        /// <summary>
        /// The Texture for the ball
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// Sound Effect for the ball bouncing against the screen bounds.
        /// </summary>
        SoundEffect bounceSFX;

        /// <summary>
        /// The Bounds for the ball
        /// </summary>
        public BoundingCircle Bounds;

        /// <summary>
        /// The Velocity of the ball
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Initialize the ball
        /// </summary>
        /// <param name="game">The game that the ball belongs to.</param>
        public Ball(Game1 game)
        {
            this.game = game;
        }

        /// <summary>
        /// sets initial size of the ball, as well as the position and velocity.
        /// </summary>
        public void Initialize()
        {
            // Set the ball's radius
            Bounds.Radius = 20;

            // position the ball in the center of the screen
            Bounds.X = game.GraphicsDevice.Viewport.Width / 2;
            Bounds.Y = game.GraphicsDevice.Viewport.Height / 2;

            // give the ball a random velocity
            Velocity = new Vector2(
                (float)game.Random.NextDouble(),
                (float)game.Random.NextDouble()
            );
            Velocity.Normalize();
        }

        /// <summary>
        /// loads the content related to the ball
        /// </summary>
        /// <param name="content">The ContentManager to use.</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("red_ball");
            bounceSFX = content.Load<SoundEffect>("Hit_Wall");
        }

        /// <summary>
        /// updates the state of the ball
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            var viewport = game.GraphicsDevice.Viewport;

            Bounds.Center += 0.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Velocity;

            // Check for wall collisions
            if (Bounds.Center.Y < Bounds.Radius)
            {
                Velocity.Y *= -1;
                float delta = Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
                bounceSFX.Play();
            }

            if (Bounds.Center.Y > viewport.Height - Bounds.Radius)
            {
                Velocity.Y *= -1;
                float delta = viewport.Height - Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
                bounceSFX.Play();
            }
            if (Bounds.X < 0)
            {
                Velocity.X *= -1;
                float delta = Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
                bounceSFX.Play();
            }
            if (Bounds.X > viewport.Width - Bounds.Radius)
            {
                Velocity.X *= -1;
                float delta = viewport.Width - Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
                bounceSFX.Play();
            }
        }

        /// <summary>
        /// Draws the ball
        /// </summary>
        /// <param name="spriteBatch">
        /// The SpriteBatch to use to draw the ball.  
        /// This method should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End() calls.
        /// </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
