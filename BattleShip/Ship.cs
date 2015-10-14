using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip
{
    class Ship
    {

        Rectangle ShipRectangle;
        Texture2D ShipTexture;
        bool[] lifes;
        bool vertical;
        public Ship(Rectangle ShipRectangle, Texture2D ShipTexture, bool vertical)        
        {
            this.ShipRectangle = ShipRectangle;
            this.ShipTexture = ShipTexture;
            this.vertical = vertical;        
            Console.WriteLine("Ship Created");
            if( vertical)
            {
                lifes = new bool[ShipRectangle.Height / Game1.TILE_SIZE];
            }
            else  // Horizontal
            {
                lifes = new bool[ShipRectangle.Width / Game1.TILE_SIZE];
            }
            for(int i=0; i<lifes.Length; i++)
            {
                lifes[i] = true;
            }

        }
        public void Draw(SpriteBatch sb)
        {
            if (IsVisible())
            {
                sb.Draw(ShipTexture, ShipRectangle, Color.White);
            }
                          
        }

        public bool IsPointInShip(Point Position)
        {   
            return ShipRectangle.Contains(Position);
                   
        }
        public bool IsVisible()
        {
            bool isVisible = true;
            for (int i = 0; i < lifes.Length; i++)
            {
                if(lifes[i])
                {
                    isVisible = false;
                    break;
                }
            }
            return isVisible;
        }

        public void LoseLife(Point Position)
        {
            if(vertical)
            {
                lifes[(ShipRectangle.Bottom - Position.Y) / Game1.TILE_SIZE] = false;
            }
            else
            {

            }
        }
    }
}
