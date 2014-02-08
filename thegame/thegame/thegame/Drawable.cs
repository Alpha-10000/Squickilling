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
        font
    }


    public class Drawable
    {
        public Texture2D image;
        public SpriteFont _font;
        public SpriteFont _fontTitle;

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
                case drawable_type.font:
                    _font = Textures.font_texture;
                    _fontTitle = Textures.fontTitle_texture;
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
            {
                sb.DrawString(_font, text, pos, color);
            }
            else
            {
                sb.DrawString(_fontTitle, text, pos, color);
            }
        }
    }
}
