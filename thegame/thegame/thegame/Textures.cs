using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace thegame
{
    class Textures
    {
        public static Texture2D plateform_texture;
        public static Texture2D hitbox;
        public static Texture2D mario_texture;
        public static Texture2D ground_texture;
        public static Texture2D buche_texture;
        public static Texture2D tree_texture;
        public static Texture2D pausedTexture;
        public static Rectangle pausedRectangle;
        public static SpriteFont font_texture;
        public static SpriteFont fontTitle_texture;
        public static Song openingSound_Effect;
        public static SoundEffect buttonSound_Effect;
        public static SoundEffect gameSound_Effect;
        public static Texture2D background;

        public static void load(ContentManager cm)
        {
            plateform_texture = cm.Load<Texture2D>(@"plateforme");
            mario_texture = cm.Load<Texture2D>(@"mario");
            buche_texture = cm.Load<Texture2D>(@"Buche_test");
            hitbox = cm.Load<Texture2D>(@"blank");
            tree_texture = cm.Load<Texture2D>(@"tree");
            ground_texture = cm.Load<Texture2D>(@"Ground");
            font_texture = cm.Load<SpriteFont>(@"FPS");
            fontTitle_texture = cm.Load<SpriteFont>(@"Title");
            openingSound_Effect = cm.Load<Song>("audio\\opening");
            buttonSound_Effect = cm.Load<SoundEffect>("audio\\button");
            gameSound_Effect = cm.Load<SoundEffect>("audio\\game");
            background = cm.Load<Texture2D>(@"Background");
            pausedTexture = cm.Load<Texture2D>(@"Paused");
            pausedRectangle = new Rectangle(0, 0, pausedTexture.Width, pausedTexture.Height);
            
        }
    }
}
