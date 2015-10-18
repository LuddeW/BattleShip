using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.Screens
{
    class EndScreen
    {
        BattleShipGame Game;
        SpriteFont StartFont;

        public EndScreen(BattleShipGame Game, SpriteFont StartFont)
        {
            this.Game = Game;
            this.StartFont = StartFont;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Game.SetScreen(BattleShipGame.GamesState.Startscreen);
        }

        public void Draw(SpriteBatch sb, int Bombs)
        {
            string Ended = "You dropped \n" + Bombs + " Bombs";
            Vector2 EndedLen = StartFont.MeasureString(Ended);
            sb.DrawString(StartFont, Ended, new Vector2(Game.Window.ClientBounds.Width / 2 - EndedLen.X / 2, Game.Window.ClientBounds.Height / 2 - EndedLen.Y / 2), Color.White);

        }
    }
}
