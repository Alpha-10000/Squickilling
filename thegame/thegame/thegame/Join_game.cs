﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    class Join_game
    {
        private Rectangle go_back = new Rectangle(600, 20, 600, 40);
        public bool goback = false;


        /* Content create */
        private Rectangle titlejoin = new Rectangle(100, 77, 600, 50);
        private Rectangle contentjoin = new Rectangle(100, 123, 600, 350);


        private Popup popup;


        public void Update()
        {
            if ((go_back.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick()) || Inputs.isKeyRelease(Microsoft.Xna.Framework.Input.Keys.Back))
                goback = true;

            for (int i = 0; i <= 2; i++)
                if (new Rectangle(contentjoin.X + 300, contentjoin.Y + 50 + i * 50, 200, 40).Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
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

            sb.Draw(Textures.hitbox, titlejoin, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, titlejoin, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Join current game", AlignType.MiddleCenter, titlejoin);
            sb.Draw(Textures.hitbox, contentjoin, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, contentjoin, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "The game created by my friends", AlignType.MiddleCenter, new Rectangle(contentjoin.X, contentjoin.Y, contentjoin.Width, 50));

            for (int i = 0; i <= 2; i++)
            {
                Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "game " + i + " by [...]", AlignType.MiddleCenter, new Rectangle(contentjoin.X + 100, contentjoin.Y + 50 + i * 50, 100, 50));
                sb.Draw(Textures.hitbox, new Rectangle(contentjoin.X + 300, contentjoin.Y + 50 + i * 50, 200, 40), Color.Red);
                Tools.DisplayBorder(sb, Color.Black, new Rectangle(contentjoin.X + 300, contentjoin.Y + 50 + i * 50, 200, 40), 4);
                Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Join", AlignType.MiddleCenter, new Rectangle(contentjoin.X + 300, contentjoin.Y + 50 + i * 50, 200, 40));
            }


            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Join game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            if (popup != null)
                popup.Display(sb);

            sb.End();
        }
    }
}