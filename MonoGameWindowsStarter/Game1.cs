using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using PlatformLibrary;

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
        const int _ballNumber = 0;

        private int _lives;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        /// <summary>
        /// variables holding the balls, player, and field line(s) objects.
        /// </summary>
        Ball[] balls = new Ball[_ballNumber];
        Player player;
         
        /// <summary>
        /// Sound Effect for when a ball collides with the player sprite.
        /// </summary>
        SoundEffect playerHitSFX;
        SpriteFont spriteFont;

        
        List<Platform> platforms;
        AxisList world;

        Tileset tileset;
        Tilemap tilemap;

        Texture2D texture;

        public Random Random = new Random();

        

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            platforms = new List<Platform>();
            for(int i = 0; i < _ballNumber; i++)
            {
                balls[i] = new Ball(this);
            }
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
            texture = Content.Load<Texture2D>("pixel");
            foreach (Ball item in balls)
            {
                item.LoadContent(Content);
            }
            spriteFont = Content.Load<SpriteFont>("defaultFont");
            playerHitSFX = Content.Load<SoundEffect>("Hit_Player");

            // TODO: use this.Content to load your game content here
            var t = Content.Load<Texture2D>("spritesheet");
            sheet = new SpriteSheet(t, 21, 21, 3, 2);

            // Load the tilemap
            tilemap = Content.Load<Tilemap>("level1");

            
            // Create the player with the corresponding frames from the spritesheet
            var playerFrames = from index in Enumerable.Range(49, 60) select sheet[index];
            player = new Player(playerFrames);


            // Create the platforms
            //platforms.Add(new Platform(new BoundingRectangle(80, 300, 105, 21), sheet[1]));
            //platforms.Add(new Platform(new BoundingRectangle(280, 400, 84, 21), sheet[2]));
            //platforms.Add(new Platform(new BoundingRectangle(160, 200, 42, 21), sheet[3]));
            //platforms.Add(new Platform(new BoundingRectangle(0, 500, 2100, 21), sheet[1]));

            // Add the platforms to the axis list
            world = new AxisList();
            
            foreach (Platform platform in platforms)
            {
                world.AddGameObject(platform);
            }

            tileset = Content.Load<Tileset>("tiledspritesheet");
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
                player.CheckForObjectCollision(tilemap.Objects);
                player.Update(gameTime);

                var platformQuery = world.QueryRange(player.Bounds.X, player.Bounds.X + player.Bounds.Width);
                //player.CheckForPlatformCollision(platformQuery);

                

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

                base.Update(gameTime);


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

            // Calculate and apply the world/view transform
            var offset = new Vector2(400, 400) - player.Position;
            var t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);

            // Draw the tilemap
            tilemap.Draw(spriteBatch);

            // Draw the platforms 
            var platformQuery = world.QueryRange(player.Position.X - 221, player.Position.X + 400);
            foreach (Platform platform in platformQuery)
            {
                platform.Draw(spriteBatch);
            }
            Debug.WriteLine($"{platformQuery.Count()} Platforms rendered");

            // Draw the balls
            foreach (Ball item in balls)
            {
                item.Draw(spriteBatch);
            }

            // Draw the player
            player.Draw(spriteBatch);

            // Draw an arbitrary range of sprites
            for (var i = 17; i < 30; i++)
            {
                sheet[i].Draw(spriteBatch, new Vector2(i * 25, 25), Color.White);
            }

            foreach(TilemapObject item in tilemap.Objects)
            {
                Rectangle rect = new BoundingRectangle(item.X, item.Y, item.Width, item.Height);
                spriteBatch.Draw(texture, rect, Color.Red);
            }

            var textPosition = new Vector2(player.Position.X - 300, player.Position.Y - 375);
            //checks if the game is still active
            if (_lives != 0)
            {
                spriteBatch.DrawString(spriteFont, "Lives: " + _lives.ToString(), textPosition, Color.White);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, "Game Over, please close the game.", textPosition, Color.White);
            }

            spriteBatch.End();

            

            base.Draw(gameTime);
        }
    }
}
