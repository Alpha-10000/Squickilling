using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    public enum drawable_type
    {
        menu_main_page,
        Plateform_default,

        // Buches
        autumnBuche,
        winterBuche,
        springBuche,
        summerBuche,

        // Trees
        tree,
        tree_autumn_entrance,
        tree_autumn_entrance_inside,
        tree_autumn_exit,
        tree_autumn_exit_inside,
        winterTree,
        tree_winter_entrance,
        tree_winter_entrance_inside,
        tree_winter_exit,
        tree_winter_exit_inside,
        springTree,
        springTreeHarmed,
        tree_spring_entrance,
        tree_spring_entrance_inside,
        tree_spring_exit,
        tree_spring_exit_inside,
        summerTree,
        summerTreeHarmed,
        summerTreeHarmed2,
        tree_summer_entrance,
        tree_summer_entrance_inside,

        font,
        // Background
        Background,
        AutumnGround,
        WinterGround,
        SpringGround,
        SummerGround,

        vid,
        game_over_vid,
        health,
        Nut,
        Acorn,
        mine_gray,
        mine_red,
        medecine,
        excavatorArm,
        Squirrel,
        axe,
        axe2
    }

    public class Drawable
    {
        public Texture2D image;
        public static Texture2D vidTexture;
        
        public SpriteFont _font;
        public SpriteFont _fontTitle;
        public SpriteFont _fonthelp;
        public SpriteFont _normalfont;
        public SpriteFont _multifont;

        public Drawable(drawable_type drawable_t)
        {
            switch (drawable_t)
            {
                case drawable_type.menu_main_page:
                    image = Textures.menu_main_page;
                    break;
                // Buches
                case drawable_type.autumnBuche:
                    image = Textures.buche_texture;
                    break;
                case drawable_type.winterBuche:
                    image = Textures.buche_texture_winter;
                    break;
                case drawable_type.springBuche:
                    image = Textures.buche_texture_spring;
                    break;
                case drawable_type.summerBuche:
                    image = Textures.buche_texture_summer;
                    break;

                // Trees
                case drawable_type.tree:
                    image = Textures.tree_texture;
                    break;
                case drawable_type.tree_autumn_entrance:
                    image = Textures.tree_autumn_entrance;
                    break;
                case drawable_type.tree_autumn_entrance_inside:
                    image = Textures.tree_autumn_entrance_inside;
                    break;
                case drawable_type.tree_autumn_exit:
                    image = Textures.tree_autumn_exit;
                    break;
                case drawable_type.tree_autumn_exit_inside:
                    image = Textures.tree_autumn_exit_inside;
                    break;
                case drawable_type.winterTree:
                    image = Textures.tree_winter_texture;
                    break;
                case drawable_type.tree_winter_entrance:
                    image = Textures.tree_winter_entrance;
                    break;
                case drawable_type.tree_winter_entrance_inside:
                    image = Textures.tree_winter_entrance_inside;
                    break;
                case drawable_type.tree_winter_exit:
                    image = Textures.tree_winter_exit;
                    break;
                case drawable_type.tree_winter_exit_inside:
                    image = Textures.tree_winter_exit_inside;
                    break;
                case drawable_type.springTree:
                    image = Textures.tree_spring_texture;
                    break;
                case drawable_type.springTreeHarmed:
                    image = Textures.tree_spring_harmed_texture;
                    break;
                case drawable_type.tree_spring_entrance:
                    image = Textures.tree_spring_entrance;
                    break;
                case drawable_type.tree_spring_entrance_inside:
                    image = Textures.tree_spring_entrance_inside;
                    break;
                case drawable_type.tree_spring_exit:
                    image = Textures.tree_spring_exit;
                    break;
                case drawable_type.tree_spring_exit_inside:
                    image = Textures.tree_spring_exit_inside;
                    break;
                case drawable_type.summerTree:
                    image = Textures.tree_summer_texture;
                    break;
                case drawable_type.summerTreeHarmed:
                    image = Textures.tree_summer_harmed_texture;
                    break;
                case drawable_type.summerTreeHarmed2:
                    image = Textures.tree_summer_harmed2_texture;
                    break;
                case drawable_type.tree_summer_entrance:
                    image = Textures.tree_summer_entrance;
                    break;
                case drawable_type.tree_summer_entrance_inside:
                    image = Textures.tree_summer_entrance_inside;
                    break;

                case drawable_type.font:
                    _font = Textures.font_texture;
                    _fontTitle = Textures.fontTitle_texture;
                    _fonthelp = Textures.fonthelp_texture;
                    _normalfont = Textures.fontnormal_texture;
                    _multifont = Textures.multi;
                    break;

                // Background
                case drawable_type.AutumnGround:
                    image = Textures.autumn_ground_texture;
                    break;
                case drawable_type.WinterGround:
                    image = Textures.winter_ground_texture;
                    break;
                case drawable_type.SpringGround:
                    image = Textures.spring_ground_texture;
                    break;
                case drawable_type.SummerGround:
                    image = Textures.summer_ground_texture;
                    break;
                case drawable_type.Background:
                    image = Textures.autumnBackground;
                    break;

                case drawable_type.Nut:
                    image = Textures.nut_texture;
                    break;
                case drawable_type.Acorn:
                    image = Textures.acorn_texture;
                    break;
                case drawable_type.health:
                    image = Textures.healthBar_texture;
                    break;
                case drawable_type.mine_gray:
                    image = Textures.mine_gray;
                    break;
                case drawable_type.mine_red:
                    image = Textures.mine_red;
                    break;
                case drawable_type.medecine:
                    image = Textures.medecine;
                    break;
                case drawable_type.excavatorArm:
                    image = Textures.excavatorArm;
                    break;
                case drawable_type.Squirrel:
                    image = Textures.SquirrelMenu;
                    break;
                case drawable_type.axe:
                    image = Textures.axe;
                    break;
                case drawable_type.axe2:
                    image = Textures.axe2;
                    break;
               default:
                    image = null;
                    break;
            }
        }

        public void Draw(SpriteBatch sb, Vector2 pos) /* To show image */
        {
           sb.Draw(image, pos, Color.White);
        }

         public void Draw(SpriteBatch sb, Vector2 position, float scale) /* To show image */
        {
            sb.Draw(image, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
         public void Draw(SpriteBatch sb, Vector2 position, float rotation, bool bullshit) /* To show image */
         {
             sb.Draw(image, position, null, Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
         }


        public void Draw(SpriteBatch sb, string text, Vector2 pos, Color color, string Type) /* To show text */
        {
            if (Type == "normal")
                sb.DrawString(_normalfont, text, pos, color);
            else if (Type == "help")
                sb.DrawString(_fonthelp, text, pos, color);
            else if (Type == "multi")
                sb.DrawString(_multifont, text, pos, color);
            else if (Type == "menu")
            {
                text = text.ToUpper();
                sb.DrawString(_font, text, pos, color);
            }
            else
                sb.DrawString(_fontTitle, text, pos, color);
        }
    }
}
