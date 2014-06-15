using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace thegame
{
    class Button
    {
        private int x, y, ButtonWidth, nbMiddleButton;
        private string text;
        private Rectangle buttonHitBox;
        private SpriteFont spritefont;
        public bool Clicked = false;

        public Button(string text, int x, int y, SpriteFont spritefont)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.spritefont = spritefont;
            int Textwidth = (int)spritefont.MeasureString(text).X;

            //calcule nb buttonmiddle we will need
            nbMiddleButton = 0;
            ButtonWidth = Textures.ButtonMiddle.Width;
            while (nbMiddleButton * ButtonWidth <= Textwidth)
                nbMiddleButton++;

            buttonHitBox = new Rectangle(x, y, Textures.ButtonLeft.Width + Textures.ButtonRight.Width + ButtonWidth * (nbMiddleButton + 1), Textures.ButtonMiddle.Height);
        }

        public void Update()
        {
            Clicked = false;//very important. Do not delete
            if (Inputs.isLMBClick() &&  buttonHitBox.Contains(Inputs.getMousePoint()))
                Clicked = true;
        }

        public void Display(SpriteBatch sb)
        {
            sb.Draw(Textures.ButtonLeft, new Rectangle(x, y, Textures.ButtonRight.Width, Textures.ButtonLeft.Height), Color.White);

            //right button
            sb.Draw(Textures.ButtonRight, new Rectangle(x + (nbMiddleButton + 1) * ButtonWidth + Textures.ButtonLeft.Width - 1, y, Textures.ButtonRight.Width, Textures.ButtonRight.Height), Color.White);

            //draw middle buttons
            for(int i = 0; i <= nbMiddleButton; i++)
                sb.Draw(Textures.ButtonMiddle, new Rectangle((x + Textures.ButtonLeft.Width ) + i * ButtonWidth, y, Textures.ButtonMiddle.Width, Textures.ButtonMiddle.Height), Color.White);

           
            Tools.DisplayAlignedText(sb, Color.Black, spritefont, text, AlignType.MiddleCenter, buttonHitBox);
        }
    }
}
