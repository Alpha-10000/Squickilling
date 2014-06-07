using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thegame
{
    class Popup
    {

        private Rectangle popuptitle;

        public bool action1bool, action2bool;
        private string action1, action2, title, content;

        private int WidthPopup = 400;
        private int heightPopup = 300;

        private Rectangle contentRectangle;

        public Popup(string text, string action1, string action2, string title, string content)//if one action set action2 to ""
        {
            action1bool = action2bool = false;
            this.action1 = action1;
            this.action2 = action2;
            this.title = title;
            this.content = content;

            contentRectangle = new Rectangle(158, 158, WidthPopup, heightPopup);
        }

        public void Update()
        {
            if (new Rectangle(contentRectangle.X + 100, contentRectangle.Y + 154, 100, 60).Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
                action1bool = true;
        }

        public void Display(SpriteBatch sb)
        {
            sb.Draw(Textures.hitbox, contentRectangle, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, contentRectangle, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, title, AlignType.MiddleCenter, new Rectangle(contentRectangle.X, contentRectangle.Y, contentRectangle.Width, 50));
            sb.Draw(Textures.hitbox, new Rectangle(contentRectangle.X, contentRectangle.Y + 50, contentRectangle.Width, 4), Color.Black);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, content, AlignType.MiddleCenter, new Rectangle(contentRectangle.X, contentRectangle.Y + 54, contentRectangle.Width, 60));
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, action1, AlignType.MiddleCenter, new Rectangle(contentRectangle.X + 100, contentRectangle.Y + 154, 100, 60));
        }



    }
}
