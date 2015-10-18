using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.Screens
{
    class SettingScreen
    {
        SpriteFont StartFont;
        BattleShipGame Game;
        string Back = "BACK";
        string LeftArrow = "<";
        string RightArrow = ">";

        public int TilesWidth = 10;
        public int TilesHeight = 10;
        public int NumberOfShips = 4;

        public SettingScreen (BattleShipGame Game, SpriteFont StartFont)
        {
            this.Game = Game;
            this.StartFont = StartFont;           
        }

        public void Update (MouseState mouseState, MouseState PrevMouseState)
        {
            Vector2 BackLen = StartFont.MeasureString(Back);
            Rectangle SettingsRectangle = new Rectangle(0, 500 - (int)BackLen.Y + BattleShipGame.HUD_HEIGHT, (int)BackLen.X, (int)BackLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && SettingsRectangle.Contains(mouseState.Position))
            {
                Game.SetScreen(BattleShipGame.GamesState.Startscreen);
            }
            HandleLeftArrow(mouseState, PrevMouseState);
            HandleRightArrow(mouseState, PrevMouseState);
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 BackLen = StartFont.MeasureString(Back);
            sb.DrawString(StartFont, Back, new Vector2(0, 500 - BackLen.Y + 100), Color.White);
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = StartFont.MeasureString(BoardSize);
            sb.DrawString(StartFont, BoardSize, new Vector2(Game.Window.ClientBounds.Width / 2 - BoardSizeLen.X / 2, Game.Window.ClientBounds.Height / 2 + BoardSizeLen.Y / 2), Color.White);
            string NumberOfShips = "Ships:" + this.NumberOfShips;
            Vector2 NumberOfShipsLen = StartFont.MeasureString(NumberOfShips);
            sb.DrawString(StartFont, NumberOfShips, new Vector2(Game.Window.ClientBounds.Width / 2 - NumberOfShipsLen.X / 2, Game.Window.ClientBounds.Height / 2 - NumberOfShipsLen.Y / 2), Color.White);
            Vector2 LeftBoardArrowLen = StartFont.MeasureString(LeftArrow);
            sb.DrawString(StartFont, LeftArrow, new Vector2(Game.Window.ClientBounds.Width / 2 - BoardSizeLen.X / 2 - LeftBoardArrowLen.X * 2, Game.Window.ClientBounds.Height / 2 + BoardSizeLen.Y / 2), Color.White);
            Vector2 LeftShipArrowLen = StartFont.MeasureString(LeftArrow);
            sb.DrawString(StartFont, LeftArrow, new Vector2(Game.Window.ClientBounds.Width / 2 - NumberOfShipsLen.X / 2 - LeftBoardArrowLen.X * 2, Game.Window.ClientBounds.Height / 2 - NumberOfShipsLen.Y / 2), Color.White);
            Vector2 RightShipArrowLen = StartFont.MeasureString(LeftArrow);
            sb.DrawString(StartFont, RightArrow, new Vector2(Game.Window.ClientBounds.Width / 2 + NumberOfShipsLen.X / 2 + RightShipArrowLen.X, Game.Window.ClientBounds.Height / 2 - NumberOfShipsLen.Y / 2), Color.White);
            Vector2 RightBoardArrowLen = StartFont.MeasureString(RightArrow);
            sb.DrawString(StartFont, RightArrow, new Vector2(Game.Window.ClientBounds.Width / 2 + BoardSizeLen.X / 2 + RightBoardArrowLen.X, Game.Window.ClientBounds.Height / 2 + BoardSizeLen.Y / 2), Color.White);
        }

        public void HandleLeftArrow(MouseState mouseState, MouseState PrevMouseState)
        {
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = StartFont.MeasureString(BoardSize);

            string NumberOfShips = "Ships" + this.NumberOfShips;
            Vector2 NumberOfShipsLen = StartFont.MeasureString(NumberOfShips);
            Vector2 LeftBoardArrowLen = StartFont.MeasureString(LeftArrow);

            Rectangle LeftShipArrowRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 - (int)NumberOfShipsLen.X / 2 - (int)LeftBoardArrowLen.X * 2, Game.Window.ClientBounds.Height / 2 - (int)NumberOfShipsLen.Y / 2, (int)NumberOfShipsLen.X, (int)NumberOfShipsLen.Y);
            Rectangle LeftBoardArrowRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 - (int)BoardSizeLen.X / 2 - (int)LeftBoardArrowLen.X * 2, Game.Window.ClientBounds.Height / 2 + (int)BoardSizeLen.Y / 2, (int)LeftBoardArrowLen.X, (int)LeftBoardArrowLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && LeftBoardArrowRectangle.Contains(mouseState.Position))
            {
                if (TilesWidth > 8 && TilesHeight > 8 && TilesWidth <= 14 && TilesHeight <= 14)
                {
                    TilesWidth--;
                    TilesHeight--;
                }
            }
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && LeftShipArrowRectangle.Contains(mouseState.Position))
            {
                if (this.NumberOfShips > 4 && this.NumberOfShips > 4 && this.NumberOfShips <= 7 && this.NumberOfShips <= 7)
                {
                    this.NumberOfShips--;
                }
            }
        }

        public void HandleRightArrow(MouseState mouseState, MouseState PrevMouseState)
        {
            string BoardSize = TilesWidth + "X" + TilesHeight;
            Vector2 BoardSizeLen = StartFont.MeasureString(BoardSize);
            string NumberOfShips = "Ships" + this.NumberOfShips;
            Vector2 NumberOfShipsLen = StartFont.MeasureString(NumberOfShips);
            Vector2 RightShipArrowLen = StartFont.MeasureString(LeftArrow);

            Rectangle RightShipArrowRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 + (int)NumberOfShipsLen.X / 2 + (int)RightShipArrowLen.X, Game.Window.ClientBounds.Height / 2 - (int)NumberOfShipsLen.Y / 2, (int)NumberOfShipsLen.X, (int)NumberOfShipsLen.Y);
            Rectangle RightBoardArrowRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 + (int)BoardSizeLen.X / 2 + (int)RightShipArrowLen.X, Game.Window.ClientBounds.Height / 2 + (int)BoardSizeLen.Y / 2, (int)RightShipArrowLen.X, (int)RightShipArrowLen.Y);

            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && RightBoardArrowRectangle.Contains(mouseState.Position))
            {
                if (TilesWidth >= 8 && TilesHeight >= 8 && TilesWidth < 14 && TilesHeight < 14)
                {
                    TilesWidth++;
                    TilesHeight++;
                }
            }
            if (mouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton == ButtonState.Pressed && RightShipArrowRectangle.Contains(mouseState.Position))
            {
                if (this.NumberOfShips >= 4 && this.NumberOfShips >= 4 && this.NumberOfShips < 7 && this.NumberOfShips < 7)
                {
                    this.NumberOfShips++;
                }
            }
        }
    }
}
