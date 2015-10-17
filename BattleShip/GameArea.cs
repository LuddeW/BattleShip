using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip
{
    class GameArea
    {
        Tile LastClickedTile = null;
        Tile[,] Tiles;
        Ship[] Ships;
        private int PosX;
        private int PosY;
        private int TilesHeight;
        private int TilesWidth;
        private Random rnd;
        private SpriteBatch spriteBatch;
        public int Bombs = 0;

        public int Width { get { return Game1.TILE_SIZE * TilesWidth; } }
        public int Height { get { return Game1.TILE_SIZE * TilesHeight; } }

        public GameArea(int PosX, int  PosY, int TilesWidth, int TilesHeight, SpriteBatch spriteBatch, Random rnd)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.TilesWidth = TilesWidth;
            this.TilesHeight = TilesHeight;
            this.spriteBatch = spriteBatch;
            this.rnd = rnd;
        }

        private void CreatShips()
        {

            Texture2D[] ConfigShip =
            {
                Game1.DestroyerTexture,
                Game1.SubmarineTexture,
                Game1.BattleshipTexture,
                Game1.HangarshipTexture,
                Game1.DestroyerTexture
            };
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
                    FoundPosition = VerifyShipPosition(Point, ConfigShip[i].Height / Game1.TILE_SIZE, TempVertical);
                }

                Rectangle TempRectangle = new Rectangle(PosX + Point.X * Game1.TILE_SIZE, PosY + Point.Y * Game1.TILE_SIZE, ConfigShip[i].Width, ConfigShip[i].Height);

                Ship ship = new Ship(TempRectangle, ConfigShip[i], TempVertical);
                Ships.SetValue(ship, i);
                AddShipToTiles(ship, Point, TempVertical, ConfigShip[i].Height / Game1.TILE_SIZE);
            }
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

        private void DrawShips()
        {


            for (int i = 0; i < Ships.Length; i++)
            {
                Ships[i].Draw(spriteBatch);
            }
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

        internal void Create()
        {
            CreatTileArray();
            CreatShips();
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

        public bool EndGame()
        {
            int temp = Ships.Length;
            for (int i = 0; i < Ships.Length; i++)
            {
                if (Ships[i].isvisible)
                {
                    temp--;
                }
            }
            return temp == 0;
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

        private void DrawHud()
        {
            string Bomb = "Bombs dropped:" + Bombs;
            spriteBatch.DrawString(Game1.HudFont, Bomb, new Vector2(PosX + 5, PosY + Game1.TILE_SIZE * TilesHeight), Color.White);
        }

        internal void Draw()
        {
            DrawWaterTiles();
            DrawShips();
            DrawTileStatus();
            DrawHud();
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
                if (ShipPosition.Y + ShipTiles > (TilesHeight - 1))
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
                    Tiles[i, k] = new Tile(new Rectangle(PosX + i * Game1.TILE_SIZE, PosY + k * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE), Game1.WaterTileTexture, Game1.Explosion, Game1.Splash);
                }
            }
        }


    }
}
