using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace thegame
{
    class Popup
    {

        private Rectangle popuptitle;
        private SpriteFont spritefont;
        public bool action1bool, action2bool;
        private bool is1, is2;
        private string action1, action2, title;
        private string[] content;

        private int WidthPopup;
        private int heightPopup;
        private int heightext = 0;

        private float AnimatedAppear = 0;
        private bool animation = true;
        private float transparency = 0;
        private float TimeAnimation = 800;//milliseconds

        private Rectangle contentRectangle;
        private Button actionbutton1, actionbutton2;

        private Color contentColor = new Color(18, 19, 20);
        private Color BorderColor = Color.White;

        public Popup(string action1, string action2, string title, string[] content, SpriteFont spritefont, int WidthPopup)//if one action set action2 to ""
        {
            action1bool = action2bool = false;
            this.action1 = action1;
            this.action2 = action2;
            this.title = title;
            this.content = content;
            this.spritefont = spritefont;
            this.WidthPopup = WidthPopup;

            heightPopup = 100;//default size
            if(content.Length > 0)
            {
            this.heightext = (int)spritefont.MeasureString(content[0]).Y;
            for (int i = 0; i < content.Length; i++)
                heightPopup += heightext + 10;
            }

            heightPopup += 25;

            Vector2 AlignCenterVector2 = Tools.GetVectorAlgin(new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5),
                new Rectangle(0, 0, WidthPopup, heightPopup), AlignType.MiddleCenter);
            contentRectangle = new Rectangle((int)AlignCenterVector2.X, (int)AlignCenterVector2.Y, WidthPopup, heightPopup);

            is1 = is2 = true;
            if (action2 == "")
                is2 = false;

            if (is1 && is2)
            {
                int x = contentRectangle.X + contentRectangle.Width / 4 - (int)spritefont.MeasureString(action1).X / 4;
                int y = contentRectangle.Y + heightPopup - 50;
                int x2 = contentRectangle.X + 3 * (contentRectangle.Width / 4) -  3 * ((int)spritefont.MeasureString(action2).X / 4);
                int y2 = contentRectangle.Y + heightPopup - 50;
                actionbutton1 = new Button(action1, x, y, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
                actionbutton2 = new Button(action2, x2, y2, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
            }
            else
            {
                int x = contentRectangle.X + contentRectangle.Width / 2 - (int)spritefont.MeasureString(action1).X / 2;
                int y = contentRectangle.Y + heightPopup - 50;
                actionbutton1 = new Button(action1, x, y, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
            }

        }

        public void Update(GameTime gametime)
        {

            if (is1 && is2)
            {
                actionbutton1.Update();
                actionbutton2.Update();
                if (actionbutton1.Clicked || Inputs.isKeyRelease(Keys.Left))
                    action1bool = true;
                if (actionbutton2.Clicked || Inputs.isKeyRelease(Keys.Right))
                    action2bool = true;
            }
            else
            {
                actionbutton1.Update();
                if (actionbutton1.Clicked || Inputs.isKeyRelease(Keys.Enter))
                    action1bool = true;
            }

            if (animation)
            {
                AnimatedAppear += gametime.ElapsedGameTime.Milliseconds;
                if (AnimatedAppear < TimeAnimation)
                {
                    transparency = AnimatedAppear / TimeAnimation;
                }
                else
                {
                    AnimatedAppear = 0;
                    animation = false;
                }
            }
        }

        public void Display(SpriteBatch sb)
        {
            sb.Draw(Textures.hitbox, contentRectangle, contentColor * transparency);//content
            Tools.DisplayBorder(sb,  BorderColor * transparency, contentRectangle, 1);//title border
            Tools.DisplayAlignedText(sb, Color.White * transparency, spritefont, title, AlignType.MiddleCenter, new Rectangle(contentRectangle.X, contentRectangle.Y, contentRectangle.Width, 50));//title
            sb.Draw(Textures.hitbox, new Rectangle(contentRectangle.X, contentRectangle.Y + 50, contentRectangle.Width, 1), BorderColor * transparency);//black line after title
            for (int i = 0; i < content.Length; i++)
                Tools.DisplayAlignedText(sb, Color.White * transparency, spritefont, content[i], AlignType.MiddleCenter, new Rectangle(contentRectangle.X, contentRectangle.Y + 54 + i * heightext, contentRectangle.Width, 60));//text content

            if (is1 && is2)
            {
                actionbutton1.Display(sb);
                actionbutton2.Display(sb);
            }
            else
            {
                actionbutton1.Display(sb);
            }
        }



    }
}
