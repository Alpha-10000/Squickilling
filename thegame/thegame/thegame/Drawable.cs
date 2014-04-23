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
        Plateform_default,
        buche,
        tree,
        tree_autumn_entrance_inside,
        font,
        Background,
        Ground,
        vid,
        game_over_vid,
        health,
        Nut,
        Acorn
    }

    public class Drawable
    {
        public Texture2D image;
        public static Texture2D vidTexture;
        
        public SpriteFont _font;
        public SpriteFont _fontTitle;
        public SpriteFont _fonthelp;

        public Drawable(drawable_type drawable_t)
        {
            switch (drawable_t)
            {
                case drawable_type.Plateform_default:
                    image = Textures.plateform_texture;
                    break;
                case drawable_type.buche:
                    image = Textures.buche_texture;
                    break;
                case drawable_type.tree:
                    image = Textures.tree_texture;
                    break;
                case drawable_type.tree_autumn_entrance_inside:
                    image = Textures.tree_autumn_entrance_inside;
                    break;
                case drawable_type.font:
                    _font = Textures.font_texture;
                    _fontTitle = Textures.fontTitle_texture;
                    _fonthelp = Textures.fonthelp_texture;
                    break;
                case drawable_type.Ground:
                    image = Textures.ground_autumn_texture;
                    break;
                case drawable_type.Background:
                    image = Textures.background;
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
                sb.DrawString(_font, text, pos, color);
            else if (Type == "help")
                sb.DrawString(_fonthelp, text, pos, color);
            else
                sb.DrawString(_fontTitle, text, pos, color);
        }
    }
}
