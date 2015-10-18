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
        public bool isvisible;
        float HorizontalValue = 1.57f;
        float VerticalValue = 0f;
        bool Vertical;
        public Ship(Rectangle ShipRectangle, Texture2D ShipTexture, bool Vertical)        
        {
            this.ShipRectangle = ShipRectangle;
            this.ShipTexture = ShipTexture;
            this.Vertical = Vertical;     
            Console.WriteLine("Ship Created");
            lives = ShipRectangle.Height / 50;
            
        }
        public void Draw(SpriteBatch sb)
        {
            Visible();
            if (isvisible)
            {
                if (Vertical)
                {
                    Vector2 origin = new Vector2(0, 0);
                    sb.Draw(ShipTexture, ShipRectangle, null, Color.White, VerticalValue, origin, SpriteEffects.None, 0f);
                }
                else
                {
                    Vector2 origin = new Vector2 (0, 50);
                    sb.Draw(ShipTexture, ShipRectangle, null, Color.White, HorizontalValue, origin, SpriteEffects.None, 0f);
                }
            }
                          
        }
        public bool IsPointInShip(Point Position)
        {
            if (Vertical)
            {
                return ShipRectangle.Contains(Position);
            }
            else
            {
                Rectangle Rotated = new Rectangle(ShipRectangle.X + BattleShipGame.TILE_SIZE - ShipRectangle.Height, ShipRectangle.Y, ShipRectangle.Height, ShipRectangle.Width);
                return Rotated.Contains(Position);
            }
            
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
