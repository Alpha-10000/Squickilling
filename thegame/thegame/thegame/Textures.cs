using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace thegame
{
    class Textures
    {
        public static Texture2D plateform_texture;
        public static  SpriteFont font_texture;

        public static void load(ContentManager cm)
        {
            plateform_texture = cm.Load<Texture2D>(@"plateforme");
            font_texture = cm.Load<SpriteFont>(@"FPS");
        }
    }
}
