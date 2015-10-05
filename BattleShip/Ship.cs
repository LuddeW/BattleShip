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
        int lives;
        bool isvisible;
        public Ship(Rectangle ShipRectangle, Texture2D ShipTexture)        
        {
            this.ShipRectangle = ShipRectangle;
            this.ShipTexture = ShipTexture;            
            Console.WriteLine("Ship Created");
            lives = ShipRectangle.Height / 50;
        }
        public void Draw(SpriteBatch sb)
        {
            Visible();
            if (isvisible)
            {
                sb.Draw(ShipTexture, ShipRectangle, Color.White);
            }
                          
        }
        public bool IsPointInShip(Point Position)
        {   
            return ShipRectangle.Contains(Position);
            
                   
        }
        public void Visible()
        {
            if (lives <= 0)
            {
                isvisible = true;
                              
            }
            else
            {
                isvisible = false;
            }
        }
        public void LoseLife()
        {
            lives--;            
        }
    }
}
