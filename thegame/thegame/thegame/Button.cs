using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace thegame
{
    public enum COLORTYPE
    { 
        light_green,
        dark_green,
        black
    }


    class Button
    {
        private int x, y, Textwidth, Textheight, radius, CircleX1, CircleX2, CircleY1, CircleY2;
        private int Topspacing = 10;//TODO : in the constructor
        private string text;
        private SpriteFont spritefont;
        public bool Clicked = false;
        private Color border, main, borderOnClick, Defautborder;

        public Button(string text, int x, int y, SpriteFont spritefont, Color Defautborder, Color borderOnClick, Color main)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.spritefont = spritefont;
            Textwidth = (int)spritefont.MeasureString(text).X;
            Textheight = (int)spritefont.MeasureString(text).Y;

            radius = (Textheight + Topspacing) / 2 + 1;
            CircleX1 = x;
            CircleX2 = x + Textwidth;
            CircleY2 = y + (Textheight + Topspacing) / 2 + 1;
            CircleY1 = y + (Textheight + Topspacing) / 2 + 1;

            this.main = main;
            this.borderOnClick = borderOnClick;
            this.Defautborder = Defautborder;
        }

        public Button(string text, int x, int y, SpriteFont spritefont, COLORTYPE type)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.spritefont = spritefont;
            Textwidth = (int)spritefont.MeasureString(text).X;
            Textheight = (int)spritefont.MeasureString(text).Y;

            radius = (Textheight + Topspacing) / 2 + 1;
            CircleX1 = x;
            CircleX2 = x + Textwidth;
            CircleY2 = y + (Textheight + Topspacing) / 2 + 1;
            CircleY1 = y + (Textheight + Topspacing) / 2 + 1;
            if (type == COLORTYPE.light_green)
            {
                this.main = new Color(122, 184, 0);
                this.borderOnClick = Color.White;
                this.Defautborder = new Color(122, 184, 0);
            }
            if (type == COLORTYPE.dark_green)
            {
                this.main = new Color(100, 143, 0);
                this.borderOnClick = Color.White;
                this.Defautborder = new Color(100,143,0);
            } 
            if (type == COLORTYPE.black)
            {
                this.main = new Color(0,0,0);
                this.borderOnClick = Color.White;
                this.Defautborder = new Color(0,0,0);
            }
        }

        public void Update()
        {
            Point themouse = Inputs.getMousePoint();
            bool MouseIn =
                radius >= Math.Sqrt(Math.Pow(themouse.X - CircleX1, 2) + Math.Pow(themouse.Y - CircleY1, 2))
                || radius >= Math.Sqrt(Math.Pow(themouse.X - CircleX2, 2) + Math.Pow(themouse.Y - CircleY2, 2))
                || new Rectangle(x, y, Textwidth, Textheight + Topspacing).Contains(themouse);


            Clicked = false;//very important. Do not delete
            if (Inputs.isLMBClick() &&  MouseIn)
                Clicked = true;

            border = Defautborder;
            if(MouseIn)
                border = borderOnClick;
        }

        public void Display(SpriteBatch sb)
        {
            //left side
            Tools.DrawCircle(sb, new Vector2(CircleX1, CircleY1), radius, 20, main, Textheight);
            Tools.DrawCircle(sb, new Vector2(CircleX1, CircleY1), radius, 100, border);
            //right side
            Tools.DrawCircle(sb, new Vector2(CircleX2, CircleY2), radius, 20, main, Textheight);
            Tools.DrawCircle(sb, new Vector2(CircleX2, CircleY2), radius, 100, border);
            //rectangle
            sb.Draw(Textures.hitbox, new Rectangle(x, y, Textwidth, Textheight + Topspacing), main);
            //horizontal lines
            Tools.DrawLine(sb, new Vector2(x, y), new Vector2(x + Textwidth, y), border);
            Tools.DrawLine(sb, new Vector2(x, y + Textheight + Topspacing), new Vector2(x + Textwidth, y + Textheight + Topspacing), border);

            //text
            Tools.DisplayAlignedText(sb, Color.White, spritefont, text, AlignType.MiddleCenter, new Rectangle(x, y, Textwidth, Textheight + Topspacing)); 
        }
    }
}
