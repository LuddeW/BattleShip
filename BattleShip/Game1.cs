using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BattleShip
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        public const int TILE_SIZE = 50;
        public const int TILES_HORIZONTAL = 10;
        public const int TILES_VERTICAL = 10;

        GraphicsDeviceManager graphics = null;
        Resources resources = null;
        SpriteBatch spriteBatch;
        Radar Radar = new Radar();
        Clock Clock = new Clock();
        MouseState prevMouseState;
        GameArea gameArea = null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            resources = new Resources(Content);
            gameArea = new GameArea(resources, 0, 0);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = gameArea.Width();
            graphics.PreferredBackBufferHeight = gameArea.Heght() + 100;
            graphics.ApplyChanges();

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
            resources.LoadPictures();
            resources.LoadFonts();
            gameArea.Create();
            // TODO: use this.Content to load your game content here
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
            HandleMouseInput();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Clock.AddTime((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            gameArea.Draw(spriteBatch);
            Radar.Draw(spriteBatch, resources.Radar_Sheet, new Rectangle(Window.ClientBounds.Width / 2 - TILE_SIZE, Window.ClientBounds.Height - 100, 100, 100), Clock.GetRotationForRadar());
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void HandleMouseInput()
        {
            MouseState mouseState = Mouse.GetState();
            gameArea.HandleMouseInput(mouseState, prevMouseState);
            prevMouseState = mouseState;
        }
    }
}
