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
        public static Texture2D plateform_texture;  // Useless
        public static Texture2D hitbox;
        public static Texture2D mario_texture;
        public static Texture2D menu_main_page;

        public static Texture2D autumn_ground_texture;
        public static Texture2D winter_ground_texture;
        public static Texture2D buche_texture;
        public static Texture2D buche_texture_winter;
        public static Texture2D tree_texture;
        public static Texture2D tree_winter_texture;
        public static Texture2D tree_autumn_entrance;
        public static Texture2D tree_autumn_entrance_inside;
        public static Texture2D tree_winter_entrance_inside;
        public static Texture2D tree_autumn_exit;
        public static Texture2D tree_autumn_exit_inside;
        public static Texture2D white_tree;
        
        public static Texture2D autumnBackground;
        public static Texture2D winterBackground;

        public static Texture2D explosion;
        public static Texture2D mine_grey;
        public static Texture2D mine_red;
        public static Texture2D medecine;

        public static Texture2D pausedTexture;
        public static Rectangle pausedRectangle;
        public static Texture2D game_overTexture_en;
        public static Texture2D game_overTexture_fr;
        public static Texture2D game_overTexture_ne;
        public static SpriteFont font_texture;
        public static SpriteFont fontTitle_texture;
        public static SpriteFont fonthelp_texture;
        public static SpriteFont fontnormal_texture;


        public static SoundEffect buttonSound_Effect;
        public static SoundEffect gameSound_Effect;
        public static SoundEffect gameSound_EffectMenu;
        public static SoundEffect gameSound_EffectWinter;
        public static SoundEffect gameExplosion_Effect;
        public static SoundEffect gamePunch_Effect;

        public static Video vid;
        public static Texture2D nut_texture;
        public static Texture2D acorn_texture;
        public static Texture2D healthBar_texture;
        public static Texture2D eraser;
       
        
        public static Button btnPlay, btnQuit, btnMenu;

        public static Texture2D snowdrop;
        

        public static void load(ContentManager cm)
        {
            plateform_texture = cm.Load<Texture2D>(@"plateforme");  // Useless
            mario_texture = cm.Load<Texture2D>(@"mario");
            eraser = cm.Load<Texture2D>(@"erase");
            hitbox = cm.Load<Texture2D>(@"blank");
            menu_main_page = cm.Load<Texture2D>(@"Menu_Main_Page");
            font_texture = cm.Load<SpriteFont>(@"FPS");
            fontnormal_texture = cm.Load<SpriteFont>(@"normal");
            fontTitle_texture = cm.Load<SpriteFont>(@"Title");
            fonthelp_texture = cm.Load<SpriteFont>(@"fontHelp");
            buttonSound_Effect = cm.Load<SoundEffect>("audio\\button");
            gameSound_Effect = cm.Load<SoundEffect>("audio\\autumn");
            gameSound_EffectWinter = cm.Load<SoundEffect>("audio\\winter");
            gameSound_EffectMenu = cm.Load<SoundEffect>("audio\\menu");
            gameExplosion_Effect = cm.Load<SoundEffect>("audio\\explosion");
            gamePunch_Effect = cm.Load<SoundEffect>("audio\\punch");
            pausedTexture = cm.Load<Texture2D>(@"Paused");
            pausedRectangle = new Rectangle(0, 0, pausedTexture.Width, pausedTexture.Height);
            nut_texture = cm.Load<Texture2D>(@"Nut");
            acorn_texture = cm.Load<Texture2D>(@"Acorn_test2");
            healthBar_texture = cm.Load<Texture2D>(@"HealthBar");
            explosion = cm.Load<Texture2D>(@"explosion-sprite-sheet-i0");
            mine_grey = cm.Load<Texture2D>(@"Mine_grey");
            mine_red = cm.Load<Texture2D>(@"Mine_red");
            medecine = cm.Load<Texture2D>(@"Medecine3");
            white_tree = cm.Load<Texture2D>(@"White_Tree");

            // Autumn
            autumn_ground_texture = cm.Load<Texture2D>(@"Autumn_Ground");
            tree_texture = cm.Load<Texture2D>(@"Tree_autumn3");
            tree_autumn_entrance = cm.Load<Texture2D>(@"tree");
            tree_autumn_entrance_inside = cm.Load<Texture2D>(@"Tree_autumn_entrance_inside");
            tree_autumn_exit = cm.Load<Texture2D>(@"tree_autumn_exit");
            tree_autumn_exit_inside = cm.Load<Texture2D>(@"Tree_autumn_exit_inside");
            autumnBackground = cm.Load<Texture2D>(@"Autumn_Background");
            buche_texture = cm.Load<Texture2D>(@"Buche_test");

            // Winter
            winter_ground_texture = cm.Load<Texture2D>(@"Winter_Ground2");
            tree_winter_texture = cm.Load<Texture2D>(@"Tree_winter");
            tree_winter_entrance_inside = cm.Load<Texture2D>(@"Tree_test_winter_entrance_inside");
            winterBackground = cm.Load<Texture2D>(@"Winter_Background");
            buche_texture_winter = cm.Load<Texture2D>(@"Buche_winter_test");

            //Intro
            vid = cm.Load<Video>(@"video\\vid");


            if (Instances.language_pause == "french")
            {
                btnPlay = new Button();
                btnMenu = new Button();
                btnQuit = new Button();
                btnPlay.Load(cm.Load<Texture2D>(@"MenuPause_language\\Play_fr"), new Vector2(313, 183));
                btnMenu.Load(cm.Load<Texture2D>(@"MenuPause_language\\Menu_fr"), new Vector2(318, 253));
                btnQuit.Load(cm.Load<Texture2D>(@"MenuPause_language\\Quit_fr"), new Vector2(307, 323));
            }
            else 
            {
                btnPlay = new Button();
                btnMenu = new Button();
                btnQuit = new Button();
                btnPlay.Load(cm.Load<Texture2D>(@"MenuPause_language\\Play_en"), new Vector2(313, 183));
                btnMenu.Load(cm.Load<Texture2D>(@"MenuPause_language\\Menu_en"), new Vector2(318, 253));
                btnQuit.Load(cm.Load<Texture2D>(@"MenuPause_language\\Quit_en"), new Vector2(307, 323));
            }

            // Game Over
            game_overTexture_en = cm.Load<Texture2D>(@"game_over_en");
            game_overTexture_fr = cm.Load<Texture2D>(@"game_over_fr");
            game_overTexture_ne = cm.Load<Texture2D>(@"game_over_ne");

            //Particles
            snowdrop = cm.Load<Texture2D>("Sprites\\snowdrop");
        }

    }
}
