using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    class MultiplayerLogin
    {

      

        private string text = "" ;

        public MultiplayerLogin()
        {
         
        }


        public void Update()
        {

        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            sb.DrawString(Textures.font_texture, "Multiplayer ", new Vector2(20, 20), Color.Black);
            sb.End();
        }

    }
}
