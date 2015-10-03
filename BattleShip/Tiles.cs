using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BattleShip
{
    class Tiles
    {
        Rectangle WaterRect;
        
        public Tiles(Rectangle TileRectangle)
        {
            WaterRect = TileRectangle;
        }
        public void DrawWater(SpriteBatch sb, Texture2D WaterTile)
        {
            sb.Draw(WaterTile, WaterRect, Color.White);
        }
       /* public void DrawShips(Ships Ship, SpriteBatch sb)
        {
            Ship.Draw();
        }*/
    }
}
