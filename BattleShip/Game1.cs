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
        const int HUD_HEIGHT = 100;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private static Texture2D waterTileTexture;
        public static Texture2D WaterTileTexture
        {
            get { return waterTileTexture; }
        }
        private static Texture2D destroyerTexture;
        public static Texture2D DestroyerTexture
        {
            get { return destroyerTexture; }
        }
        private static Texture2D submarineTexture;
        public static Texture2D SubmarineTexture
        {
            get { return submarineTexture; }
        }
        private static Texture2D battleshipTexture;
        public static Texture2D BattleshipTexture
        {
            get { return battleshipTexture; }
        }
        private static Texture2D hangarshipTexture;
        public static Texture2D HangarshipTexture
        {
            get { return hangarshipTexture; }
        }
        private static Texture2D splash;
        public static Texture2D Splash
        {
            get { return splash; }
        }
        private static Texture2D explosion;
        public static Texture2D Explosion
        {
            get { return explosion; }
        }
        private static Texture2D radar_Sheet;
        public static Texture2D Radar_Sheet
        {
            get { return radar_Sheet; }
        }
        private static SpriteFont hudFont;
        public static SpriteFont HudFont
        {
            get { return hudFont; }
        }
        private static SpriteFont startFont;
        public static SpriteFont StartFont
        {
            get { return startFont; }
        }
        Radar Radar = new Radar();
        Clock Clock = new Clock();
        Random rnd;
        enum GamesState { Startscreen, Gameplay, Settings, EndScreen}
        GamesState CurrentState = GamesState.Startscreen;
        int TilesWidth = 10;
        int TilesHeight = 10;
        int Init = 0;
        string Start = "START";
        string Settings = "SETTINGS";
        string Back = "BACK";
        string LeftArrow = "<";
        string RightArrow = ">";
        GameArea gameArea;
        MouseState PrevMouseState = Mouse.GetState();

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
            IsMouseVisible = true;
            // TODO: Add your initialization logic here
            rnd = new Random();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameArea = new GameArea(0, 0, TilesWidth, TilesHeight, spriteBatch, rnd);
            graphics.PreferredBackBufferWidth = gameArea.Width;
            graphics.PreferredBackBufferHeight = gameArea.Height + HUD_HEIGHT;
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
            LoadPictures();
            LoadFonts();
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
            MouseState mouseState = Mouse.GetState();
            switch (CurrentState)
            {
                case GamesState.Startscreen:
                    base.Update(gameTime);
                    StartGame(mouseState, PrevMouseState);
                    break;
                case GamesState.Gameplay:
                    if (Init == 1)
                    {
                        Initialize();
                        Init = 0;
                    }
                    
                    gameArea.HandleMouseInput(mouseState, PrevMouseState);
                    if( gameArea.EndGame())
                    {
                        CurrentState = GamesState.EndScreen;
                    }
                    base.Update(gameTime);
                    break;
                case GamesState.Settings:
                    SettingMenu(mouseState, PrevMouseState);
                    base.Update(gameTime);
                    break;
                case GamesState.EndScreen:
                    
                    base.Update(gameTime);
                    break;
                default:
                    break;
            }
            PrevMouseState = mouseState;
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
            switch (CurrentState)
            {
                case GamesState.Startscreen:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    DrawStartGame();
                    spriteBatch.End();
                    break;
                case GamesState.Gameplay:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    gameArea.Draw();
                    Radar.Draw(spriteBatch, radar_Sheet, new Rectangle(Window.ClientBounds.Width / 2 - 50, Window.ClientBounds.Height - 100, 100, 100), Clock.GetRotationForRadar());
                    spriteBatch.End();
                    break;
                case GamesState.Settings:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    DrawSettingMenu();
                    spriteBatch.End();
                    break;
                case GamesState.EndScreen:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    DrawEndGame();
                    spriteBatch.End();
                    break;
                default:
                    break;
            }
            
           
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        private void LoadPictures()
        {
            waterTileTexture = Content.Load<Texture2D>(@"watertile");
            destroyerTexture = Content.Load<Texture2D>(@"ship2x2");
            submarineTexture = Content.Load<Texture2D>(@"ship3x3");
            battleshipTexture = Content.Load<Texture2D>(@"ship4x4");
            hangarshipTexture = Content.Load<Texture2D>(@"ship5x5");
            splash = Content.Load<Texture2D>(@"splash");
            explosion = Content.Load<Texture2D>(@"explosion");
            radar_Sheet = Content.Load<Texture2D>(@"radar_spritesheet");

        }
        private void LoadFonts()
        {
            hudFont = Content.Load<SpriteFont>(@"HUDFont");
            startFont = Content.Load<SpriteFont>(@"StartGameFont");
        }

        protected void StartGame(MouseState mouseState, MouseState prevMouseState)
        {
            Vector2 StartVector = startFont.MeasureString(Start);
            Rectangle StartRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)StartVector.X /2, Window.ClientBounds.Height / 2 - (int)StartVector.Y / 2, (int)StartVector.X, (int)StartVector.Y);

            Vector2 SettingsVector = startFont.MeasureString(Settings);
            Rectangle SettingsRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)SettingsVector.X / 2, Window.ClientBounds.Height / 2 + (int)SettingsVector.Y, (int)SettingsVector.X, (int)SettingsVector.Y);
            if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed && StartRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Gameplay;
                Init = 1;
            }
            if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Settings;
            }
        }
        protected void SettingMenu(MouseState mouseState, MouseState prevMouseState)
        {
            Vector2 BackLen = startFont.MeasureString(Back);
            Rectangle SettingsRectangle = new Rectangle(0, 500 - (int)BackLen.Y + HUD_HEIGHT, (int)BackLen.X, (int)BackLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Startscreen;
            }
            HandleLeftArrow(mouseState, PrevMouseState);
            HandleRightArrow(mouseState, PrevMouseState);
        }
        protected void HandleLeftArrow(MouseState mouseState, MouseState prevMouseState)
        {
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = startFont.MeasureString(BoardSize);
            Vector2 LeftArrowLen = startFont.MeasureString(LeftArrow);
            Rectangle LeftArrowRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)BoardSizeLen.X / 2 - (int)LeftArrowLen.X * 2, Window.ClientBounds.Height / 2 - (int)BoardSizeLen.Y / 2, (int)LeftArrowLen.X, (int)LeftArrowLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed && LeftArrowRectangle.Contains(mouseState.Position))
            {
                //Problem
                if (TilesWidth > 8 && TilesHeight > 8 && TilesWidth <= 14 && TilesHeight <= 14)
                {
                    TilesWidth--;
                    TilesHeight--;
                }
            }
        }
        protected void HandleRightArrow(MouseState mouseState, MouseState prevMouseState)
        {
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = startFont.MeasureString(BoardSize);
            Vector2 RightArrowLen = startFont.MeasureString(LeftArrow);
            Rectangle RightArrowRectangle = new Rectangle(Window.ClientBounds.Width / 2 + (int)BoardSizeLen.X / 2 + (int)RightArrowLen.X, Window.ClientBounds.Height / 2 - (int)BoardSizeLen.Y / 2, (int)RightArrowLen.X, (int)RightArrowLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed && RightArrowRectangle.Contains(mouseState.Position))
            {
                if (TilesWidth >= 8 && TilesHeight >= 8 && TilesWidth < 14 && TilesHeight < 14)
                {
                    TilesWidth++;
                    TilesHeight++;
                }
            }
        }

        protected void DrawSettingMenu()
        {
            Vector2 BackLen = startFont.MeasureString(Back);
            spriteBatch.DrawString(startFont, Back, new Vector2(0, 500 - BackLen.Y + 100), Color.White);
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = startFont.MeasureString(BoardSize);
            spriteBatch.DrawString(startFont, BoardSize, new Vector2(Window.ClientBounds.Width / 2 - BoardSizeLen.X / 2, Window.ClientBounds.Height / 2 - BoardSizeLen.Y / 2), Color.White);
            Vector2 LeftArrowLen = startFont.MeasureString(LeftArrow);
            spriteBatch.DrawString(startFont, LeftArrow, new Vector2(Window.ClientBounds.Width / 2 - BoardSizeLen.X / 2 - LeftArrowLen.X * 2, Window.ClientBounds.Height / 2 - BoardSizeLen.Y / 2), Color.White);
            Vector2 RightArrowLen = startFont.MeasureString(RightArrow);
            spriteBatch.DrawString(startFont, RightArrow, new Vector2(Window.ClientBounds.Width / 2 + BoardSizeLen.X / 2 + RightArrowLen.X, Window.ClientBounds.Height / 2 - BoardSizeLen.Y / 2), Color.White);
        }

        protected void DrawStartGame()
        {
            
            Vector2 StartLen = startFont.MeasureString(Start);
            spriteBatch.DrawString(startFont, Start, new Vector2(Window.ClientBounds.Width / 2 - StartLen.X / 2, Window.ClientBounds.Height / 2 - StartLen.Y / 2), Color.White);
            Vector2 SettingsLen = startFont.MeasureString(Settings);
            spriteBatch.DrawString(startFont, Settings, new Vector2(Window.ClientBounds.Width / 2 - SettingsLen.X / 2, Window.ClientBounds.Height / 2 + SettingsLen.Y), Color.White);
        }

        protected void DrawEndGame()
        {
            string Ended = "You dropped \n" + gameArea.Bombs + " Bombs";
            Vector2 EndedLen = startFont.MeasureString(Ended);
            spriteBatch.DrawString(startFont, Ended, new Vector2(Window.ClientBounds.Width / 2 - EndedLen.X / 2, Window.ClientBounds.Height / 2 - EndedLen.Y / 2), Color.White);
              
        }
    }
}
