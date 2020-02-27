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
    /// <summary>
    /// An enum representing the states the player can be in
    /// </summary>
    enum State
    {
        South = 0,
        East = 1,
        West = 2,
        North = 3,
        Idle = 4,
    }

    public class Player
    {

        /// <summary>
        /// How quickly the animation should advance frames (1/8 second as milliseconds)
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// How quickly the player should move
        /// </summary>
        const float PLAYER_SPEED = 300;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int FRAME_WIDTH = 49;

        /// <summary>
        /// The hieght of the animation frames
        /// </summary>
        const int FRAME_HEIGHT = 64;

        /// <summary>
        /// The game object
        /// </summary>
        Game1 game;

        /// <summary>
        /// The Bounds for this player
        /// </summary>
        public BoundingRectangle Bounds;

        /// <summary>
        /// used for positioning the character sprite
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// The Texture for this player
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// Other variables for the animation of the player model
        /// </summary>
        State state;
        TimeSpan timer;
        int frame;
        SpriteFont font;

        /// <summary>
        /// Creates a player
        /// </summary>
        /// <param name="game">The game this player belongs to</param>
        public Player(Game1 game)
        {
            this.game = game;
            timer = new TimeSpan(0);
            state = State.Idle;
        }

        /// <summary>
        /// Sets the players initial size, bounds, and position on the screen.
        /// </summary>
        public void Initialize()
        {
            Bounds.Width = FRAME_WIDTH;
            Bounds.Height = FRAME_HEIGHT;
            Bounds.X = 0;
            Bounds.Y = game.GraphicsDevice.Viewport.Height / 2 - Bounds.Height / 2;
            position = new Vector2(Bounds.X, Bounds.Y);
        }

        /// <summary>
        /// loads the content related to the player
        /// </summary>
        /// <param name="content">The ContentManager to use</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("spritesheet");
            font = content.Load<SpriteFont>("defaultFont");
        }

        /// <summary>
        /// Updates the state of the player
        /// </summary>
        /// <param name="gameTime">The game's GameTime</param>
        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                // move up
                state = State.North;
                position.Y -= delta * PLAYER_SPEED;
                Bounds.Y -= delta * PLAYER_SPEED;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                // move down
                state = State.South;
                position.Y += delta * PLAYER_SPEED;
                Bounds.Y += delta * PLAYER_SPEED;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                // move left
                state = State.West;
                position.X -= delta * PLAYER_SPEED;
                Bounds.X -= delta * PLAYER_SPEED;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                // move right
                state = State.East;
                position.X += delta * PLAYER_SPEED;
                Bounds.X += delta * PLAYER_SPEED;
            }
            else
            {
                state = State.Idle;
            }

            //checks if player collides with walls of the viewport
            if (Bounds.Y < 0)
            {
                Bounds.Y = 0;
                position.Y = 0;
            }
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height - Bounds.Height)
            {
                Bounds.Y = game.GraphicsDevice.Viewport.Height - Bounds.Height;
                position.Y = game.GraphicsDevice.Viewport.Height - Bounds.Height;
            }
            if (Bounds.X < 0)
            {
                Bounds.X = 0;
                position.X = 0;
            }
            UpdateAnimation(gameTime);
        }

        /// <summary>
        /// Draws the player
        /// </summary>
        /// <param name="spriteBatch">
        /// The SpriteBatch to draw the player with.  This method should 
        /// be invoked between SpriteBatch.Begin() and SpriteBatch.End() calls.
        /// </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // determine the source rectagle of the sprite's current frame
            var source = new Rectangle(
                frame * FRAME_WIDTH, // X value 
                (int)state % 4 * FRAME_HEIGHT, // Y value
                FRAME_WIDTH, // Width 
                FRAME_HEIGHT // Height
                );

            // render the sprite
            spriteBatch.Draw(texture, position, source, Color.White);
        }

        /// <summary>
        /// updates the animation for the player, used within the Update() method.
        /// </summary>
        /// <param name="gameTime">the game's gameTime</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            // Update the player animation timer when the player is moving
            if (state != State.Idle) timer += gameTime.ElapsedGameTime;

            // Determine the frame should increase.  Using a while 
            // loop will accomodate the possiblity the animation should 
            // advance more than one frame.
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase by one frame
                frame++;
                // reduce the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            // Keep the frame within bounds (there are four frames)
            frame %= 4;
        }
    }
}
