﻿using System;
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
        font
    }


    public class Drawable
    {
        public Texture2D image;

        public Drawable(drawable_type drawable_t)
        {
            switch (drawable_t)
            {
                case drawable_type.Plateform_default:
                    image = Textures.plateform_texture;
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
        public void Draw(SpriteBatch sb, SpriteFont _font, string text,  Vector2 pos) /* To show text */
        {
            sb.DrawString(_font, text, pos, Color.Black);
        }
    }
}
