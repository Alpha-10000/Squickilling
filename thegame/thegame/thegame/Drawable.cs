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
        autumnBuche,
        winterBuche,
        tree,
        tree_autumn_entrance_inside,
        tree_autumn_exit,
        tree_autumn_exit_inside,
        winterTree,
        tree_winter_entrance_inside,
        font,
        Background,
        AutumnGround,
        WinterGround,
        vid,
        game_over_vid,
        health,
        Nut,
        Acorn,
        mine_grey,
        mine_red
    }

    public class Drawable
    {
        public Texture2D image;
        public static Texture2D vidTexture;
        
        public SpriteFont _font;
        public SpriteFont _fontTitle;
        public SpriteFont _fonthelp;
        public SpriteFont _normalfont;

        public Drawable(drawable_type drawable_t)
        {
            switch (drawable_t)
            {
                case drawable_type.menu_main_page:
                    image = Textures.menu_main_page;
                    break;
                case drawable_type.Plateform_default:
                    image = Textures.plateform_texture;
                    break;
                case drawable_type.autumnBuche:
                    image = Textures.buche_texture;
                    break;
                case drawable_type.winterBuche:
                    image = Textures.buche_texture_winter;
                    break;
                case drawable_type.tree:
                    image = Textures.tree_texture;
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
                case drawable_type.tree_winter_entrance_inside:
                    image = Textures.tree_winter_entrance_inside;
                    break;
                case drawable_type.font:
                    _font = Textures.font_texture;
                    _fontTitle = Textures.fontTitle_texture;
                    _fonthelp = Textures.fonthelp_texture;
                    _normalfont = Textures.fontnormal_texture;
                    break;
                case drawable_type.AutumnGround:
                    image = Textures.autumn_ground_texture;
                    break;
                case drawable_type.WinterGround:
                    image = Textures.winter_ground_texture;
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
                case drawable_type.mine_grey:
                    image = Textures.mine_grey;
                    break;
                case drawable_type.mine_red:
                    image = Textures.mine_red;
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
        public void Draw(SpriteBatch sb, string text,  Vector2 pos, Color color, string Type) /* To show text */
        {
            if (Type == "normal")
                sb.DrawString(_normalfont, text, pos, color);
            else if (Type == "help")
                sb.DrawString(_fonthelp, text, pos, color);
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
