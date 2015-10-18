using BattleShip.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace BattleShip
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BattleShipGame : Game
    {

        public const int TILE_SIZE = 50;
        public const int HUD_HEIGHT = 100;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        SpriteFont HudFont;
        SpriteFont StartFont;

        StartScreen StartScreen;
        SettingScreen SettingScreen;
        GamePlayScreen GamePlayScreen;
        EndScreen EndScreen;

        
        MouseState PrevMouseState;
        
        public enum GamesState { Startscreen, Gameplay, Settings, EndScreen}
        GamesState CurrentState = GamesState.Startscreen;
        
        int GameAreaWidth;
        int GameAreaHeight;       
        
        

        public BattleShipGame()
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
            IsMouseVisible = true;
            // TODO: Add your initialization logic here
            SetWindowSize(10, 10);            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadFonts();
            CreateScreens();
        }
        
        private void CreateScreens()
        {
            StartScreen = new StartScreen(this, StartFont);
            SettingScreen = new SettingScreen(this, StartFont);
            GamePlayScreen = new GamePlayScreen(this, HudFont);
            EndScreen = new EndScreen(this, StartFont);
        }

        private void StartGame()
        {
            SetWindowSize(SettingScreen.TilesWidth, SettingScreen.TilesHeight);

            GamePlayScreen.StartGame(SettingScreen.TilesWidth, SettingScreen.NumberOfShips);
            
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            switch (CurrentState)
            {
                case GamesState.Startscreen:
                    StartScreen.Update(mouseState, PrevMouseState);
                    break;
                case GamesState.Gameplay:
                    GamePlayScreen.Update(gameTime, mouseState, PrevMouseState);
                    break;
                case GamesState.Settings:
                    SettingScreen.Update(mouseState, PrevMouseState);
                    break;
                case GamesState.EndScreen:
                    EndScreen.Update();
                    break;
                default:
                    break;
            }
            PrevMouseState = mouseState;
            base.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
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
            switch (CurrentState)
            {
                case GamesState.Startscreen:
                    StartScreen.Draw(spriteBatch);
                    break;
                case GamesState.Gameplay:
                    GamePlayScreen.Draw(spriteBatch);                    
                    break;
                case GamesState.Settings:
                    SettingScreen.Draw(spriteBatch);
                    break;
                case GamesState.EndScreen:
                    EndScreen.Draw(spriteBatch, GamePlayScreen.Bombs);
                    break;
                default:
                    break;
            }
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        
        private void LoadFonts()
        {
            HudFont = Content.Load<SpriteFont>(@"HUDFont");
            StartFont = Content.Load<SpriteFont>(@"StartGameFont");
        }

        protected void SetWindowSize(int Width, int Height)
        {
            GameAreaWidth = Width * TILE_SIZE;
            GameAreaHeight = Height * TILE_SIZE;
            graphics.PreferredBackBufferWidth = GameAreaWidth;
            graphics.PreferredBackBufferHeight = GameAreaHeight + HUD_HEIGHT;
            graphics.ApplyChanges();
        }
        
        public void SetScreen(GamesState GameState)
        {
            CurrentState = GameState;
            if (CurrentState == GamesState.Gameplay)
            {
                StartGame();
            }
        }
    }
}
