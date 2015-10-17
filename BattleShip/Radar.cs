using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip
{
    class Radar
    {
        Clock Clock = new Clock();

        public void Draw(SpriteBatch sb, Texture2D Radar_Sheet, Rectangle Pos, int Rotation)
        {
            sb.Draw(Radar_Sheet, Pos,
                new Rectangle(Rotation * 100, 0, 100, 100), Color.White);
        }
    }
}
