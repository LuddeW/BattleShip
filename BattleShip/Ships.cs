using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip
{
    class Ships
    {

        Rectangle rect;    

            public Ships(Rectangle ShipRectangle)
            {
                rect = ShipRectangle;
            }
            public void Draw(SpriteBatch sb, Texture2D ShipTile)
            {
                sb.Draw(ShipTile, rect, Color.White);
            }
        
    }
}
