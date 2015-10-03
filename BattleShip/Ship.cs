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
        public Ship(Rectangle ShipRectangle, Texture2D ShipTexture)        
        {
            this.ShipRectangle = ShipRectangle;
            this.ShipTexture = ShipTexture;
            Console.WriteLine("Ship Created");

        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(ShipTexture, ShipRectangle, Color.White);
            
        }
        public bool IsPointInShip(Point Position)
        {   
            return ShipRectangle.Contains(Position);               
        }
    }
}
