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
        private string login_email_string = "";
        private string login_password_string = "";
        private string login_password_string_hidden = "";
        private int rightCircleX = 580;
        private int rightCircleY = 244;
        private int leftCircleX = 240;
        private int leftCircleY = 244;
        private int radius = 140;
        

        private Cursor mouseCursor;

        private int XcreateForm = 484;
        private int XloginForm = 138;
        /* CREATE ACCOUNT TEXT BOX */
        Rectangle create_email, create_password, create_account_button, login_email, login_password, login_account_button;
        Rectangle back_main_menu = new Rectangle(480, 20, 400, 40);

        private bool AnimatedCursor;
        private float AnimatedCursorTime = 0;
        private float AnimatedColor_RightCircle = 0;
        private float AnimatedColor_LeftCircle = 0;
        private float AnimatedTransparency = 0;

        int xColorRightCircle = 73;
        int xColorLeftCircle = 73;

        private enum Cursor
        {
            none,
            create_email,
            create_pwd,
            login_email,
            login_pwd
        }

        private bool displayRightCircle = true;
        private bool displayLeftCircle = true;
        private bool displayLoginForm = false;
        private bool displayCreateForm = false;

        private float transparency = 0;
        

        public MultiplayerLogin()
        {
            mouseCursor = Cursor.none;
            AnimatedCursor = mainmenu = false;

            create_email = new Rectangle(XcreateForm, 160, 200, 40);
            create_password = new Rectangle(XcreateForm, 250, 200, 40);
            create_account_button = new Rectangle(XcreateForm, 310, 200, 40);
            login_email = new Rectangle(XloginForm, 160, 200, 40);
            login_password = new Rectangle(XloginForm, 250, 200, 40);
            login_account_button = new Rectangle(XloginForm, 310, 200, 40);
            
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
            Point themouse = Inputs.getMousePoint();
            if ((displayCreateForm || displayLoginForm) && transparency < 1)
            {
                AnimatedTransparency +=(float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedTransparency > 20)
                {
                    transparency += 0.03f;
                    AnimatedTransparency = 0;
                }
            }

            if (radius >= Math.Sqrt(Math.Pow(themouse.X - rightCircleX, 2) + Math.Pow(themouse.Y - rightCircleY, 2)))
            {
                if (Inputs.isLMBClick() && !displayCreateForm)
                {
                    displayCreateForm = true;
                    displayRightCircle = false;
                    displayLeftCircle = true;
                    displayLoginForm = false;
                    transparency = 0;
                }

                AnimatedColor_RightCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_RightCircle > 5)
                {
                    if (xColorRightCircle < 150)
                        xColorRightCircle++;
                }
                else
                    AnimatedColor_RightCircle = 0;
            }
            else
            {
                AnimatedColor_RightCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_RightCircle > 50)
                {
                    if (xColorRightCircle > 75)
                        xColorRightCircle--;
                }
                else
                    AnimatedColor_RightCircle = 0;
            }

            if (radius >= Math.Sqrt(Math.Pow(themouse.X - leftCircleX, 2) + Math.Pow(themouse.Y - leftCircleY, 2)))
            {
                if (Inputs.isLMBClick() && !displayLoginForm)
                {
                    displayLoginForm = true;
                    displayLeftCircle = false;
                    displayRightCircle = true;
                    displayCreateForm = false;
                    transparency = 0;
                }
                AnimatedColor_LeftCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_LeftCircle > 5)
                {
                    if (xColorLeftCircle < 150)
                        xColorLeftCircle++;
                }
                else
                    AnimatedColor_LeftCircle = 0;
            }
            else
            {
                AnimatedColor_LeftCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_LeftCircle > 50)
                {
                    if (xColorLeftCircle > 75)
                        xColorLeftCircle--;
                }
                else
                    AnimatedColor_LeftCircle = 0;
            }




            if (Inputs.isLMBClick() && (displayCreateForm || displayLoginForm))
            {
                
                
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
            if(displayCreateForm || displayLoginForm)
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

            if (displayCreateForm)
            {
                //create email input
                sb.DrawString(Textures.font_texture, "Email", new Vector2(XcreateForm + 70, 126), Color.Black * transparency);
                sb.Draw(Textures.hitbox, create_email, Color.White * transparency);
                Tuple<int, int> bound = CalculateBound(create_email_string.Length);
                sb.DrawString(Textures.font_texture, create_email_string.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.create_email) ? cursor : ""), new Vector2(create_email.X + 10, create_email.Y + 5), Color.Black * transparency);

                sb.DrawString(Textures.font_texture, "Password", new Vector2(XcreateForm + 50, 213), Color.Black * transparency);
                sb.Draw(Textures.hitbox, create_password, Color.White * transparency);

                for (int i = 0; i < create_password_string.Length; i++)
                    create_password_string_hidden += "*";
                bound = CalculateBound(create_password_string_hidden.Length);
                sb.DrawString(Textures.font_texture, create_password_string_hidden.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.create_pwd) ? cursor : ""), new Vector2(create_password.X + 10, create_password.Y + 5), Color.Black * transparency);
                create_password_string_hidden = "";

                //button
                sb.Draw(Textures.hitbox, create_account_button, Color.Red * transparency);
                sb.DrawString(Textures.font_texture, "Create account", new Vector2(create_account_button.X + 22, create_account_button.Y + 7), Color.White * transparency);
            }
            if (displayLoginForm)
            {
                //create email input
                sb.DrawString(Textures.font_texture, "Email", new Vector2(XloginForm + 70, 126), Color.Black * transparency);
                sb.Draw(Textures.hitbox, login_email, Color.White * transparency);
                Tuple<int, int> bound = CalculateBound(login_email_string.Length);
                sb.DrawString(Textures.font_texture, login_email_string.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.login_email) ? cursor : ""), new Vector2(login_email.X + 10, login_email.Y + 5), Color.Black * transparency);

                sb.DrawString(Textures.font_texture, "Password", new Vector2(XloginForm + 50, 213), Color.Black * transparency);
                sb.Draw(Textures.hitbox, login_password, Color.White * transparency);

                for (int i = 0; i < login_password_string.Length; i++)
                    login_password_string_hidden += "*";
                bound = CalculateBound(login_password_string_hidden.Length);
                sb.DrawString(Textures.font_texture, login_password_string_hidden.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.login_pwd) ? cursor : ""), new Vector2(login_password.X + 10, login_password.Y + 5), Color.Black * transparency);
                login_password_string_hidden = "";

                //button
                sb.Draw(Textures.hitbox, login_account_button, Color.Red * transparency);
                sb.DrawString(Textures.font_texture, "Login", new Vector2(login_account_button.X + 75, login_account_button.Y + 7), Color.White * transparency);
            }
            sb.DrawString(Textures.font_texture, infotext, new Vector2(50, 450), Color.Black);

            //public static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, int sides, Color color, float thickness = 1f
            if (displayRightCircle)
            {
                Tools.DrawCircle(sb, rightCircleX, rightCircleY, radius, 50, new Color(227, xColorRightCircle, 73), 150);
                sb.DrawString(Textures.font_texture, "Create an account", new Vector2(484, 228), Color.White);
            }
            if (displayLeftCircle)
            {
                Tools.DrawCircle(sb, leftCircleX, leftCircleY, radius, 50, new Color(227, xColorLeftCircle, 73), 150);
                sb.DrawString(Textures.font_texture, "Login", new Vector2(210, 228), Color.White);
            }
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
