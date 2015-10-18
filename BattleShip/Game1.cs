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
    public class Game1 : Game
    {

        public const int TILE_SIZE = 50;
        const int HUD_HEIGHT = 100;

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
        int TilesWidth = 10;
        int TilesHeight = 10;
        int GameAreaWidth;
        int GameAreaHeight;
        int NumberOfShipsLeft = 4;
        int Init = 0;
        string Start = "START";
        string Settings = "SETTINGS";
        string Back = "BACK";
        string LeftArrow = "<";
        string RightArrow = ">";

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
            GameAreaWidth = TilesWidth * TILE_SIZE;
            GameAreaHeight = TilesHeight * TILE_SIZE;
            graphics.PreferredBackBufferWidth = TILE_SIZE * TilesWidth;
            graphics.PreferredBackBufferHeight = TILE_SIZE * TilesHeight + HUD_HEIGHT;
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
            CreatShips();
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
                    UpdateStartMenu();
                    break;
                case GamesState.Gameplay:
                    if (Init == 1)
                    {
                        Initialize();
                        Init = 0;
                    }
                    
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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            switch (CurrentState)
            {
                case GamesState.Startscreen:
                    DrawStartMenu();
                    break;
                case GamesState.Gameplay:
                    DrawWaterTiles();                    
                    DrawShips();                    
                    DrawTileStatus();
                    DrawHud();
                    Radar.Draw(spriteBatch, Radar_Sheet, new Rectangle(Window.ClientBounds.Width / 2 - 50, Window.ClientBounds.Height - 100, 100, 100), Clock.GetRotationForRadar());
                    break;
                case GamesState.Settings:
                    DrawSettingMenu();
                    break;
                case GamesState.EndScreen:
                    DrawEndGame();
                    break;
                default:
                    break;
            }
            spriteBatch.End();


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
            Tiles = new Tile[TilesHeight, TilesWidth];
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
        private void CreatShips()
        {
            
                Texture2D[] ConfigShip = new Texture2D[]
            {
                HangarshipTexture, BattleshipTexture, SubmarineTexture, DestroyerTexture, SubmarineTexture,
                BattleshipTexture, DestroyerTexture
            };
                ConfigShip = ConfigShip.Take(NumberOfShipsLeft).ToArray();
                Ships = new Ship[ConfigShip.Length];
                for (int i = 0; i < ConfigShip.Length; i++)
                {
                    bool FoundPosition = false;
                    bool TempVertical = RandomVertical();
                    Point Point = new Point();
                    while (!FoundPosition)
                    {
                        Point.X = Random();
                        Point.Y = Random();
                        FoundPosition = VerifyShipPosition(Point, ConfigShip[i].Height / TILE_SIZE, TempVertical);
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
                    Tiles[Point.X, Point.Y + i].SetShip(ship);
                }
            }
            else
            {
                for (int i = 0; i < ShipTiles; i++)
                {
                    Tiles[Point.X - ShipTiles + 1 + i, Point.Y].SetShip(ship);
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
            spriteBatch.DrawString(HudFont, Bomb, new Vector2(5, TILE_SIZE * TilesHeight), Color.White);
        }
        private void HandleMouseInput()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && IsPointInAnyTile(mouseState.Position))
            {
                LastClickedTile = GetTileWithPointInside(mouseState.Position);
                if (!LastClickedTile.Clicked)
                {
                    Bombs++;
                }                             
                if (IsPointInAnyShip(mouseState.Position))
                {
                    if (!LastClickedTile.Clicked)
                    {
                        
                        LastClickedTile.Explos = true;
                        LoseLife();
                    }
                }   
                else
                {
                    LastClickedTile.Explos = false;
                }
                LastClickedTile.Clicked = true;
            }
            PrevMouseState = mouseState;
        }
        private bool IsPointInAnyShip(Point Point)
        {

            for (int i = 0; i < Ships.Length; i++)
            {
                if (Ships[i].IsPointInShip(Point))
                {
                   return true;
                }
            }
            return false;
        }
        private Ship GetShipWithPointInsideIt(Point Point)
        {
            for (int i = 0; i < Ships.Length; i++)
            {
                if (Ships[i].IsPointInShip(Point))
                {
                    return Ships[i] ;

                }
            }
            return null;
        }
        private bool IsPointInAnyTile(Point Point)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    if (Tiles[i,k].IsPointInTile(Point))
                    {
                        return true;
                    }                   
                }
            }
            return false;
        }
        private Tile GetTileWithPointInside(Point Point)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    if (Tiles[i,k].IsPointInTile(Point))
                    {
                        return Tiles[i, k];
                    }
                }
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
        private int Random()
        {
            
            int Rndom = rnd.Next(0, 9);
            return Rndom;
        }
        public bool RandomVertical()
        {
            
            int Rndom = rnd.Next(0, 99);
            if (Rndom < 50)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        protected void LoseLife()
        {
            MouseState mouseState = Mouse.GetState();
            for (int i = 0; i < Ships.Length; i++)
            {
                if (IsPointInAnyShip(mouseState.Position))
                {
                    GetShipWithPointInsideIt(mouseState.Position).LoseLife();
                    Console.WriteLine(i);
                    break;
                }
            }
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
        protected void UpdateStartMenu()
        {
            Vector2 StartVector = StartFont.MeasureString(Start);
            Rectangle StartRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)StartVector.X /2, Window.ClientBounds.Height / 2 - (int)StartVector.Y / 2, (int)StartVector.X, (int)StartVector.Y);

            Vector2 SettingsVector = StartFont.MeasureString(Settings);
            Rectangle SettingsRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)SettingsVector.X / 2, Window.ClientBounds.Height / 2 + (int)SettingsVector.Y, (int)SettingsVector.X, (int)SettingsVector.Y);
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && StartRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Gameplay;
                Init = 1;
            }
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Settings;
            }
                PrevMouseState = mouseState;
        }
        protected void SettingMenu()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 BackLen = StartFont.MeasureString(Back);
            Rectangle SettingsRectangle = new Rectangle(0, 500 - (int)BackLen.Y + HUD_HEIGHT, (int)BackLen.X, (int)BackLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                CurrentState = GamesState.Startscreen;
            }
            HandleLeftArrow();
            HandleRightArrow();
            PrevMouseState = mouseState;

        }
        protected void HandleLeftArrow()
        {
            MouseState mouseState = Mouse.GetState();
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = StartFont.MeasureString(BoardSize);

            string NumberOfShips = "Ships" + NumberOfShipsLeft;
            Vector2 NumberOfShipsLen = StartFont.MeasureString(NumberOfShips);
            Vector2 LeftBoardArrowLen = StartFont.MeasureString(LeftArrow);

            Rectangle LeftShipArrowRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)NumberOfShipsLen.X / 2 - (int)LeftBoardArrowLen.X * 2, Window.ClientBounds.Height / 2 - (int)NumberOfShipsLen.Y / 2, (int)NumberOfShipsLen.X, (int)NumberOfShipsLen.Y);
            Rectangle LeftBoardArrowRectangle = new Rectangle(Window.ClientBounds.Width / 2 - (int)BoardSizeLen.X / 2 - (int)LeftBoardArrowLen.X * 2, Window.ClientBounds.Height / 2 + (int)BoardSizeLen.Y / 2, (int)LeftBoardArrowLen.X, (int)LeftBoardArrowLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && LeftBoardArrowRectangle.Contains(mouseState.Position))
            {
                if (TilesWidth > 8 && TilesHeight > 8 && TilesWidth <= 14 && TilesHeight <= 14)
                {
                    TilesWidth--;
                    TilesHeight--;
                }
            }
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && LeftShipArrowRectangle.Contains(mouseState.Position))
            {
                if (NumberOfShipsLeft > 4 && NumberOfShipsLeft > 4 && NumberOfShipsLeft <= 7 && NumberOfShipsLeft <= 7)
                {
                    NumberOfShipsLeft--;
                }
            }
        }
        protected void HandleRightArrow()
        {
            MouseState mouseState = Mouse.GetState();
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = StartFont.MeasureString(BoardSize);
            string NumberOfShips = "Ships" + NumberOfShipsLeft;
            Vector2 NumberOfShipsLen = StartFont.MeasureString(NumberOfShips);
            Vector2 RightShipArrowLen = StartFont.MeasureString(LeftArrow);

            Rectangle RightShipArrowRectangle = new Rectangle(Window.ClientBounds.Width / 2 + (int)NumberOfShipsLen.X / 2 + (int)RightShipArrowLen.X, Window.ClientBounds.Height / 2 - (int)NumberOfShipsLen.Y / 2, (int)NumberOfShipsLen.X, (int)NumberOfShipsLen.Y);
            Rectangle RightBoardArrowRectangle = new Rectangle(Window.ClientBounds.Width / 2 + (int)BoardSizeLen.X / 2 + (int)RightShipArrowLen.X, Window.ClientBounds.Height / 2 + (int)BoardSizeLen.Y / 2, (int)RightShipArrowLen.X, (int)RightShipArrowLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && RightBoardArrowRectangle.Contains(mouseState.Position))
            {
                if (TilesWidth >= 8 && TilesHeight >= 8 && TilesWidth < 14 && TilesHeight < 14)
                {
                    TilesWidth++;
                    TilesHeight++;
                }
            }
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && RightShipArrowRectangle.Contains(mouseState.Position))
            {
                if (NumberOfShipsLeft >= 4 && NumberOfShipsLeft >= 4 && NumberOfShipsLeft < 7 && NumberOfShipsLeft < 7)
                {
                    NumberOfShipsLeft++;
                }
            }
        }

        protected void DrawSettingMenu()
        {
            Vector2 BackLen = StartFont.MeasureString(Back);
            spriteBatch.DrawString(StartFont, Back, new Vector2(0, 500 - BackLen.Y + 100), Color.White);
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = StartFont.MeasureString(BoardSize);
            spriteBatch.DrawString(StartFont, BoardSize, new Vector2(Window.ClientBounds.Width / 2 - BoardSizeLen.X / 2, Window.ClientBounds.Height / 2 + BoardSizeLen.Y / 2), Color.White);
            string NumberOfShips = "Ships:" + NumberOfShipsLeft;
            Vector2 NumberOfShipsLen = StartFont.MeasureString(NumberOfShips);
            spriteBatch.DrawString(StartFont, NumberOfShips, new Vector2(Window.ClientBounds.Width / 2 - NumberOfShipsLen.X / 2, Window.ClientBounds.Height / 2 - NumberOfShipsLen.Y / 2), Color.White);
            Vector2 LeftBoardArrowLen = StartFont.MeasureString(LeftArrow);
            spriteBatch.DrawString(StartFont, LeftArrow, new Vector2(Window.ClientBounds.Width / 2 - BoardSizeLen.X / 2 - LeftBoardArrowLen.X * 2, Window.ClientBounds.Height / 2 + BoardSizeLen.Y / 2), Color.White);
            Vector2 LeftShipArrowLen = StartFont.MeasureString(LeftArrow);
            spriteBatch.DrawString(StartFont, LeftArrow, new Vector2(Window.ClientBounds.Width / 2 - NumberOfShipsLen.X / 2 - LeftBoardArrowLen.X * 2, Window.ClientBounds.Height / 2 - NumberOfShipsLen.Y / 2), Color.White);
            Vector2 RightShipArrowLen = StartFont.MeasureString(LeftArrow);
            spriteBatch.DrawString(StartFont, RightArrow, new Vector2(Window.ClientBounds.Width / 2 + NumberOfShipsLen.X / 2 + RightShipArrowLen.X, Window.ClientBounds.Height / 2 - NumberOfShipsLen.Y / 2), Color.White);
            Vector2 RightBoardArrowLen = StartFont.MeasureString(RightArrow);
            spriteBatch.DrawString(StartFont, RightArrow, new Vector2(Window.ClientBounds.Width / 2 + BoardSizeLen.X / 2 + RightBoardArrowLen.X, Window.ClientBounds.Height / 2 + BoardSizeLen.Y / 2), Color.White);
        }
        protected void DrawStartMenu()
        {
            
            Vector2 StartLen = StartFont.MeasureString(Start);
            spriteBatch.DrawString(StartFont, Start, new Vector2(Window.ClientBounds.Width / 2 - StartLen.X / 2, Window.ClientBounds.Height / 2 - StartLen.Y / 2), Color.White);
            Vector2 SettingsLen = StartFont.MeasureString(Settings);
            spriteBatch.DrawString(StartFont, Settings, new Vector2(Window.ClientBounds.Width / 2 - SettingsLen.X / 2, Window.ClientBounds.Height / 2 + SettingsLen.Y), Color.White);
        }
        protected void EndGame()
        {
            for (int i = 0; i < Ships.Length; i++)
            {
                if (NumberOfShipsLeft == 0)
                {
                    CurrentState = GamesState.EndScreen;
                }
            }
        }
        protected void ShipDeathCounter()
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
        protected void DrawEndGame()
        {
            string Ended = "You dropped \n" + Bombs + " Bombs";
            Vector2 EndedLen = StartFont.MeasureString(Ended);
            spriteBatch.DrawString(StartFont, Ended, new Vector2(Window.ClientBounds.Width / 2 - EndedLen.X / 2, Window.ClientBounds.Height / 2 - EndedLen.Y / 2), Color.White);
              
        }
    }
}
