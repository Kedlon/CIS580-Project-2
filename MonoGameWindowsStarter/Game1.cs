using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;

namespace Dodgeball
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        /// <summary>
        /// number of balls that will be in the game.
        /// </summary>
        const int _ballNumber = 3;

        private int _lives;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// variables holding the balls, player, and field line(s) objects.
        /// </summary>
        Ball[] balls = new Ball[_ballNumber];
        Player player;
        FieldLine centerLine;

        /// <summary>
        /// Sound Effect for when a ball collides with the player sprite.
        /// </summary>
        SoundEffect playerHitSFX;

        SpriteFont spriteFont;

        public Random Random = new Random();

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            player = new Player(this);
            for(int i = 0; i < _ballNumber; i++)
            {
                balls[i] = new Ball(this);
            }
            centerLine = new FieldLine(this);
            _lives = 3;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            foreach(Ball item in balls)
            {
                item.Initialize();
            }
            player.Initialize();
            centerLine.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (Ball item in balls)
            {
                item.LoadContent(Content);
            }
            player.LoadContent(Content);
            centerLine.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("defaultFont");
            playerHitSFX = Content.Load<SoundEffect>("Hit_Player");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            newKeyboardState = Keyboard.GetState();
            int newLifeCount = _lives;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (newKeyboardState.IsKeyDown(Keys.Escape))
                Exit();
            
            //loop that keeps the game going when lives are still available.
            if(_lives != 0)
            {
                player.Update(gameTime);

                //update calls for each ball
                foreach (Ball item in balls)
                {
                    item.Update(gameTime);

                    if (player.Bounds.CollidesWith(item.Bounds))
                    {
                        item.Velocity.X *= -1;
                        var delta = (player.Bounds.X + player.Bounds.Width) - (item.Bounds.X - item.Bounds.Radius);
                        item.Bounds.X += 2 * delta;
                        playerHitSFX.Play();
                        _lives--;
                    }
                }

                //checks if the player is trying to pass the center line onto the other side.
                if (player.Bounds.CollidesWith(centerLine.Bounds))
                {
                    player.Bounds.X = (graphics.GraphicsDevice.Viewport.Width / 2) - (centerLine.Bounds.Width * 2);
                    player.position.X = (graphics.GraphicsDevice.Viewport.Width / 2) - (centerLine.Bounds.Width * 2);
                }
            }
            //if the player loses
            else
            {
                foreach (Ball item in balls)
                {
                    item.Velocity = Vector2.Zero;
                }
            }
            

            oldKeyboardState = newKeyboardState;
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (Ball item in balls)
            {
                item.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            centerLine.Draw(spriteBatch);
            //checks if the game is still active
            if(_lives != 0)
            {
                spriteBatch.DrawString(spriteFont, "Lives: " + _lives.ToString(), Vector2.Zero, Color.White);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, "Game Over, please close the game.", Vector2.Zero, Color.White);
            }

            spriteBatch.End();

            

            base.Draw(gameTime);
        }
    }
}
