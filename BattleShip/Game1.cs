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

        const int TILE_SIZE = 50;

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
        Tile LastClickedTile = null;
        Tile[,] Tiles;
        Ship[] Ships;
        Radar Radar = new Radar();
        Clock Clock = new Clock();
        MouseState PrevMouseState;
        int Bombs = 0;

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
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 600;
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
            DrawWaterTiles();
            DrawShips();
            DrawTileStatus();
            DrawFonts();
            Radar.Draw(spriteBatch, Radar_Sheet, new Rectangle(Window.ClientBounds.Width / 2 - 50, Window.ClientBounds.Height - 100, 100, 100), Clock.GetRotationForRadar());
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
            Tiles = new Tile[10, 10];
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
        }
        private void CreatShips()
        {
            
            Ships = new Ship[]
            {

                new Ship(new Rectangle(Random() * TILE_SIZE, Random() * TILE_SIZE, 1 * TILE_SIZE, 2 * TILE_SIZE), DestroyerTexture), //Destroyer
                new Ship(new Rectangle(1 * TILE_SIZE, 0 * TILE_SIZE, 1 * TILE_SIZE, 3 * TILE_SIZE), SubmarineTexture), //Submarine
                new Ship(new Rectangle(2 * TILE_SIZE, 0 * TILE_SIZE, 1 * TILE_SIZE, 4 * TILE_SIZE), BattleshipTexture), // Battleship
                new Ship(new Rectangle(3 * TILE_SIZE, 0 * TILE_SIZE, 1 * TILE_SIZE, 5 * TILE_SIZE), HangarshipTexture)// Hangarship
            };

        }
        private void DrawShips()
        {
            
            
            for (int i = 0; i < Ships.Length; i++)
            {                
                    Ships[i].Draw(spriteBatch);   
            }
        }
        private void DrawFonts()
        {
            string Bomb = "Bombs droped:" + Bombs;
            spriteBatch.DrawString(HudFont, Bomb, new Vector2(5, 505), Color.White);
        }
        private void HandleMouseInput()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && IsPointInAnyTile(mouseState.Position))
            {
                
                Bombs++;
                LastClickedTile = GetTileWithPointInside(mouseState.Position);
                for (int i = 0; i < Tiles.GetLength(0); i++)
                {
                    for (int k = 0; k < Tiles.GetLength(1); k++)
                    {
                        if (Tiles [i, k].WaterRect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            Tiles[i, k].Clicked = true;
                            Tiles[i, k].Explos = false;
                        }
                        
                    }
                }
                if (IsPointInAnyShip(mouseState.Position))
                {
                   
                    Console.WriteLine("Clicked on ship");
                    GetShipWithPointInsideIt(mouseState.Position).LoseLife();
                    for (int i = 0; i < Tiles.GetLength(0); i++)
                    {
                        for (int k = 0; k < Tiles.GetLength(1); k++)
                        {
                            if (Tiles[i, k].WaterRect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                            {
                                Tiles[i, k].Clicked = true;
                                Tiles[i, k].Explos = true;
                            }
                        }
                    }
                }                
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
            Random rnd = new Random();
            int Rndom = rnd.Next(0, 10);
            return Rndom;
        }
    }
}
