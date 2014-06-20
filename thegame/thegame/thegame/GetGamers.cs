using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    class GetGamers
    {

        public GetGamers()
        {

        }

        public void Update(GameTime gametime)
        {

        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            if (!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0, 0), Color.White);
            else
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5), Color.White);

 


            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Now let's start multiplayer mode", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

          
            sb.End();
        }



    }
}
