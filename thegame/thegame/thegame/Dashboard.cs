using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace thegame
{
    class Dashboard
    {
        private Rectangle back_main_menu = new Rectangle(600, 20, 600, 40);
        public bool mainmenu = false;

        /* Coordonnates right and left circles */
        private int xCircle = 144;
        private int yTopCircle = 177;
        private int yBottomCircle = 392;
        private int radius = 100;

        private int ColorTop = 70;
        private int ColorBottom = 70;
        private float AnimatedColor_TopCircle = 0;
        private float AnimatedColor_BottomCircle = 0;

        /* Coordonates friends */
        private Rectangle titlefriends = new Rectangle(400, 77, 380, 50);
        private Rectangle contentfriends = new Rectangle(400, 123, 380, 150);
        private Rectangle titleinvitation = new Rectangle(400, 300, 380, 50);
        private Rectangle contentinvitation = new Rectangle(400, 346, 380, 150);

        public bool Create_game { get; private set; }
        public bool Join_game { get; private set; }

        public void Update(GameTime gametime)
        {
            if (back_main_menu.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
                mainmenu = true;
            Point themouse = Inputs.getMousePoint();
            if (radius >= Math.Sqrt(Math.Pow(themouse.X -xCircle, 2) + Math.Pow(themouse.Y - yTopCircle, 2)))
            {
                if (Inputs.isLMBClick())
                    Create_game = true;

                AnimatedColor_TopCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_TopCircle > 5)
                {
                    if (ColorTop < 150)
                        ColorTop++;
                }
                else
                    AnimatedColor_TopCircle = 0;
            }
            else
            {
                AnimatedColor_TopCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_TopCircle > 50)
                {
                    if (ColorTop > 75)
                        ColorTop--;
                }
                else
                    AnimatedColor_TopCircle = 0;
            }

            if (radius >= Math.Sqrt(Math.Pow(themouse.X - xCircle, 2) + Math.Pow(themouse.Y - yBottomCircle, 2)))
            {

                if (Inputs.isLMBClick())
                    Join_game = true;

                AnimatedColor_BottomCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_BottomCircle > 5)
                {
                    if (ColorBottom < 150)
                        ColorBottom++;
                }
                else
                    AnimatedColor_BottomCircle = 0;
            }
            else
            {
                AnimatedColor_BottomCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_BottomCircle > 50)
                {
                    if (ColorBottom > 75)
                        ColorBottom--;
                }
                else
                    AnimatedColor_BottomCircle = 0;
            }

        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();

            if (!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0, 0), Color.White);
            else
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5), Color.White);

            sb.DrawString(Textures.font_texture, "Back main menu", new Vector2(back_main_menu.X, back_main_menu.Y), Color.White);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "WELCOME " + Session.session_name, AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            /* Circles */
            Tools.DrawCircle(sb, xCircle, yTopCircle, radius, 50, new Color(227, ColorTop, 73), 140);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Create a game", AlignType.MiddleCenter, new Rectangle(xCircle - radius, yTopCircle - radius, radius * 2, radius * 2));

            Tools.DrawCircle(sb, xCircle, yBottomCircle, radius, 50, new Color(227, ColorBottom, 73), 140);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Join a game", AlignType.MiddleCenter, new Rectangle(xCircle - radius, yBottomCircle - radius, radius * 2, radius * 2));

            /* MY FRIENDS */
            sb.Draw(Textures.hitbox, titlefriends, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, titlefriends, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "My friends", AlignType.MiddleCenter, titlefriends);
            sb.Draw(Textures.hitbox, contentfriends, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, contentfriends, 4);

            for(int i = 0; i <= 2; i++)
                Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Friends " + i, AlignType.MiddleCenter, new Rectangle(contentfriends.X, contentfriends.Y + i * 40, contentfriends.Width, 40));

            /* INVITATIONS */
            sb.Draw(Textures.hitbox, titleinvitation, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, titleinvitation, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Invitations", AlignType.MiddleCenter, titleinvitation);
            sb.Draw(Textures.hitbox, contentinvitation, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, contentinvitation, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "No friends request yet", AlignType.MiddleCenter, new Rectangle(contentinvitation.X, contentinvitation.Y, contentinvitation.Width, 50));


            sb.End();
        }
    }
}
