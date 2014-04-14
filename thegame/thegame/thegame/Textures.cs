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
        public static Texture2D tree_autumn_entrance_inside;
        public static Texture2D pausedTexture;
        public static Texture2D game_overTexture;
        public static Rectangle pausedRectangle;
        public static SpriteFont font_texture;
        public static SpriteFont fontTitle_texture;
        public static Song openingSound_Effect;
        public static SoundEffect openingSound_Effect1;
        public static SoundEffect buttonSound_Effect;
        public static SoundEffect gameSound_Effect;
        public static Texture2D background;
        public static Video vid;
        public static Texture2D nut_texture;
        public static Texture2D acorn_texture;
        public static Texture2D healthBar_texture;
        
        public static Button btnPlay, btnQuit, btnMenu;
        

        public static void load(ContentManager cm)
        {
            plateform_texture = cm.Load<Texture2D>(@"plateforme");
            mario_texture = cm.Load<Texture2D>(@"mario");
            buche_texture = cm.Load<Texture2D>(@"Buche_test");
            hitbox = cm.Load<Texture2D>(@"blank");
            tree_texture = cm.Load<Texture2D>(@"tree");
            tree_autumn_entrance_inside = cm.Load<Texture2D>(@"Tree_Autumn_entrance_inside");
            ground_texture = cm.Load<Texture2D>(@"Ground");
            font_texture = cm.Load<SpriteFont>(@"FPS");
            fontTitle_texture = cm.Load<SpriteFont>(@"Title");
            openingSound_Effect = cm.Load<Song>("audio\\opening");
            openingSound_Effect1 = cm.Load<SoundEffect>("audio\\opening1");
            buttonSound_Effect = cm.Load<SoundEffect>("audio\\button");
            gameSound_Effect = cm.Load<SoundEffect>("audio\\game");
            background = cm.Load<Texture2D>(@"Autumn_Background9");
            pausedTexture = cm.Load<Texture2D>(@"Paused");
            pausedRectangle = new Rectangle(0, 0, pausedTexture.Width, pausedTexture.Height);
            nut_texture = cm.Load<Texture2D>(@"Nut");
            acorn_texture = cm.Load<Texture2D>(@"Acorn_test2");
            healthBar_texture = cm.Load<Texture2D>(@"HealthBar");
            //Intro
            vid = cm.Load<Video>(@"video\\vid");
           
            btnPlay = new Button();
            btnMenu = new Button();
            btnQuit = new Button();
            btnPlay.Load(cm.Load<Texture2D>(@"Play"), new Vector2(313, 183));
            btnMenu.Load(cm.Load<Texture2D>(@"Menu"), new Vector2(318, 253));
            btnQuit.Load(cm.Load<Texture2D>(@"Quit"), new Vector2(307, 323));

            // Game Over
            game_overTexture = cm.Load<Texture2D>(@"game_over");
            
        }
    }
}
