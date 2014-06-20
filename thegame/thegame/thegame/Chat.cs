using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace thegame
{
    class Chat
    {
        public static Textbox text;
        string message;

        public Chat()
        {
            text = new Textbox(0,Game1.graphics.PreferredBackBufferHeight - 33, Game1.graphics.PreferredBackBufferWidth, 30);
        }

        public void Update(GameTime gametime)
        {
            text.Update(gametime);
            if (Inputs.isKeyRelease(Keys.Enter))
            {
                message = text.text;
                text.Reset();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            text.Display(sb, false);
        }

    }
}
