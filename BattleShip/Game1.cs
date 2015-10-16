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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D WaterTileTexture;
        Texture2D DestroyerTexture;
        Texture2D SubmarineTexture;
        Texture2D BattleshipTexture;
        Texture2D HangarshipTexture;
        Texture2D Splash;
        Texture2D Explosion;
        Texture2D Radar_Sheet;
        SpriteFont HudFont;
        SpriteFont StartFont;
        Tile LastClickedTile = null;
        Tile[,] Tiles;
        Ship[] Ships;
        Radar Radar = new Radar();
        Clock Clock = new Clock();
        MouseState PrevMouseState;
        Random rnd;
        enum GamesState { Startscreen, Gameplay, Settings, EndScreen}
        GamesState CurrentState = GamesState.Startscreen;
        int Bombs = 0;
        int TilesWidth = 20;
        int TilesHeight = 10;
        Rectangle GameAreaRectangle = new Rectangle();
        int NumberOfShipsLeft = 5;
        string Start = "START";
        string Settings = "SETTINGS";
        string Back = "BACK";

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
            GameAreaRectangle.X = 0;
            GameAreaRectangle.Y = 0;
            GameAreaRectangle.Width = TilesWidth * TILE_SIZE;
            GameAreaRectangle.Height = TilesHeight * TILE_SIZE;
            graphics.PreferredBackBufferWidth = GameAreaRectangle.Width;
            graphics.PreferredBackBufferHeight = GameAreaRectangle.Height + 100;
            graphics.ApplyChanges();
            rnd = new Random();

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
            LoadPictures();
            LoadFonts();
            CreatTileArray();
            CreateShips();
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
            switch (CurrentState)
            {
                case GamesState.Startscreen:
                    base.Update(gameTime);
                    //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                    //    CurrentState = GamesState.Gameplay;
                    StartGame();
                    break;
                case GamesState.Gameplay:
                    HandleMouseInput();
                    EndGame();
                    ShipDeathCounter();
                    base.Update(gameTime);
                    break;
                case GamesState.Settings:
                    SettingMenu();
                    base.Update(gameTime);
                    break;
                case GamesState.EndScreen:
                    
                    base.Update(gameTime);
                    break;
                default:
                    break;
            }
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
                    DrawSettings();
                    spriteBatch.End();
                    break;
                case GamesState.Gameplay:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Begin();
                    DrawWaterTiles();
                    DrawShips();
                    DrawTileStatus();
                    DrawHud();
                    Radar.Draw(spriteBatch, Radar_Sheet, new Rectangle(Window.ClientBounds.Width / 2 - 50, Window.ClientBounds.Height - 100, 100, 100), Clock.GetRotationForRadar());
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
        protected void DrawWaterTiles()
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    Tiles[i, k].DrawWater(spriteBatch);
                }
            }
        }
        protected void CreatTileArray()
        {
            Tiles = new Tile[TilesWidth, TilesHeight];
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    Tiles[i, k] = new Tile(new Rectangle(i * TILE_SIZE, k * TILE_SIZE, 1 * TILE_SIZE, 1 * TILE_SIZE), WaterTileTexture, Explosion, Splash);
                }
            }
        }
        private void LoadPictures()
        {
            WaterTileTexture = Content.Load<Texture2D>(@"watertile");
            DestroyerTexture = Content.Load<Texture2D>(@"ship2x2");
            SubmarineTexture = Content.Load<Texture2D>(@"ship3x3");
            BattleshipTexture = Content.Load<Texture2D>(@"ship4x4");
            HangarshipTexture = Content.Load<Texture2D>(@"ship5x5");
            Splash = Content.Load<Texture2D>(@"splash");
            Explosion = Content.Load<Texture2D>(@"explosion");
            Radar_Sheet = Content.Load<Texture2D>(@"radar_spritesheet");

        }
        private void LoadFonts()
        {
            HudFont = Content.Load<SpriteFont>(@"HUDFont");
            StartFont = Content.Load<SpriteFont>(@"StartGameFont");
        }
        private void CreateShips()
        {
            
            Texture2D[] ConfigShip = new Texture2D[]
            {
                DestroyerTexture, SubmarineTexture, BattleshipTexture, HangarshipTexture, DestroyerTexture
            };
            Ships = new Ship[ConfigShip.Length];
            for (int i = 0; i < ConfigShip.Length; i++)
            {
                bool FoundPosition = false;
                bool TempVertical = RandomVertical();
                Point Point = new Point();
                while (!FoundPosition)
                {
                    Point.X = RandomTile(false);
                    Point.Y = RandomTile(true);
                    FoundPosition = VerifyShipPosition(Point, ConfigShip[i].Height/TILE_SIZE, TempVertical);
                }
                
                Rectangle TempRectangle = new Rectangle(Point.X * TILE_SIZE, Point.Y * TILE_SIZE, ConfigShip[i].Width, ConfigShip[i].Height);
                
                Ship ship = new Ship(TempRectangle, ConfigShip[i], TempVertical);
                Ships.SetValue(ship, i);
                AddShipToTiles(ship, Point, TempVertical, ConfigShip[i].Height / TILE_SIZE);
            }
        }

        private void AddShipToTiles (Ship ship, Point Point, bool Vertical, int ShipTiles)
        {
            if (Vertical)
            {
                for (int i = 0; i < ShipTiles; i++)
                {
                    Tiles[Point.X, Point.Y + i].Ship = ship;
                }
            }
            else
            {
                for (int i = 0; i < ShipTiles; i++)
                {
                    Tiles[Point.X - ShipTiles + 1 + i, Point.Y].Ship = ship;
                }
            }
        }

        private void DrawShips()
        {
            for (int i = 0; i < Ships.Length; i++)
            {                
                    Ships[i].Draw(spriteBatch);   
            }
        }
        private void DrawHud()
        {
            string Bomb = "Bombs dropped:" + Bombs;
            spriteBatch.DrawString(HudFont, Bomb, new Vector2(5, 505), Color.White);
        }

        private void HandleMouseInput()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && GameAreaRectangle.Contains(mouseState.Position))
            {
                LastClickedTile = GetTileWithPointInside(mouseState.Position);
                if(LastClickedTile!=null)
                {
                    if (LastClickedTile.HitTile())
                    {
                        Bombs++;
                    }                             
                }
            }
            PrevMouseState = mouseState;
        }

        private Tile GetTileWithPointInside(Point Point)
        {
            if (GameAreaRectangle.Contains(Point))
            {
                int tileX = (TilesWidth - 1) - (GameAreaRectangle.Right - Point.X) / TILE_SIZE;
                int tileY = (TilesHeight - 1) - (GameAreaRectangle.Bottom - Point.Y) / TILE_SIZE;
                return Tiles[tileX, tileY];
            }
            return null;
        }

        private void DrawTileStatus()
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    if (Tiles[i, k].Explos == true && Tiles[i, k].Clicked == true)
                    {
                        Tiles[i, k].DrawExplosion(spriteBatch);
                    }
                    else if (Tiles[i, k].Explos == false)
                    {
                        if (LastClickedTile != null && Tiles[i, k].Clicked == true)
                        {
                            Tiles[i, k].DrawSplash(spriteBatch);
                        }
                    }
                }
            }         
        }
        private int RandomTile(bool vertical)
        {
            int tiles = TilesWidth;
            if(vertical)
            {
                tiles = TilesHeight;
            }
            return rnd.Next(0, tiles);
        }

        public bool RandomVertical()
        {
            int Rndom = rnd.Next(0, 99);
            return Rndom < 50;
        }

        protected bool VerifyShipPosition(Point ShipPosition, int ShipTiles, bool Vertical)
        {
            bool result = true;
            //Verifying rect in gamearea
            if (Vertical)
            {
              if (ShipPosition.Y + ShipTiles > (TilesHeight-1))
                {
                    result = false;
                }
            }
            else
            {
                if (ShipPosition.X - ShipTiles + 1 < 0)
                {
                    result = false;
                }
            }
            // Verifying ship on top of ship
            if (result)
            {
                if (Vertical)
                {
                    for (int i = 0; i < ShipTiles; i++)
                    {
                        if (Tiles[ShipPosition.X, ShipPosition.Y + i].HasShip())
                        {
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ShipTiles; i++)
                    {
                        if (Tiles[ShipPosition.X - ShipTiles + 1 + i, ShipPosition.Y].HasShip())
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }
        public void StartGame()
        {
            Vector2 StartVector = StartFont.MeasureString(Start);
            Rectangle StartRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)StartVector.X /2, Window.ClientBounds.Height / 2 - (int)StartVector.Y / 2, (int)StartVector.X, (int)StartVector.Y);

            Vector2 SettingsVector = StartFont.MeasureString(Settings);
            Rectangle SettingsRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)SettingsVector.X / 2, Window.ClientBounds.Height / 2 + (int)SettingsVector.Y, (int)SettingsVector.X, (int)SettingsVector.Y);
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && StartRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Gameplay;
            }
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Settings;
            }
                PrevMouseState = mouseState;
        }
        public void SettingMenu()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 SettingsLen = StartFont.MeasureString(Back);
            Rectangle SettingsRectangle = new Rectangle(0, TILE_SIZE * TilesHeight - (int)SettingsLen.Y + 100, (int)SettingsLen.X, (int)SettingsLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Startscreen;
            }
            PrevMouseState = mouseState;
        }
        public void DrawSettingMenu()
        {
            Vector2 SettingsLen = StartFont.MeasureString(Back);
            spriteBatch.DrawString(StartFont, Back, new Vector2(0, TILE_SIZE * TilesHeight - SettingsLen.Y + 100), Color.White);
        }
        public void DrawSettings()
        {
            Vector2 SettingsLen = StartFont.MeasureString(Settings);
            spriteBatch.DrawString(StartFont, Settings, new Vector2(Window.ClientBounds.Width / 2 - SettingsLen.X / 2, Window.ClientBounds.Height / 2 + SettingsLen.Y), Color.White);
        }
        public void DrawStartGame()
        {
            
            Vector2 StartLen = StartFont.MeasureString(Start);
            spriteBatch.DrawString(StartFont, Start, new Vector2(Window.ClientBounds.Width / 2 - StartLen.X / 2, Window.ClientBounds.Height / 2 - StartLen.Y / 2), Color.White);
        }
        public void EndGame()
        {
            for (int i = 0; i < Ships.Length; i++)
            {
                if (NumberOfShipsLeft == 0)
                {
                    CurrentState = GamesState.EndScreen;
                }
            }
        }
        public void ShipDeathCounter()
        {
            int temp = Ships.Length;
            for (int i = 0; i < Ships.Length; i++)
            {
                if (Ships[i].isvisible)
                {
                    temp--;
                }
            }
            NumberOfShipsLeft = temp;
        }
        public void DrawEndGame()
        {
            string Ended = "You dropped \n" + Bombs + " Bombs";
            Vector2 EndedLen = StartFont.MeasureString(Ended);
            spriteBatch.DrawString(StartFont, Ended, new Vector2(Window.ClientBounds.Width / 2 - EndedLen.X / 2, Window.ClientBounds.Height / 2 - EndedLen.Y / 2), Color.White);
              
        }
    }
}
