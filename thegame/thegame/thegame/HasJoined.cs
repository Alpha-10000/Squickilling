using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    class HasJoined
    {


        private Rectangle TheBox = new Rectangle(50, 50, 750, 400);
        private Button go_back;
        public bool goback = false;

        public HasJoined()
        {
            go_back = new Button("Go back", 620, 10, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
        }

        public void Update(GameTime gametime)
        {
            go_back.Update();
            if (go_back.Clicked)
                goback = true;

            
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            if (!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0, 0), Color.White);
            else
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5), Color.White);

            go_back.Display(sb);



            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Friends game", AlignType.MiddleCenter, new Rectangle(TheBox.X, TheBox.Y, TheBox.Width / 2, 60));
            sb.Draw(Textures.hitbox, new Rectangle(TheBox.X, TheBox.Y + 52, TheBox.Width, 1), Color.White);

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "You have joined a game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));



            sb.End();
        }


    }
}
