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

        private Rectangle contentRectangle, actionbutton1, actionbutton2;

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
                actionbutton1 = new Rectangle(0, 0, (int)spritefont.MeasureString(action1).X + 35, (int)spritefont.MeasureString(action1).Y + 10);
                Vector2 getalgin = Tools.GetVectorAlgin(new Rectangle(contentRectangle.X, contentRectangle.Y + 54 + heightext * content.Length + 20, WidthPopup / 2, 60), actionbutton1, AlignType.MiddleCenter);
                actionbutton1 = new Rectangle((int)getalgin.X, (int)getalgin.Y, actionbutton1.Width, actionbutton1.Height);
                actionbutton2 = new Rectangle(0, 0, (int)spritefont.MeasureString(action2).X + 35, (int)spritefont.MeasureString(action2).Y + 10);
                getalgin = Tools.GetVectorAlgin(new Rectangle(contentRectangle.X + WidthPopup / 2, contentRectangle.Y + 54 + heightext * content.Length + 20, WidthPopup / 2, 60), actionbutton1, AlignType.MiddleCenter);
                actionbutton2 = new Rectangle((int)getalgin.X, (int)getalgin.Y, actionbutton2.Width, actionbutton2.Height);
            }
            else
            {
                actionbutton1 = new Rectangle(0, 0, (int)spritefont.MeasureString(action1).X + 35, (int)spritefont.MeasureString(action1).Y + 10);
                Vector2 getalgin = Tools.GetVectorAlgin(new Rectangle(contentRectangle.X, contentRectangle.Y + 54 + heightext * content.Length + 20, WidthPopup, 60), actionbutton1, AlignType.MiddleCenter);
                actionbutton1 = new Rectangle((int)getalgin.X, (int)getalgin.Y, actionbutton1.Width, actionbutton1.Height);
            }

        }

        public void Update(GameTime gametime)
        {
            if (Inputs.isLMBClick() && actionbutton1.Contains(Inputs.getMousePoint()))
                action1bool = true;
            if (is2 && Inputs.isLMBClick() && actionbutton2.Contains(Inputs.getMousePoint()))
                action2bool = true;
            if (is1 && is2)
            {
                if (Inputs.isKeyRelease(Keys.Left))
                    action1bool = true;
                if (Inputs.isKeyRelease(Keys.Right))
                    action2bool = true;
            }
            else
                if (Inputs.isKeyRelease(Keys.Enter))
                    action1bool = true;

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
            sb.Draw(Textures.hitbox, contentRectangle, Color.Beige * transparency);//content
            Tools.DisplayBorder(sb, Color.Black * transparency, contentRectangle, 4);//title border
            Tools.DisplayAlignedText(sb, Color.Black * transparency, spritefont, title, AlignType.MiddleCenter, new Rectangle(contentRectangle.X, contentRectangle.Y, contentRectangle.Width, 50));//title
            sb.Draw(Textures.hitbox, new Rectangle(contentRectangle.X, contentRectangle.Y + 50, contentRectangle.Width, 4), Color.Black * transparency);//black line after title
            for (int i = 0; i < content.Length; i++)
                Tools.DisplayAlignedText(sb, Color.Black * transparency, spritefont, content[i], AlignType.MiddleCenter, new Rectangle(contentRectangle.X, contentRectangle.Y + 54 + i * heightext, contentRectangle.Width, 60));//text content

            if (is1 && is2)
            {
                sb.Draw(Textures.hitbox, actionbutton1, Color.Red * transparency);
                Tools.DisplayBorder(sb, Color.Black * transparency, actionbutton1, 4);
                Tools.DisplayAlignedText(sb, Color.Black * transparency, spritefont, action1, AlignType.MiddleCenter, actionbutton1);
                sb.Draw(Textures.hitbox, actionbutton2, Color.Red * transparency);
                Tools.DisplayBorder(sb, Color.Black * transparency, actionbutton2, 4);
                Tools.DisplayAlignedText(sb, Color.Black * transparency, Textures.font_texture, action2, AlignType.MiddleCenter, actionbutton2);
            }
            else
            {
                sb.Draw(Textures.hitbox, actionbutton1, Color.Red * transparency);
                Tools.DisplayBorder(sb, Color.Black * transparency, actionbutton1, 4);
                Tools.DisplayAlignedText(sb, Color.Black * transparency, spritefont, action1, AlignType.MiddleCenter, actionbutton1);
            }
        }



    }
}
