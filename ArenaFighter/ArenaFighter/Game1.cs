using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArenaFighter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public float aspectRatio;

        Player player;
        BasicEnemy enemy;
        Arena arena;
        public Vector3 cameraPosition = new Vector3(0.0f, 200.0f, 5000.0f);
        public Vector3 cameraTarget = Vector3.Zero;

        SpriteFont spriteFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            // TODO: use this.Content to load your game content here
            player = new Player(this);
            enemy = new BasicEnemy(this);
            arena = new Arena(this);

            spriteFont = Content.Load<SpriteFont>("text");

        }

        MouseState oldState = Mouse.GetState();

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            enemy.Update(gameTime);

            MouseState newState = Mouse.GetState();
            //TODO: Put mouse updates that need to happen here
            if (newState.LeftButton == ButtonState.Pressed)
            {
                float distanceTraveled = newState.Position.X - oldState.Position.X;
                player.rotateCamera(distanceTraveled);
            }
            oldState = newState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            player.Draw();
            enemy.Draw();
            arena.Draw();

            // Write text to the screen
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, "Battle of the Boxes", new Vector2(300, 20), Color.White);
            spriteBatch.End();

            // Dont touch this stuff. These reset some problems that come out of writing text on the screen
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            base.Draw(gameTime);
        }
    }
}
