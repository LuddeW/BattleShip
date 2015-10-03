using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BattleShip
{
    class Tile
    {
        Rectangle WaterRect;
        Texture2D WaterTile;
        
        public Tile(Rectangle TileRectangle, Texture2D WaterTile)
        {
            WaterRect = TileRectangle;
            this.WaterTile = WaterTile;
        }
        public void DrawWater(SpriteBatch sb)
        {
            sb.Draw(WaterTile, WaterRect, Color.White);
        }
    }
}
