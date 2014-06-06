using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    class create_game
    {
        private Rectangle back_main_menu = new Rectangle(480, 20, 400, 40);
        public bool mainmenu = false;


        public void Update()
        {
            if (back_main_menu.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
                mainmenu = true;
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();

            if (!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0, 0), Color.White);
            else
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5), Color.White);

            sb.DrawString(Textures.font_texture, "Back main menu", new Vector2(back_main_menu.X, back_main_menu.Y), Color.White);

            sb.DrawString(Textures.font_texture, "Connected. id : " + Session.session_id + " email : " +  Session.session_email + " Password : "  + Session.session_password, new Vector2(20, 50), Color.White);
            sb.DrawString(Textures.font_texture, "Name :  " + Session.session_name, new Vector2(20, 100), Color.White);
            sb.End();
        }
    }
}
