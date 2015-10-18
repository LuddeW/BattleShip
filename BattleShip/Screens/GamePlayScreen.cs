using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BattleShip.Screens
{
    class GamePlayScreen
    {
        BattleShipGame Game;

        SpriteFont HudFont;
        Texture2D WaterTileTexture;
        Texture2D DestroyerTexture;
        Texture2D SubmarineTexture;
        Texture2D BattleshipTexture;
        Texture2D HangarshipTexture;
        Texture2D Splash;
        Texture2D Explosion;
        Texture2D Radar_Sheet;

        Tile LastClickedTile = null;
        Tile[,] Tiles;
        Ship[] Ships;
        Radar Radar = new Radar();
        Clock Clock = new Clock();
        Random rnd = new Random();

        public int Bombs = 0;
        int NumberOfShipsLeft = 0;
        int Boardsize = 0;

        public GamePlayScreen(BattleShipGame Game, SpriteFont HudFont)
        {
            this.Game = Game;
            this.HudFont = HudFont;
            LoadPictures();
        }

        private void LoadPictures()
        {
            WaterTileTexture = Game.Content.Load<Texture2D>(@"watertile");
            DestroyerTexture = Game.Content.Load<Texture2D>(@"ship2x2");
            SubmarineTexture = Game.Content.Load<Texture2D>(@"ship3x3");
            BattleshipTexture = Game.Content.Load<Texture2D>(@"ship4x4");
            HangarshipTexture = Game.Content.Load<Texture2D>(@"ship5x5");
            Splash = Game.Content.Load<Texture2D>(@"splash");
            Explosion = Game.Content.Load<Texture2D>(@"explosion");
            Radar_Sheet = Game.Content.Load<Texture2D>(@"radar_spritesheet");
        }

        public void StartGame(int Size, int NumberOfShips)
        {
            NumberOfShipsLeft = NumberOfShips;
            Boardsize = Size;
            CreateTileArray();
            CreateShips();
        }

        protected void CreateTileArray()
        {
            Tiles = new Tile[Boardsize, Boardsize];
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    Tiles[i, k] = new Tile(new Rectangle(i * BattleShipGame.TILE_SIZE, k * BattleShipGame.TILE_SIZE, 1 * BattleShipGame.TILE_SIZE, 1 * BattleShipGame.TILE_SIZE), WaterTileTexture, Explosion, Splash);
                }
            }
        }

        private void CreateShips()
        {

            Texture2D[] ConfigShip = new Texture2D[]
        {
                HangarshipTexture, BattleshipTexture, SubmarineTexture, DestroyerTexture, SubmarineTexture,
                BattleshipTexture, DestroyerTexture
        };
            
            Ships = new Ship[NumberOfShipsLeft];
            for (int i = 0; i < NumberOfShipsLeft; i++)
            {
                bool FoundPosition = false;
                bool TempVertical = RandomVertical();
                Point Point = new Point();
                while (!FoundPosition)
                {
                    Point.X = Random();
                    Point.Y = Random();
                    FoundPosition = VerifyShipPosition(Point, ConfigShip[i].Height / BattleShipGame.TILE_SIZE, TempVertical);
                }

                Rectangle TempRectangle = new Rectangle(Point.X * BattleShipGame.TILE_SIZE, Point.Y * BattleShipGame.TILE_SIZE, ConfigShip[i].Width, ConfigShip[i].Height);

                Ship ship = new Ship(TempRectangle, ConfigShip[i], TempVertical);
                Ships.SetValue(ship, i);
                AddShipToTiles(ship, Point, TempVertical, ConfigShip[i].Height / BattleShipGame.TILE_SIZE);
            }

        }

        public void Update(GameTime gameTime, MouseState mouseState, MouseState PrevMouseState)
        {
            HandleMouseInput(mouseState, PrevMouseState);
            ShipDeathCounter();
            CheckIfGameHasEnded();
            Clock.AddTime((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void HandleMouseInput(MouseState mouseState, MouseState PrevMouseState)
        {
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
        }

        public void CheckIfGameHasEnded()
        {           
            if (NumberOfShipsLeft == 0)
            {
                Game.SetScreen(BattleShipGame.GamesState.EndScreen);
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

        public void Draw(SpriteBatch sb)
        {
            DrawWaterTiles(sb);
            DrawShips(sb);
            DrawTileStatus(sb);
            DrawRadar(sb);
            DrawHud(sb);
        }

        public void DrawWaterTiles(SpriteBatch sb)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    Tiles[i, k].DrawWater(sb);
                }
            }
        }

        public void DrawShips(SpriteBatch sb)
        {
            for (int i = 0; i < Ships.Length; i++)
            {
                Ships[i].Draw(sb);
            }
        }

        public void DrawTileStatus(SpriteBatch sb)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    if (Tiles[i, k].Explos == true && Tiles[i, k].Clicked == true)
                    {
                        Tiles[i, k].DrawExplosion(sb);
                    }
                    else if (Tiles[i, k].Explos == false)
                    {
                        if (LastClickedTile != null && Tiles[i, k].Clicked == true)
                        {
                            Tiles[i, k].DrawSplash(sb);
                        }
                    }
                }
            }
        }

        private void DrawHud(SpriteBatch sb)
        {
            string Bomb = "Bombs dropped:" + Bombs;
            sb.DrawString(HudFont, Bomb, new Vector2(5, BattleShipGame.TILE_SIZE * Boardsize), Color.White);
        }

        public void DrawRadar(SpriteBatch sb)
        {
            Radar.Draw(sb, Radar_Sheet, new Rectangle(Game.Window.ClientBounds.Width / 2 - 50, Game.Window.ClientBounds.Height - 100, 100, 100), Clock.GetRotationForRadar());
        }

        private bool IsPointInAnyTile(Point Point)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    if (Tiles[i, k].IsPointInTile(Point))
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
                    if (Tiles[i, k].IsPointInTile(Point))
                    {
                        return Tiles[i, k];
                    }
                }
            }
            return null;
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
                    return Ships[i];

                }
            }
            return null;
        }

        protected void LoseLife()
        {
            MouseState mouseState = Mouse.GetState();
            for (int i = 0; i < Ships.Length; i++)
            {
                if (IsPointInAnyShip(mouseState.Position))
                {
                    GetShipWithPointInsideIt(mouseState.Position).LoseLife();
                    break;
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

        protected bool VerifyShipPosition(Point ShipPosition, int ShipTiles, bool Vertical)
        {
            bool result = true;
            //Verifying rect in gamearea
            if (Vertical)
            {
                if (ShipPosition.Y + ShipTiles > (Boardsize - 1))
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

        private void AddShipToTiles(Ship ship, Point Point, bool Vertical, int ShipTiles)
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

    }
}
