using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Dodgeball
{

    /// <summary>
    /// An enumeration of possible player animation states
    /// </summary>
    enum PlayerAnimState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    /// <summary>
    /// An enumeration of possible player veritcal movement states
    /// </summary>
    enum VerticalMovementState
    {
        OnGround,
        Jumping,
        Falling
    }

    public class Player
    {

        /// <summary>
        /// How quickly the animation should advance frames (1/8 second as milliseconds)
        /// </summary>
        const int FRAME_RATE = 124;

        /// <summary>
        /// The duration of a player's jump, in milliseconds
        /// </summary>
        const int JUMP_TIME = 500;

        /// <summary>
        /// How quickly the player should move
        /// </summary>
        const int PLAYER_SPEED = 3;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int FRAME_WIDTH = 49;

        /// <summary>
        /// The hieght of the animation frames
        /// </summary>
        const int FRAME_HEIGHT = 64;

        /// <summary>
        /// The currently rendered frame
        /// </summary>
        int currentFrame = 0;

        /// <summary>
        /// A timer for jumping
        /// </summary>
        TimeSpan jumpTimer;

        /// <summary>
        /// The player sprite frames
        /// </summary>
        Sprite[] frames;

        /// <summary>
        /// The player's animation state
        /// </summary>
        PlayerAnimState animationState = PlayerAnimState.Idle;

        /// <summary>
        /// The player's vertical movement state
        /// </summary>
        VerticalMovementState verticalState = VerticalMovementState.OnGround;

        // A timer for animations
        TimeSpan animationTimer;

        /// <summary>
        /// The currently applied SpriteEffects
        /// </summary>
        SpriteEffects spriteEffects = SpriteEffects.None;

        /// <summary>
        /// The color of the sprite
        /// </summary>
        Color color = Color.White;

        /// <summary>
        /// The origin of the sprite (centered on its feet)
        /// </summary>
        Vector2 origin = new Vector2(10, 21);

        /// <summary>
        /// Gets and sets the position of the player on-screen
        /// </summary>
        public Vector2 Position = new Vector2(200, 490);

        /// <summary>
        /// The Bounds for this player
        /// </summary>
        public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.8f * origin, 38, 41);

        
        

        /// <summary>
        /// Creates a player
        /// </summary>
        /// <param name="game">The game this player belongs to</param>
        public Player(IEnumerable<Sprite> frames)
        {
            this.frames = frames.ToArray();
            animationState = PlayerAnimState.WalkingLeft;
        }

        /// <summary>
        /// Updates the state of the player, as well as movement, physics, and other aspects of the player.
        /// </summary>
        /// <param name="gameTime">The game's GameTime</param>
        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            UpdateVertical(gameTime, keyboardState);
            UpdateHorizontal(gameTime, keyboardState);
            ApplyAnimations(gameTime);
        }

        /// <summary>
        /// Render the player sprite.  Should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if VISUAL_DEBUG 
            VisualDebugging.DrawRectangle(spriteBatch, Bounds, Color.Red);
#endif
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }


        /// <summary>
        /// checks and updates the player's vertical movement.
        /// NOTE: MEANT TO BE USED IN THE UPDATE METHOD ONLY.
        /// </summary>
        /// <param name="gameTime">the current game time.</param>
        /// <param name="keyboard">the state of the keyboard.</param>
        private void UpdateVertical(GameTime gameTime, KeyboardState keyboard)
        {
            switch (verticalState)
            {
                case VerticalMovementState.OnGround:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        verticalState = VerticalMovementState.Jumping;
                        jumpTimer = new TimeSpan(0);
                    }
                    break;
                case VerticalMovementState.Jumping:
                    jumpTimer += gameTime.ElapsedGameTime;
                    // Simple jumping with platformer physics
                    Position.Y -= (250 / (float)jumpTimer.TotalMilliseconds);
                    if (jumpTimer.TotalMilliseconds >= JUMP_TIME) verticalState = VerticalMovementState.Falling;
                    break;
                case VerticalMovementState.Falling:
                    Position.Y += PLAYER_SPEED;
                    // TODO: This needs to be replaced with collision logic
                    if (Position.Y > 500)
                    {
                        Position.Y = 500;
                    }
                    break;
            }
        }

        /// <summary>
        /// checks and updates the player's horizontal movement.
        /// NOTE: MEANT TO BE USED IN THE UPDATE METHOD ONLY.
        /// </summary>
        /// <param name="gameTime">the current game time.</param>
        /// <param name="keyboard">the state of the keyboard.</param>
        private void UpdateHorizontal(GameTime gameTime, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Left))
            {
                if (verticalState == VerticalMovementState.Jumping || verticalState == VerticalMovementState.Falling)
                    animationState = PlayerAnimState.JumpingLeft;
                else animationState = PlayerAnimState.WalkingLeft;
                Position.X -= PLAYER_SPEED;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                if (verticalState == VerticalMovementState.Jumping || verticalState == VerticalMovementState.Falling)
                    animationState = PlayerAnimState.JumpingRight;
                else animationState = PlayerAnimState.WalkingRight;
                Position.X += PLAYER_SPEED;
            }
            else
            {
                animationState = PlayerAnimState.Idle;
            }
        }

        /// <summary>
        /// Applies animations to the player
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void ApplyAnimations(GameTime gameTime)
        {
            switch (animationState)
            {
                case PlayerAnimState.Idle:
                    currentFrame = 0;
                    animationTimer = new TimeSpan(0);
                    break;

                case PlayerAnimState.JumpingLeft:
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    currentFrame = 7;
                    break;

                case PlayerAnimState.JumpingRight:
                    spriteEffects = SpriteEffects.None;
                    currentFrame = 7;
                    break;

                case PlayerAnimState.WalkingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // Walking frames are 9 & 10
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;

                case PlayerAnimState.WalkingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    // Walking frames are 9 & 10
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                    break;

            }
        }

        public void CheckForPlatformCollision(IEnumerable<IBoundable> platforms)
        {
            Debug.WriteLine($"Checking collisions against {platforms.Count()} platforms");
            if (verticalState != VerticalMovementState.Jumping)
            {
                verticalState = VerticalMovementState.Falling;
                foreach (Platform platform in platforms)
                {
                    if (Bounds.CollidesWith(platform.Bounds))
                    {
                        Position.Y = platform.Bounds.Y - 1;
                        verticalState = VerticalMovementState.OnGround;
                    }
                }
            }
        }
    }
}
