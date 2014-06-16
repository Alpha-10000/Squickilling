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
        #region Texture
        public static Texture2D plateform_texture;  // Useless
        public static Texture2D hitbox;
        public static Texture2D mario_texture;
        public static Texture2D menu_main_page;

        public static Texture2D autumn_ground_texture;
        public static Texture2D winter_ground_texture;
        public static Texture2D spring_ground_texture;
        public static Texture2D summer_ground_texture;

        public static Texture2D buche_texture;
        public static Texture2D buche_texture_winter;
        public static Texture2D buche_texture_spring;
        public static Texture2D buche_texture_summer;

        public static Texture2D tree_texture;
        public static Texture2D tree_winter_texture;
        public static Texture2D tree_spring_texture;
        public static Texture2D tree_spring_harmed_texture;
        public static Texture2D tree_summer_texture;
        public static Texture2D tree_summer_harmed_texture;
        public static Texture2D tree_summer_cut_texture;
        public static Texture2D tree_summer_harmed2_texture;
        public static Texture2D tree_autumn_entrance;
        public static Texture2D tree_autumn_entrance_inside;
        public static Texture2D tree_winter_entrance;
        public static Texture2D tree_winter_entrance_inside;
        public static Texture2D tree_spring_entrance;
        public static Texture2D tree_spring_entrance_inside;
        public static Texture2D tree_summer_entrance_inside;
        public static Texture2D tree_summer_entrance;
        public static Texture2D tree_autumn_exit;
        public static Texture2D tree_autumn_exit_inside;
        public static Texture2D tree_winter_exit;
        public static Texture2D tree_winter_exit_inside;
        public static Texture2D tree_spring_exit;
        public static Texture2D tree_spring_exit_inside;

        public static Texture2D white_tree;
        public static Texture2D excavatorArm;
        public static Texture2D SquirrelMenu;
        public static Texture2D animation_mine;

        public static Texture2D autumnBackground;
        public static Texture2D winterBackground;
        public static Texture2D springBackground;
        public static Texture2D summerBackground;

        public static Texture2D ButtonLeft;
        public static Texture2D ButtonMiddle;
        public static Texture2D ButtonRight;

        public static Texture2D explosion;
        public static Texture2D mine_grey;
        public static Texture2D mine_red;
        public static Texture2D bloodEmitter;
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
        public static SpriteFont font_pause;

        public static SoundEffect buttonSound_Effect;
        public static SoundEffect gameSound_Effect;
        public static SoundEffect gameSound_EffectMenu;
        public static SoundEffect gameSound_EffectWinter;
        public static SoundEffect gameSound_EffectSpring;
        public static SoundEffect gameSound_EffectSummer;
        public static SoundEffect gameExplosion_Effect;
        public static SoundEffect gamePunch_Effect;

        public static Video vid;
        public static Texture2D nut_texture;
        public static Texture2D acorn_texture;
        public static Texture2D healthBar_texture;
        public static Texture2D eraser;


        public static Button btnPlay_Autumn, btnQuit_Autumn, btnMenu_Autumn, btnPlay_Winter, btnQuit_Winter, btnMenu_Winter, btnMute;

        public static Texture2D snowdrop;
        public static Texture2D leaf_brown, leaf_orange, leaf_red, leaf_yellow;
        #endregion

        public static void load(ContentManager cm)
        {
            // Sprite Fonts
            font_texture = cm.Load<SpriteFont>(@"FPS");
            fontnormal_texture = cm.Load<SpriteFont>(@"normal");
            fontTitle_texture = cm.Load<SpriteFont>(@"Title");
            fonthelp_texture = cm.Load<SpriteFont>(@"fontHelp");
            font_pause = cm.Load<SpriteFont>(@"PauseMenuFont");

            // Sounds
            buttonSound_Effect = cm.Load<SoundEffect>("audio\\button");
            gameSound_Effect = cm.Load<SoundEffect>("audio\\autumn");
            gameSound_EffectWinter = cm.Load<SoundEffect>("audio\\winter");
            gameSound_EffectSpring = cm.Load<SoundEffect>("audio\\Crimson_Fly");
            gameSound_EffectSummer = cm.Load<SoundEffect>("audio\\Echinoderm_Regeneration");
            gameSound_EffectMenu = cm.Load<SoundEffect>("audio\\menu");
            gameExplosion_Effect = cm.Load<SoundEffect>("audio\\explosion");
            gamePunch_Effect = cm.Load<SoundEffect>("audio\\punch");

            // Texture 2D
            menu_main_page = cm.Load<Texture2D>(@"Menu_Main_Page");
            plateform_texture = cm.Load<Texture2D>(@"plateforme");  // Useless
            mario_texture = cm.Load<Texture2D>(@"mario");
            eraser = cm.Load<Texture2D>(@"erase");
            hitbox = cm.Load<Texture2D>(@"blank");
            pausedTexture = cm.Load<Texture2D>(@"Paused");
            pausedRectangle = new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 20);
            nut_texture = cm.Load<Texture2D>(@"Nut");
            acorn_texture = cm.Load<Texture2D>(@"Acorn_test2");
            healthBar_texture = cm.Load<Texture2D>(@"HealthBar");
            explosion = cm.Load<Texture2D>(@"explosion-sprite-sheet-i0");
            mine_grey = cm.Load<Texture2D>(@"Mine_grey");
            mine_red = cm.Load<Texture2D>(@"Mine_red");
            medecine = cm.Load<Texture2D>(@"Medecine3");
            white_tree = cm.Load<Texture2D>(@"White_Tree");
            excavatorArm = cm.Load<Texture2D>(@"Excavator_arm2");
            SquirrelMenu = cm.Load<Texture2D>(@"Squirrel_menu");
            animation_mine = cm.Load<Texture2D>(@"mine-animation");
            bloodEmitter = cm.Load<Texture2D>(@"blood_spritesheet");

            // Autumn
            autumn_ground_texture = cm.Load<Texture2D>(@"Autumn/Autumn_Ground");
            tree_texture = cm.Load<Texture2D>(@"Autumn/Tree_autumn3");
            tree_autumn_entrance = cm.Load<Texture2D>(@"Autumn/tree");
            tree_autumn_entrance_inside = cm.Load<Texture2D>(@"Autumn/Tree_autumn_entrance_inside");
            tree_autumn_exit = cm.Load<Texture2D>(@"Autumn/Autumn_Tree_exit");
            tree_autumn_exit_inside = cm.Load<Texture2D>(@"Autumn/Autumn_Tree_exit_inside");
            autumnBackground = cm.Load<Texture2D>(@"Autumn/Autumn_Background");
            buche_texture = cm.Load<Texture2D>(@"Autumn/Buche_test");
            leaf_brown = cm.Load<Texture2D>(@"Autumn/Autumn_leaf_brown");
            leaf_orange = cm.Load<Texture2D>(@"Autumn/Autumn_leaf_orange");
            leaf_red = cm.Load<Texture2D>(@"Autumn/Autumn_leaf_red");
            leaf_yellow = cm.Load<Texture2D>(@"Autumn/Autumn_leaf_yellow");

            // Winter
            winter_ground_texture = cm.Load<Texture2D>(@"Winter/Winter_ground2");
            tree_winter_texture = cm.Load<Texture2D>(@"Winter/Tree_winter");
            tree_winter_entrance = cm.Load<Texture2D>(@"Winter/Winter_tree_entrance");
            tree_winter_entrance_inside = cm.Load<Texture2D>(@"Winter/Winter_tree_entrance_inside");
            tree_winter_exit = cm.Load<Texture2D>(@"Winter/Winter_Tree_exit");
            tree_winter_exit_inside = cm.Load<Texture2D>(@"Winter/Winter_tree_exit_inside");
            winterBackground = cm.Load<Texture2D>(@"Winter/Winter_Background");
            buche_texture_winter = cm.Load<Texture2D>(@"Winter/Buche_winter_test");

            // Spring
            spring_ground_texture = cm.Load<Texture2D>(@"Spring/Spring_ground3");
            tree_spring_texture = cm.Load<Texture2D>(@"Spring/Spring_Tree2");
            tree_spring_harmed_texture = cm.Load<Texture2D>(@"Spring/Spring_tree_harmed");
            springBackground = cm.Load<Texture2D>(@"Spring/Spring_Background4");
            buche_texture_spring = cm.Load<Texture2D>(@"Spring/Buche_spring");
            tree_spring_entrance = cm.Load<Texture2D>(@"Spring/Spring_tree_entrance");
            tree_spring_entrance_inside = cm.Load<Texture2D>(@"Spring/Spring_tree_entrance_inside");
            tree_spring_exit = cm.Load<Texture2D>(@"Spring/Spring_tree_exit");
            tree_spring_exit_inside = cm.Load<Texture2D>(@"Spring/Spring_tree_exit_inside");


            // Summer
            summer_ground_texture = cm.Load<Texture2D>(@"Summer/Summer_Ground2");
            tree_summer_texture = cm.Load<Texture2D>(@"Summer/Summer_Tree");
            tree_summer_harmed_texture = cm.Load<Texture2D>(@"Summer/Summer_tree_harmed");
            tree_summer_harmed2_texture = cm.Load<Texture2D>(@"Summer/Summer_tree_harmed2");
            summerBackground = cm.Load<Texture2D>(@"Summer/Summer_Background2");
            buche_texture_summer = cm.Load<Texture2D>(@"Summer/Summer_Buche");
            tree_summer_entrance = cm.Load<Texture2D>(@"Summer/Summer_tree_entrance");
            tree_summer_entrance_inside = cm.Load<Texture2D>(@"Summer/Summer_tree_entrance_inside");
            tree_summer_cut_texture = cm.Load<Texture2D>(@"Summer/Summer_Cut_tree");

            //Intro
            vid = cm.Load<Video>(@"video\\vid");

            //button
            ButtonLeft = cm.Load<Texture2D>(@"Button/Bouton_left");
            ButtonMiddle = cm.Load<Texture2D>(@"Button/Bouton_middle");
            ButtonRight = cm.Load<Texture2D>(@"Button/Bouton_right");





            // Game Over
            game_overTexture_en = cm.Load<Texture2D>(@"game_over_en");
            game_overTexture_fr = cm.Load<Texture2D>(@"game_over_fr");
            game_overTexture_ne = cm.Load<Texture2D>(@"game_over_ne");

            //Particles
            snowdrop = cm.Load<Texture2D>("Sprites\\snowdrop");
        }

    }
}
