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
        public Rectangle WaterRect;
        Texture2D WaterTile;
        Texture2D Explosion;
        Texture2D Splash;
        public bool Clicked;
        public bool Explos ;
        

        public Tile(Rectangle TileRectangle, Texture2D WaterTile, Texture2D Explosion, Texture2D Splash)
        {
            WaterRect = TileRectangle;
            this.WaterTile = WaterTile;
            this.Explosion = Explosion;
            this.Splash = Splash;
        }
        public void DrawWater(SpriteBatch sb)
        {
            sb.Draw(WaterTile, WaterRect, Color.White);
            
        }
        public bool IsPointInTile(Point Position)
        {
            return WaterRect.Contains(Position);
        }
        public Rectangle GetTilePosition()
        {

             
             return WaterRect;
               
        }
        public void DrawExplosion(SpriteBatch sb)
        {
            sb.Draw(Explosion, WaterRect, Color.White);
        }
        public void DrawSplash(SpriteBatch sb)
        {
            sb.Draw(Splash, WaterRect, Color.White);
            
        }
    }
}
