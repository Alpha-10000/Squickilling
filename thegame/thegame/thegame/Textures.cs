﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace thegame
{
    class Textures
    {
        public static Texture2D plateform_texture;
        public static Texture2D mario_texture;
        public static  SpriteFont font_texture;
        public static Song openingSound_Effect;
        public static SoundEffect buttonSound_Effect;

        public static void load(ContentManager cm)
        {
            plateform_texture = cm.Load<Texture2D>(@"plateforme");
            mario_texture = cm.Load<Texture2D>(@"mario");
            font_texture = cm.Load<SpriteFont>(@"FPS");
            openingSound_Effect = cm.Load<Song>("audio\\opening");
            buttonSound_Effect = cm.Load<SoundEffect>("audio\\button");
        }
    }
}
