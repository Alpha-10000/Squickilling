using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thegame
{
    class Textbox
    {
        public string KeyBoard;
        public bool isSelected = false;
        public Rectangle theBoxrectangle;
        public string text;
        public string cursor = " ";
        private float AnimatedCursorTime = 0;
        private int width;
        public bool next = false;

        public bool HasJustType = false;
        

        public Textbox(int x, int y, int width, int height)
        {
            this.theBoxrectangle = new Rectangle(x, y, width, height);
            text = "";
            this.width = width;

            // Detect KeyBoard Layout.
            string KeyBoardLayout = KeyboardLayout.getName();
            if (KeyBoardLayout == "00000407" || KeyBoardLayout == "00000807")
            {
                KeyBoard = "QWERTZ";
            }
            else if (KeyBoardLayout == "0000040C" || KeyBoardLayout == "0000080C")
            {
                KeyBoard = "AZERTY";
            }
            else if (KeyBoardLayout == "00010409")
            {
                KeyBoard = "Dvorak";
            }
            else
            {
               KeyBoard = "QWERTY";
            }
        }



        public void Update(GameTime gametime)
        {

            HasJustType = false;

            if (Inputs.isLMBClick() && theBoxrectangle.Contains(Inputs.getMousePoint()))
            {
                isSelected = true;
                next = false;
            }
            else if (Inputs.isLMBClick())//the mouse is elsewhere
            {
                isSelected = false;
                next = false;
                cursor = " ";
            }

            if (isSelected && Inputs.AnyKeyPressed())
            {
                HasJustType = true;
            }

            if (next)
            {
                isSelected = false;
                next = false;
                cursor = " ";
            }

            if (isSelected)
            {
                char truc = '\0';
                if (TryConvertKeyboardInput(out truc))
                    text += truc;
                if (Inputs.isKeyRelease(Keys.Back) && text.Length > 0)
                    text = text.Substring(0, text.Length - 1);
                if (!Inputs.AnyKeyPressed())
                    AnimateCursor(gametime);
                if (Inputs.isKeyRelease(Keys.Tab))
                {
                    next = true;
                    isSelected = false;
                    cursor = " ";
                }
            }
            
        }

        private void AnimateCursor(GameTime gametime)
        {
                AnimatedCursorTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedCursorTime < 400)
                    cursor = " ";
                else if (AnimatedCursorTime < 800)
                    cursor = "|";
                else
                    AnimatedCursorTime = 0;
        }

        public void Reset()
        {
            text = "";
        }

        private Tuple<int, int> CalculateBound(Vector2 dim)
        {
            if (dim.X > width)
            {
                int textLegnth = text.Length;
                int compteur = 0;
                for (int i = textLegnth; i >= 0; i--)
                    if (Textures.font_texture.MeasureString(text.Substring(0, i)).X > width - 10)
                        compteur++;//nb char that are outside the textbox
                return new Tuple<int, int>(compteur, textLegnth - compteur);
            }
            else
            {
                return new Tuple<int, int>(0, text.Length);
            }


        }


        public void Display(SpriteBatch sb, bool hidden)
        {
            sb.Draw(Textures.hitbox, theBoxrectangle, Color.White);
            Tuple<int, int> bounds = CalculateBound(Textures.font_texture.MeasureString(text));
            string textD;
            if (hidden)
            {
                textD = "";
                for (int i = bounds.Item1; i < bounds.Item2; i++)
                    textD += "*";
            }
            else
                textD = text.Substring(bounds.Item1, bounds.Item2);
            sb.DrawString(Textures.font_texture, textD + cursor, new Vector2(theBoxrectangle.X, theBoxrectangle.Y + 6 ), Color.Black);
        }

        public void Display(SpriteBatch sb, bool hidden, float transparency)
        {
            sb.Draw(Textures.hitbox, theBoxrectangle, Color.White * transparency);
            Tuple<int, int> bounds = CalculateBound(Textures.font_texture.MeasureString(text));
            string textD;
            if (hidden)
            {
                textD = "";
                for (int i = 0; i < text.Length; i++)
                    textD += "*";
                textD = textD.Substring(bounds.Item1, bounds.Item2);
            }
            else
                textD = text.Substring(bounds.Item1, bounds.Item2);
            sb.DrawString(Textures.font_texture, textD + cursor, new Vector2(theBoxrectangle.X, theBoxrectangle.Y + 6), Color.Black  * transparency);
        }



        private bool TryConvertKeyboardInput(out char key)
        {
            bool shift = Inputs.isKeyDown(Keys.LeftShift) || Inputs.isKeyDown(Keys.RightShift);
            bool LeftAlt = Inputs.isKeyDown(Keys.LeftAlt);
            bool RighAlt = Inputs.isKeyDown(Keys.LeftControl);

            if (Inputs.AnyKeyPressed())
            {
                string KL = KeyboardLayout.getName();

                if (KeyBoard == "QWERTY")
                {
                    switch (Inputs.pressedKeys[0])
                    {
                        //Alphabet keys
                        case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } return true;
                        case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } return true;
                        case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } return true;
                        case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } return true;
                        case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } return true;
                        case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } return true;
                        case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } return true;
                        case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } return true;
                        case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } return true;
                        case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } return true;
                        case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } return true;
                        case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } return true;
                        case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } return true;
                        case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } return true;
                        case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } return true;
                        case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } return true;
                        case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } return true;
                        case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } return true;
                        case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } return true;
                        case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } return true;
                        case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } return true;
                        case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } return true;
                        case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } return true;
                        case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } return true;
                        case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } return true;
                        case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } return true;

                        //Decimal keys
                        case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } return true;
                        case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } return true;
                        case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } return true;
                        case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } return true;
                        case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } return true;
                        case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } return true;
                        case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } return true;
                        case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } return true;
                        case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } return true;
                        case Keys.D9: if (shift) { key = '('; } else { key = '9'; } return true;

                        //Decimal numpad keys
                        case Keys.NumPad0: key = '0'; return true;
                        case Keys.NumPad1: key = '1'; return true;
                        case Keys.NumPad2: key = '2'; return true;
                        case Keys.NumPad3: key = '3'; return true;
                        case Keys.NumPad4: key = '4'; return true;
                        case Keys.NumPad5: key = '5'; return true;
                        case Keys.NumPad6: key = '6'; return true;
                        case Keys.NumPad7: key = '7'; return true;
                        case Keys.NumPad8: key = '8'; return true;
                        case Keys.NumPad9: key = '9'; return true;
                        case Keys.Subtract: key = '-'; return true;
                        case Keys.Add: key = '+'; return true;
                        case Keys.Multiply: key = '*'; return true;
                        case Keys.Divide: key = '/'; return true;
                        case Keys.Decimal: key = '.'; return true;

                        //Special keys
                        case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } return true;
                        case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } return true;
                        case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } return true;
                        case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } return true;
                        case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } return true;
                        case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } return true;
                        case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } return true;
                        case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } return true;
                        case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } return true;
                        case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } return true;
                        case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } return true;
                        case Keys.Space: key = ' '; return true;
                    }
                }
                else if (KeyBoard == "AZERTY")
                {
                    switch (Inputs.pressedKeys[0])
                    {
                        //Alphabet keys
                        case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } return true;
                        case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } return true;
                        case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } return true;
                        case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } return true;
                        case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } return true;
                        case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } return true;
                        case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } return true;
                        case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } return true;
                        case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } return true;
                        case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } return true;
                        case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } return true;
                        case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } return true;
                        case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } return true;
                        case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } return true;
                        case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } return true;
                        case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } return true;
                        case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } return true;
                        case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } return true;
                        case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } return true;
                        case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } return true;
                        case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } return true;
                        case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } return true;
                        case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } return true;
                        case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } return true;
                        case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } return true;
                        case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } return true;

                        //Decimal keys
                        case Keys.D1: if (shift) { key = '1'; } else  { key = '&'; } return true;
                        case Keys.D2: if (shift) { key = '2'; } else if (RighAlt) { key = '~'; } else { key = 'é'; } return true;
                        case Keys.D3: if (shift) { key = '3'; } else if (RighAlt) { key = '#'; } else { key = '"'; } return true;
                        case Keys.D4: if (shift) { key = '4'; } else if (RighAlt) { key = '{'; } else { key = '\''; } return true;
                        case Keys.D5: if (shift) { key = '5'; } else if (RighAlt) { key = '['; } else { key = '('; } return true;
                        case Keys.D6: if (shift) { key = '6'; } else if (RighAlt) { key = '|'; } else { key = '-'; } return true;
                        case Keys.D7: if (shift) { key = '7'; } else if (RighAlt) { key = '`'; } else { key = 'è'; } return true;
                        case Keys.D8: if (shift) { key = '8'; } else if (RighAlt) { key = '\\'; } else { key = '_'; } return true;
                        case Keys.D9: if (shift) { key = '9'; } else if (RighAlt) { key = '^'; } else { key = 'ç'; } return true;
                        case Keys.D0: if (shift) { key = '0'; } else if (RighAlt) { key = '@'; } else { key = 'à'; } return true;

                        //Decimal numpad keys
                        case Keys.NumPad0: key = '0'; return true;
                        case Keys.NumPad1: key = '1'; return true;
                        case Keys.NumPad2: key = '2'; return true;
                        case Keys.NumPad3: key = '3'; return true;
                        case Keys.NumPad4: key = '4'; return true;
                        case Keys.NumPad5: key = '5'; return true;
                        case Keys.NumPad6: key = '6'; return true;
                        case Keys.NumPad7: key = '7'; return true;
                        case Keys.NumPad8: key = '8'; return true;
                        case Keys.NumPad9: key = '9'; return true;
                        case Keys.Subtract: key = '-'; return true;
                        case Keys.Add: key = '+'; return true;
                        case Keys.Multiply: key = '*'; return true;
                        case Keys.Divide: key = '/'; return true;
                        case Keys.Decimal: key = '.'; return true;

                        //Special keys
                        case Keys.OemTilde:             if (shift) { key = '%'; } else { key = 'ù'; } return true;
                        case Keys.OemSemicolon:         if (shift) { key = '£'; } else { key = '$'; } return true;
                        case Keys.OemQuotes:            key = '²'; return true;
                        case Keys.OemQuestion:          if (shift) { key = '/'; } else { key = ':'; } return true;
                        case Keys.OemPlus:              if (shift) { key = '+'; } else if (RighAlt) { key = '}'; } else { key = '='; } return true;
                        case Keys.OemPipe:              if (shift) { key = 'µ'; } else { key = '*'; } return true;
                        case Keys.OemPeriod:            if (shift) { key = '.'; } else { key = ';'; } return true;
                        case Keys.OemOpenBrackets:      if (shift) { key = '°'; } else if (RighAlt) { key = ']'; } else { key = ')'; } return true;
                        case Keys.OemCloseBrackets:     if (shift) { key = '¨'; } else { key = '^'; } return true;
                        case Keys.OemMinus:             if (shift) { key = '_'; } else { key = '-'; } return true;
                        case Keys.OemComma:             if (shift) { key = '?'; } else { key = ','; } return true;
                        case Keys.Oem8:                 if (shift) { key = '§'; } else { key = '!'; } return true;
                        case Keys.OemBackslash:         if (shift) { key = '>'; } else { key = '<'; } return true;
                        case Keys.Space: key = ' '; return true;
                    }
                }
            }

            key = (char)0;
            return false;
        }
    }
}
