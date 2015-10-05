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
        Tile[,] Tiles;
        Ship[] Ships;
        MouseState PrevMouseState;
        

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
            graphics.PreferredBackBufferHeight = 500;
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
            CheckMouseInput();
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
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            DrawWaterTiles();
            DrawShips();
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        protected void DrawWaterTiles()
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[i, j].DrawWater(spriteBatch);
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
                    Tiles[i, k] = new Tile(new Rectangle(i * TILE_SIZE, k * TILE_SIZE, 1 * TILE_SIZE, 1 * TILE_SIZE), WaterTileTexture);
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
        }
        private void CreatShips()
        {
            Ships = new Ship[]
            {

                new Ship(new Rectangle(0 * TILE_SIZE, 0 * TILE_SIZE, 1 * TILE_SIZE, 2 * TILE_SIZE), DestroyerTexture), //Destroyer
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
        private void CheckMouseInput()
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed)
            {
                if (IsPointInAnyShip(mouseState.Position))
                {
                    Console.WriteLine("Clicked on ship");
                    for (int i = 0; i < Ships.Length; i++)
                    {
                        Ships[i].LoseLife();
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
    }
}
