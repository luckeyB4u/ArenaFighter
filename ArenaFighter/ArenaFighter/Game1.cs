using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ArenaFighter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphicsManager;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        public float aspectRatio;

        Player player;
        Healthbar playerHealthbar;
        Zombie enemy;
        Arena arena;
        Boolean gameOver;

        public Vector3 cameraPosition;
        public Vector3 cameraTarget;

        MouseState oldState = Mouse.GetState();

        public Game1()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            graphicsManager.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("text");
            aspectRatio = graphicsManager.GraphicsDevice.Viewport.AspectRatio;

            // Load your game content here
            player = new Player(this);
            playerHealthbar = new Healthbar(this, graphicsManager.GraphicsDevice, GameConstants.PLAYER_INITIAL_HEALTH, GameConstants.PLAYER_HEALTHBAR_INITIAL_POSITION);
            enemy = new Zombie(this);
            arena = new Arena(this);
            gameOver = false;

            // Sets up global camera variables
            cameraPosition = Vector3.Zero;
            cameraTarget = Vector3.Zero;

            // Initializes mouse state
            oldState = Mouse.GetState();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Main game loop
            if(!gameOver)
            {
                player.Update(gameTime, enemy);
                playerHealthbar.changeHealth(player.getHealth());

                enemy.Update(gameTime);

                MouseState newState = Mouse.GetState();
                // Put mouse update code here
                if (newState.LeftButton == ButtonState.Pressed)
                {
                    float distanceTraveled = newState.Position.X - oldState.Position.X;
                    player.rotateCamera(distanceTraveled);
                }
                oldState = newState;

                if (player.getHealth() == 0)
                {
                    gameOver = true;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            // Put drawing code here
            player.Draw();
            enemy.Draw();
            arena.Draw();
            playerHealthbar.Draw(spriteBatch);

            // Write text to the screen
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, "Battle of the Frozen Aliens", new Vector2(280, 20), Color.Black);
            if(gameOver)
            {
                spriteBatch.DrawString(spriteFont, "Game over!", new Vector2(345, 150), Color.Black);
            }
            spriteBatch.End(); 

            // Don't touch this stuff. These reset some problems that come from writing text on the screen
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            base.Draw(gameTime);
        }
    }
}
