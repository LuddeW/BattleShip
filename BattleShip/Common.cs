using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;

namespace BattleShip
{
    class Common
    {

        private Texture2D waterTileTexture;
        private Texture2D destroyerTexture;
        private Texture2D submarineTexture;
        private Texture2D battleshipTexture;
        private Texture2D hangarshipTexture;
        private Texture2D splash;
        private Texture2D explosion;
        private Texture2D radar_Sheet;
        private ContentManager content;
        private SpriteFont hudFont;

        public Texture2D WaterTileTexture1
        {
            get
            {
                return waterTileTexture;
            }
        }

        public Texture2D DestroyerTexture1
        {
            get
            {
                return destroyerTexture;
            }
        }

        public Texture2D SubmarineTexture
        {
            get
            {
                return submarineTexture;
            }
        }

        public Texture2D BattleshipTexture
        {
            get
            {
                return battleshipTexture;
            }
        }

        public Texture2D HangarshipTexture
        {
            get
            {
                return hangarshipTexture;
            }
        }

        public Texture2D Splash
        {
            get
            {
                return splash;
            }
        }

        public Texture2D Explosion
        {
            get
            {
                return explosion;
            }
        }

        public Texture2D Radar_Sheet
        {
            get
            {
                return radar_Sheet;
            }
        }

        public SpriteFont HudFont
        {
            get
            {
                return hudFont;
            }
        }

        public Common(ContentManager content)
        {
            this.content = content;
        }

        public void LoadPictures()
        {
            waterTileTexture = content.Load<Texture2D>(@"watertile");
            destroyerTexture = content.Load<Texture2D>(@"ship2x2");
            submarineTexture = content.Load<Texture2D>(@"ship3x3");
            battleshipTexture = content.Load<Texture2D>(@"ship4x4");
            hangarshipTexture = content.Load<Texture2D>(@"ship5x5");
            splash = content.Load<Texture2D>(@"splash");
            explosion = content.Load<Texture2D>(@"explosion");
            radar_Sheet = content.Load<Texture2D>(@"radar_spritesheet");

        }

        public void LoadFonts()
        {
            hudFont = content.Load<SpriteFont>(@"HUDFont");
        }

    }
}
