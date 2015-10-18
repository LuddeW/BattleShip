using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.Screens
{
    public class StartScreen
    {
        SpriteFont StartFont;
        BattleShipGame Game;
        string Start = "START";
        string Settings = "SETTINGS";
        
        public StartScreen(BattleShipGame Game, SpriteFont StartFont)
        {
            this.StartFont = StartFont;
            this.Game = Game;
        }

        public void Update(MouseState mouseState, MouseState PrevMouseState)
        {
            Vector2 StartVector = StartFont.MeasureString(Start);
            Rectangle StartRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 - (int)StartVector.X / 2, Game.Window.ClientBounds.Height / 2 - (int)StartVector.Y / 2, (int)StartVector.X, (int)StartVector.Y);

            Vector2 SettingsVector = StartFont.MeasureString(Settings);
            Rectangle SettingsRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 - (int)SettingsVector.X / 2, Game.Window.ClientBounds.Height / 2 + (int)SettingsVector.Y, (int)SettingsVector.X, (int)SettingsVector.Y);
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && StartRectangle.Contains(mouseState.Position))
            {
                Game.SetScreen(BattleShipGame.GamesState.Gameplay);
            }
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                Game.SetScreen(BattleShipGame.GamesState.Settings);
            }
            
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 StartLen = StartFont.MeasureString(Start);
            sb.DrawString(StartFont, Start, new Vector2(Game.Window.ClientBounds.Width / 2 - StartLen.X / 2, Game.Window.ClientBounds.Height / 2 - StartLen.Y / 2), Color.White);
            Vector2 SettingsLen = StartFont.MeasureString(Settings);
            sb.DrawString(StartFont, Settings, new Vector2(Game.Window.ClientBounds.Width / 2 - SettingsLen.X / 2, Game.Window.ClientBounds.Height / 2 + SettingsLen.Y), Color.White);
        }
    }
    
}
