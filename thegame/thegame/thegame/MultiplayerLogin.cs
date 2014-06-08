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
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace thegame
{
    class MultiplayerLogin
    {

        public bool mainmenu { get; private set; }

        private WebClient wb;
        private string infotext = "" ;
        private string cursor = "";

        public string session_password = "";
        public string session_email = "";
        public string session_id = "";
        public string session_name = "";
        public bool session_isset = false;

        //TEXT BOX
        private string create_name_string = "";
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
        Rectangle create_name, create_email, create_password, create_account_button, login_email, login_password, login_account_button, login_forgot_password,
            login_text_email, login_text_password, create_text_name, create_text_email, create_text_password;
        Rectangle back_main_menu = new Rectangle(480, 20, 400, 40);

        private bool AnimatedCursor;
        private float AnimatedCursorTime = 0;
        private float AnimatedColor_RightCircle = 0;
        private float AnimatedColor_LeftCircle = 0;
        private float AnimatedTransparency = 0;

        private bool FirstTime = true;

        int xColorRightCircle = 73;
        int xColorLeftCircle = 73;

        private enum Cursor
        {
            none,
            create_name,
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
            wb = new WebClient();
            mouseCursor = Cursor.none;
            AnimatedCursor = mainmenu = false;

            /* All rectangle for alignements and display to check collision with mouse */
            //Create form
            create_text_name = new Rectangle(XcreateForm, 110, 200, 50);//+50 for text, +40 otherwise
            create_name = new Rectangle(XcreateForm, 160, 200, 40);

            create_text_email = new Rectangle(XcreateForm, 200, 200, 50);
            create_email = new Rectangle(XcreateForm, 250, 200, 40);

            create_text_password = new Rectangle(XcreateForm, 290, 200, 50);
            create_password = new Rectangle(XcreateForm, 340, 200, 40);
            create_account_button = new Rectangle(XcreateForm, 400, 200, 40);

            //Login form
            login_text_email = new Rectangle(XloginForm, 110, 200, 50);
            login_email = new Rectangle(XloginForm, 160, 200, 40);

            login_text_password = new Rectangle(XloginForm, 200, 200, 50);
            login_password = new Rectangle(XloginForm, 250, 200, 40);
            

            login_account_button = new Rectangle(XloginForm, 310, 200, 40);
            login_forgot_password = new Rectangle(XloginForm, 350, 200, 50);
        }

        public void CreateAccount()
        {
            try
            {
                var data = new NameValueCollection();
                data["email"] = create_email_string;
                data["password"] = create_password_string;
                data["name"] = create_name_string;
                var response = wb.UploadValues("http://squickilling.com/json/create_account.php", "POST", data);
                infotext = System.Text.Encoding.UTF8.GetString(response);
                // List<string> myObj = new List<string>(); DO NOT DELETED THIS!!!
                //Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text); DO NOT DELETED THIS!!!
                create_password_string = create_email_string = "";
            }
            catch
            {
                infotext = "Sorry. We are unable to reach the database. Try again later.";
            }
        }

      

        public void Update(GameTime gametime)
        {

            //fix a bug
            if (FirstTime && transparency >=1)
                FirstTime = false;

            Point themouse = Inputs.getMousePoint();

            if (back_main_menu.Contains(themouse) && Inputs.isLMBClick())
                mainmenu = true;

            if ((displayCreateForm || displayLoginForm) && transparency < 1)
            {
                AnimatedTransparency +=(float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedTransparency > 5)
                {
                    transparency += 0.02f;//un multiple de 100 pour pas que ça dépasse 1.
                    AnimatedTransparency = 0;
                }
            }

            bool keyboardleft = Inputs.isKeyRelease(Microsoft.Xna.Framework.Input.Keys.Left);
            bool keyboardright = Inputs.isKeyRelease(Microsoft.Xna.Framework.Input.Keys.Right);

            if (radius >= Math.Sqrt(Math.Pow(themouse.X - rightCircleX, 2) + Math.Pow(themouse.Y - rightCircleY, 2)) || keyboardright)
            {
                if ((Inputs.isLMBClick() || keyboardright) && !displayCreateForm)
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

            if (radius >= Math.Sqrt(Math.Pow(themouse.X - leftCircleX, 2) + Math.Pow(themouse.Y - leftCircleY, 2)) || keyboardleft)
            {
                if ((Inputs.isLMBClick() || keyboardleft)&& !displayLoginForm)
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




            if ((Inputs.isLMBClick() || !Inputs.UseMouse()) && (displayCreateForm || displayLoginForm))
            {
                /* Handle Keyboard input */
                if (!Inputs.UseMouse())
                {
                    if (mouseCursor == Cursor.none && displayCreateForm)
                    {
                        mouseCursor = Cursor.create_name;
                        AnimatedCursor = true;
                        AnimatedCursorTime = 400;
                    }
                    else if (mouseCursor == Cursor.none && displayLoginForm)
                    {
                        mouseCursor = Cursor.login_email;
                        AnimatedCursor = true;
                        AnimatedCursorTime = 400;
                    }
                    else
                    {
                        bool thekey = Inputs.isKeyRelease(Keys.Tab);
                        if (displayCreateForm)
                        {
                            if (mouseCursor == Cursor.create_name && thekey)
                            {
                                mouseCursor = Cursor.create_email;
                                AnimatedCursor = true;
                                AnimatedCursorTime = 400;
                            }
                            else if (mouseCursor == Cursor.create_email && thekey)
                            {
                                mouseCursor = Cursor.create_pwd;
                                AnimatedCursor = true;
                                AnimatedCursorTime = 400;
                            }
                            if (Inputs.isKeyRelease(Keys.Enter))
                                CreateAccount();
                        }
                        else
                        {
                            if (displayLoginForm)
                            {
                                if (mouseCursor == Cursor.login_email && thekey)
                                {
                                    mouseCursor = Cursor.login_pwd;
                                    AnimatedCursor = true;
                                    AnimatedCursorTime = 400;
                                }
                                if (Inputs.isKeyRelease(Keys.Enter))
                                    Login();
                            }

                        }
                    }
                }
                else
                {

                    
                    if (create_account_button.Contains(themouse) && transparency >= 1)
                        CreateAccount();
                    else if (login_account_button.Contains(themouse) && transparency >= 1)
                        Login();
                    else if (create_name.Contains(themouse))
                    {
                        mouseCursor = Cursor.create_name;
                        AnimatedCursor = true;
                        AnimatedCursorTime = 400;
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
                    else if (login_password.Contains(themouse))
                    {
                        mouseCursor = Cursor.login_pwd;
                        AnimatedCursor = true;
                        AnimatedCursorTime = 400;
                    }
                    else if (login_forgot_password.Contains(themouse))
                    {
                        Process.Start("http://www.squickilling.com/user/forgot-password.php");
                    }
                    else if (login_email.Contains(themouse))
                    {
                        mouseCursor = Cursor.login_email;
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
            }
            if (mouseCursor != Cursor.none)
            {
                switch (mouseCursor)
                {
                    case Cursor.create_email:
                        WriteText(ref create_email_string);
                        break;
                    case Cursor.create_name:
                        WriteText(ref create_name_string);
                        break;
                    case Cursor.create_pwd:
                        WriteText(ref create_password_string);
                        break;
                    case Cursor.login_pwd:
                        WriteText(ref login_password_string);
                        break;
                    case Cursor.login_email:
                        WriteText(ref login_email_string);
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

        private void Login()
        {
            try
            {
                var data = new NameValueCollection();
                data["email"] = login_email_string;
                data["password"] = login_password_string;
                var response = wb.UploadValues("http://squickilling.com/json/login.php", "POST", data);
                infotext = System.Text.Encoding.UTF8.GetString(response);
                List<string> myObj = new List<string>();
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(infotext);
                
                session_email = values["email"];
                session_password = values["password"];
                session_id = values["id"];
                session_name = values["name"];
                infotext = values["error"];
                if(infotext == "")
                    session_isset = true;
                login_password_string = login_email_string = "";
           }
           catch
           {
                infotext = "Sorry. We are unable to reach the database. Try again later.";
            }
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();

            if (!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0, 0), Color.White);
            else
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5), Color.White);

            sb.DrawString(Textures.font_texture, "MULTIPLAYER", new Vector2(20, 20), Color.White);
            sb.DrawString(Textures.font_texture, "Back main menu", new Vector2(back_main_menu.X, back_main_menu.Y), Color.White);

            /* CREATE ACCOUNT FORM */
                float newColor;
                if (FirstTime && displayRightCircle)
                    newColor = 0;
                else
                newColor = (displayLeftCircle) ? transparency : 1 - transparency;
                //create email input
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Name", AlignType.MiddleCenter, create_text_name);
                sb.Draw(Textures.hitbox, create_name, Color.White * newColor);
                Tuple<int, int> bound = CalculateBound(create_name_string.Length);
                sb.DrawString(Textures.font_texture, create_name_string.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.create_name) ? cursor : ""), new Vector2(create_name.X + 10, create_name.Y + 5), Color.Black * newColor);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Email", AlignType.MiddleCenter, create_text_email);
                sb.Draw(Textures.hitbox, create_email, Color.White * newColor);
                 bound = CalculateBound(create_email_string.Length);
                 sb.DrawString(Textures.font_texture, create_email_string.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.create_email) ? cursor : ""), new Vector2(create_email.X + 10, create_email.Y + 5), Color.Black * newColor);

                 Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Password", AlignType.MiddleCenter, create_text_password);
                 sb.Draw(Textures.hitbox, create_password, Color.White * newColor);

                for (int i = 0; i < create_password_string.Length; i++)
                    create_password_string_hidden += "*";
                bound = CalculateBound(create_password_string_hidden.Length);
                sb.DrawString(Textures.font_texture, create_password_string_hidden.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.create_pwd) ? cursor : ""), new Vector2(create_password.X + 10, create_password.Y + 5), Color.Black * newColor);
                create_password_string_hidden = "";

                //button
                sb.Draw(Textures.hitbox, create_account_button, Color.Red * newColor);
                sb.DrawString(Textures.font_texture, "Create account", new Vector2(create_account_button.X + 22, create_account_button.Y + 7), Color.White * newColor);
                
          /* LOGIN FORM */
                if (FirstTime && displayLeftCircle)
                    newColor = 0;
                else
                    newColor = (displayRightCircle) ? transparency : 1 - transparency;
                //create email input
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Email", AlignType.MiddleCenter, login_text_email);
                sb.Draw(Textures.hitbox, login_email, Color.White * newColor);
                bound = CalculateBound(login_email_string.Length);
                sb.DrawString(Textures.font_texture, login_email_string.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.login_email) ? cursor : ""), new Vector2(login_email.X + 10, login_email.Y + 5), Color.Black * newColor);

                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Password", AlignType.MiddleCenter, login_text_password);
                sb.Draw(Textures.hitbox, login_password, Color.White * newColor);

                for (int i = 0; i < login_password_string.Length; i++)
                    login_password_string_hidden += "*";
                bound = CalculateBound(login_password_string_hidden.Length);
                sb.DrawString(Textures.font_texture, login_password_string_hidden.Substring(bound.Item1, bound.Item2) + ((mouseCursor == Cursor.login_pwd) ? cursor : ""), new Vector2(login_password.X + 10, login_password.Y + 5), Color.Black * newColor);
                login_password_string_hidden = "";

                //button
                sb.Draw(Textures.hitbox, login_account_button, Color.Red * newColor);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Login", AlignType.MiddleCenter, login_account_button);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Forgot password ?", AlignType.MiddleCenter, login_forgot_password);
            
            sb.DrawString(Textures.font_texture, infotext, new Vector2(50, 450), Color.White);

            //public static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, int sides, Color color, float thickness = 1f

            /* RIGHT CIRCLE */
                if (FirstTime && displayRightCircle)
                    newColor = 1;
                else
                    newColor = (displayLeftCircle) ? 1 - transparency : transparency;
                Tools.DrawCircle(sb, rightCircleX, rightCircleY, radius, 50, new Color(227, xColorRightCircle, 73) * newColor, 140);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Create an account", AlignType.MiddleCenter, new Rectangle(rightCircleX - radius, rightCircleY - radius, radius*2, radius * 2));
            
            /* LEFT CIRCLE */
                if (FirstTime && displayLeftCircle)
                    newColor = 1;
                else
                    newColor = (displayRightCircle) ? 1 - transparency : transparency;
                Tools.DrawCircle(sb, leftCircleX, leftCircleY, radius, 50, new Color(227, xColorLeftCircle, 73) * newColor, 150);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Login", AlignType.MiddleCenter, new Rectangle(leftCircleX - radius, leftCircleY - radius, radius * 2, radius * 2));
            
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
