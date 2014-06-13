using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    class Create_game
    {
        private Rectangle go_back = new Rectangle(600, 20, 600, 40);
        public bool goback = false;

        /* Content create */
        private Rectangle titlecreate = new Rectangle(100, 77, 600, 50);
        private Rectangle contentcreate = new Rectangle(100, 123, 600, 350);

        private Popup popup;

        private Textbox essai_textbox;

        public Create_game()
        {
<<<<<<< HEAD
            essai_textbox = new Textbox(50, 50, 200, 40);
=======
            string text = "";

            //Add an event for a character being added
            
>>>>>>> origin/master
        }

        public void Update(GameTime gametime)
        {
            essai_textbox.Update(gametime);

            if ((go_back.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick()))
                goback = true;
          for(int i = 0; i<=2; i++)
              if (new Rectangle(contentcreate.X + 300, contentcreate.Y + 50 + i * 50, 200, 40).Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
                    popup = new Popup("OK", "", "Information", new string[] {"We are developping the functionnality"}, Textures.font_texture, 450);

            if (popup != null)
            {
                popup.Update();
                if (popup.action1bool)
                    popup = null;
            }
        }



        public void Display(SpriteBatch sb)
        {
            sb.Begin();


            if (!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0, 0), Color.White);
            else
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5), Color.White);

            sb.DrawString(Textures.font_texture, "Go back", new Vector2(go_back.X, go_back.Y), Color.White);

            sb.Draw(Textures.hitbox, titlecreate, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, titlecreate, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Create new game", AlignType.MiddleCenter, titlecreate);
            sb.Draw(Textures.hitbox, contentcreate, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, contentcreate, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "My friends", AlignType.MiddleCenter, new Rectangle(contentcreate.X, contentcreate.Y, contentcreate.Width, 50));

            for (int i = 0; i <= 2; i++)
            {
                Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Friends " + i, AlignType.MiddleCenter, new Rectangle(contentcreate.X + 100, contentcreate.Y + 50 + i * 50, 100, 50));
                sb.Draw(Textures.hitbox, new Rectangle(contentcreate.X + 300, contentcreate.Y + 50 + i * 50, 200, 40), Color.Red);
                Tools.DisplayBorder(sb, Color.Black, new Rectangle(contentcreate.X + 300, contentcreate.Y + 50 + i * 50, 200,  40) , 4);
                Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Invite" , AlignType.MiddleCenter, new Rectangle(contentcreate.X + 300, contentcreate.Y + 50 + i * 50, 200,  40));
            }


            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Create game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            if (popup != null)
                popup.Display(sb);

            essai_textbox.Display(sb);
            sb.End();
        }
    }
}
