using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace thegame
{
    class MultiplayerLogin
    {

        public bool mainmenu { get; private set; }

        private WebClient wb;
        private string infotext = "" ;
        private string cursor = "";

        //TEXT BOX
        private string create_email_string = "";
        private string create_password_string = "";
        private string create_password_string_hidden = "";
        

        private Cursor mouseCursor;

        /* CREATE ACCOUNT TEXT BOX */
        Rectangle create_email = new Rectangle(20, 130, 200, 40);
        Rectangle create_password = new Rectangle(20, 220, 200, 40);
        Rectangle create_account_button = new Rectangle(20, 280, 200, 40);
        Rectangle back_main_menu = new Rectangle(570, 20, 400, 40);

        private bool AnimatedCursor;
        private float AnimatedCursorTime = 0;

        private enum Cursor
        {
            none,
            create_email,
            create_pwd
        }

        public MultiplayerLogin()
        {
            mouseCursor = Cursor.none;
            AnimatedCursor = false;
            mainmenu = false;
        }

        public void CreateAccount()
        {
            wb = new WebClient();
            var data = new NameValueCollection();
            data["email"] = create_email_string;
            data["password"] = create_password_string;
            var response = wb.UploadValues("http://squickilling.com/json/login.php", "POST", data);
            infotext = System.Text.Encoding.UTF8.GetString(response);
            // List<string> myObj = new List<string>(); DO NOT DELETED THIS!!!
            //Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text); DO NOT DELETED THIS!!!
            create_password_string = create_email_string = "";
        }

        public void Update(GameTime gametime)
        {
            if (Inputs.isLMBClick())
            {
                Point themouse = Inputs.getMousePoint();
                if (back_main_menu.Contains(themouse))
                    mainmenu = true;
                else if (create_account_button.Contains(themouse))
                {
                    CreateAccount();
                }
                else if (create_email.Contains(themouse))
                {
                    mouseCursor = Cursor.create_email;
                    AnimatedCursor = true;
                    AnimatedCursorTime = 400;
                }
                else if (create_password.Contains(themouse))
                {
                    mouseCursor = Cursor.create_pwd;
                    AnimatedCursor = true;
                    AnimatedCursorTime = 400;
                }
                else
                {
                    mouseCursor = Cursor.none;
                    AnimatedCursor = false;
                    cursor = "";
                }
            }
            if (mouseCursor != Cursor.none)
            {
                switch (mouseCursor)
                {
                    case Cursor.create_email:
                        WriteText(ref create_email_string);
                        break;
                    case Cursor.create_pwd:
                        WriteText(ref create_password_string);
                        break;
                    default:
                        break;
                }

            }

            CursorAnimaition(gametime);
        }

        private void WriteText(ref string text)
        {
            if (text.Length < 35)
            {
                string ajout = Inputs.getInputKey();
                text += ajout;
                if(ajout != "")
                    AnimatedCursorTime = 0;
            }
            if (Inputs.isKeyRelease(Microsoft.Xna.Framework.Input.Keys.Back) && text.Length >= 1)
                text = text.Substring(0, text.Length - 1);
        }

        private void CursorAnimaition(GameTime gametime)
        {
            if (AnimatedCursor)
            {
                AnimatedCursorTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedCursorTime < 400)
                     cursor = "";
                else if ( AnimatedCursorTime < 800)
                    cursor = "|" ;
                else
                    AnimatedCursorTime = 0;
            }

        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            sb.DrawString(Textures.font_texture, "Multiplayer : DESIGN COMIGN SOON ", new Vector2(20, 20), Color.Black);
            sb.DrawString(Textures.font_texture, "Back main menu", new Vector2(back_main_menu.X, back_main_menu.Y), Color.Black);

            //create email input
            sb.DrawString(Textures.font_texture, "Email", new Vector2(88, 96), Color.Black);
            sb.Draw(Textures.hitbox, create_email, Color.White);
            Tuple<int, int> bound = CalculateBound(create_email_string.Length);
            sb.DrawString(Textures.font_texture, create_email_string.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.create_email) ?  cursor : "" ), new Vector2(create_email.X + 10, create_email.Y + 5), Color.Black);

            sb.DrawString(Textures.font_texture, "Password", new Vector2(64, 183), Color.Black);
            sb.Draw(Textures.hitbox, create_password, Color.White);
            
            for (int i = 0; i < create_password_string.Length; i++)
                create_password_string_hidden += "*";
            bound = CalculateBound(create_password_string_hidden.Length);
            sb.DrawString(Textures.font_texture, create_password_string_hidden.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.create_pwd) ? cursor : ""), new Vector2(create_password.X + 10, create_password.Y + 5), Color.Black);
            create_password_string_hidden = "";

            //button
            sb.Draw(Textures.hitbox, create_account_button, Color.Red);
            sb.DrawString(Textures.font_texture, "Create account", new Vector2(create_account_button.X + 10, create_account_button.Y + 5), Color.White);

            sb.DrawString(Textures.font_texture, infotext, new Vector2(50, 450), Color.Black);
            sb.End();
        }

        private Tuple<int, int> CalculateBound(int lenght)
        {
            if (lenght > 16)
                return new Tuple<int, int>(lenght - 16, 16);
            else
                return new Tuple<int, int>(0, lenght);


        }

    }
}
