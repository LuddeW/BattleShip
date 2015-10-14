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
        private Tile LastClickedTile = null;
        private Tile[,] Tiles;
        private Ship[] Ships;
        private Resources resources = null;
        private int posX = 0;
        private int posY = 0;
        private int Bombs = 0;

        public GameArea(Resources resources, int posX, int posY)
        {
            this.resources = resources;
            this.posX = posX;
            this.posY = posY;
        }

        public int Width()
        {
            return Game1.TILE_SIZE * Game1.TILES_HORIZONTAL;
        }

        public int Heght()
        {
            return Game1.TILE_SIZE * Game1.TILES_VERTICAL;
        }

        public void Create()
        {
            Tiles = new Tile[Game1.TILES_HORIZONTAL, Game1.TILES_VERTICAL];
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    Tiles[i, k] = new Tile(new Rectangle(posX + i * Game1.TILE_SIZE, posY + k * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE),
                                                resources.WaterTileTexture, resources.Explosion, resources.Splash);
                }
            }
            GenerateShips();
        }

        private void GenerateShips()
        {

            Ships = new Ship[]
            {

                new Ship(new Rectangle(Random() * Game1.TILE_SIZE, Random() * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE, 2 * Game1.TILE_SIZE), resources.DestroyerTexture, true), //Destroyer
                new Ship(new Rectangle(1 * Game1.TILE_SIZE, 0 * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE, 3 * Game1.TILE_SIZE), resources.SubmarineTexture, true), //Submarine
                new Ship(new Rectangle(2 * Game1.TILE_SIZE, 0 * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE, 4 * Game1.TILE_SIZE), resources.BattleshipTexture, true), // Battleship
                new Ship(new Rectangle(3 * Game1.TILE_SIZE, 0 * Game1.TILE_SIZE, 1 * Game1.TILE_SIZE, 5 * Game1.TILE_SIZE), resources.HangarshipTexture, true)// Hangarship
            };

        }

        private int Random()
        {
            Random rnd = new Random();
            int Rndom = rnd.Next(0, 10);
            return Rndom;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            DrawWaterTiles(spriteBatch);
            DrawShips(spriteBatch);
            DrawTileStatus(spriteBatch);
            DrawFonts(spriteBatch);
        }

        protected void DrawWaterTiles(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int k = 0; k < Tiles.GetLength(1); k++)
                {
                    Tiles[i, k].DrawWater(spriteBatch);
                }
            }
        }
        private void DrawShips(SpriteBatch spriteBatch)
        {


            for (int i = 0; i < Ships.Length; i++)
            {
                Ships[i].Draw(spriteBatch);
            }
        }

        private void DrawTileStatus(SpriteBatch spriteBatch)
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
                        if (/*LastClickedTile != null &&*/ Tiles[i, k].Clicked == true)
                        {
                            Tiles[i, k].DrawSplash(spriteBatch);
                        }
                    }
                }
            }
        }

        private void DrawFonts(SpriteBatch spriteBatch)
        {
            string Bomb = "Bombs droped:" + Bombs;
            spriteBatch.DrawString(resources.HudFont, Bomb, new Vector2(5, Heght() + 5), Color.White);
        }

        internal void HandleMouseInput(MouseState mouseState, MouseState prevMouseState)
        {
            if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed && IsPointInAnyTile(mouseState.Position))
            {

                Bombs++;
                LastClickedTile = GetTileWithPointInside(mouseState.Position);
                for (int i = 0; i < Tiles.GetLength(0); i++)
                {
                    for (int k = 0; k < Tiles.GetLength(1); k++)
                    {
                        if (Tiles[i, k].WaterRect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            Tiles[i, k].Clicked = true;
                            Tiles[i, k].Explos = false;
                        }

                    }
                }
                if (IsPointInAnyShip(mouseState.Position))
                {

                    Console.WriteLine("Clicked on ship");
                    GetShipWithPointInsideIt(mouseState.Position).LoseLife(mouseState.Position);
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
    }
}

