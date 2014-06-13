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
        private string[] infotext = new string[] { ""};
        private string infotext2 = "";
        private string cursor = "";

        public string session_password = "";
        public string session_email = "";
        public string session_id = "";
        public string session_name = "";

        public bool session_isset = false;

        //TEXT BOX
        private Textbox create_name_string;
        private Textbox create_email_string;
        private Textbox create_password_string;
        private Textbox login_email_string;
        private Textbox login_password_string;
        private int rightCircleX = 580;
        private int rightCircleY = 244;
        private int leftCircleX = 240;
        private int leftCircleY = 244;
        private int radius = 140;
        
        private int XcreateForm = 484;
        private int XloginForm = 138;
        /* CREATE ACCOUNT TEXT BOX */
        Rectangle create_account_button, login_account_button, login_forgot_password, create_text_name, create_text_email
            , create_text_password, login_text_email, login_text_password;
        Rectangle back_main_menu = new Rectangle(480, 20, 400, 40);

    
        private float AnimatedColor_RightCircle = 0;
        private float AnimatedColor_LeftCircle = 0;
        private float AnimatedTransparency = 0;
        private Popup popup;

        private bool FirstTime = true;

        int xColorRightCircle = 73;
        int xColorLeftCircle = 73;

       

        private bool displayRightCircle = true;
        private bool displayLeftCircle = true;
        private bool displayLoginForm = false;
        private bool displayCreateForm = false;

        private float transparency = 0;
        

        public MultiplayerLogin()
        {
            wb = new WebClient();

            /* All rectangle for alignements and display to check collision with mouse */
            //Create form
            create_text_name = new Rectangle(XcreateForm, 110, 200, 50);//+50 for text, +40 otherwise
            create_name_string = new Textbox(XcreateForm, 160, 200, 40);

            create_text_email = new Rectangle(XcreateForm, 200, 200, 50);
            create_email_string = new Textbox(XcreateForm, 250, 200, 40);

            create_text_password = new Rectangle(XcreateForm, 290, 200, 50);
            create_password_string = new Textbox(XcreateForm, 340, 200, 40);
            create_account_button = new Rectangle(XcreateForm, 400, 200, 40);

            //Login form
            login_text_email = new Rectangle(XloginForm, 110, 200, 50);
            login_email_string = new Textbox(XloginForm, 160, 200, 40);

            login_text_password = new Rectangle(XloginForm, 200, 200, 50);
            login_password_string = new Textbox(XloginForm, 250, 200, 40);
            

            login_account_button = new Rectangle(XloginForm, 310, 200, 40);
            login_forgot_password = new Rectangle(XloginForm, 350, 200, 50);
        }

        public void CreateAccount()
        {
            try
            {
                var data = new NameValueCollection();
                data["email"] = create_email_string.text;
                data["password"] = create_password_string.text;
                data["name"] = create_name_string.text;
                var response = wb.UploadValues("http://squickilling.com/json/create_account.php", "POST", data);
                infotext[0] = System.Text.Encoding.UTF8.GetString(response);
                // List<string> myObj = new List<string>(); DO NOT DELETED THIS!!!
                //Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text); DO NOT DELETED THIS!!!
                create_password_string.text = create_email_string.text = create_name_string.text = "";
            }
            catch
            {
                infotext[0] = "Sorry. We are unable to reach the database. Try again later.";
            }
        }

      

        public void Update(GameTime gametime)
        {
            if (popup == null && infotext[0] != "")
                popup = new Popup("ok", "", (infotext[0] == "Your account has been created." ? "Congratulations" : "Something wrong"), infotext, Textures.font_texture, 500);
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
                
                if ((Inputs.isLMBClick() || keyboardright) && !displayCreateForm && popup == null)
                {
                    displayCreateForm = true;
                    displayRightCircle = false;
                    displayLeftCircle = true;
                    displayLoginForm = false;
                    transparency = 0;
                    create_name_string.isSelected = true;
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
                
                if ((Inputs.isLMBClick() || keyboardleft) && !displayLoginForm && popup == null)
                {
                    displayLoginForm = true;
                    displayLeftCircle = false;
                    displayRightCircle = true;
                    displayCreateForm = false;
                    transparency = 0;
                    login_email_string.isSelected = true;
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


            

            if (displayLoginForm)
            {
                if (Inputs.isLMBClick() && login_account_button.Contains(Inputs.getMousePoint()) || Inputs.isKeyRelease(Keys.Enter))
                    Login();

                if (login_email_string.next)
                    login_password_string.isSelected = true;


            }
            else if (displayCreateForm)
            {
                if (Inputs.isLMBClick() && create_account_button.Contains(Inputs.getMousePoint()) || Inputs.isKeyRelease(Keys.Enter))
                    CreateAccount();

                if (create_name_string.next)
                    create_email_string.isSelected = true;
                else if (create_email_string.next)
                    create_password_string.isSelected = true;
            }

            create_name_string.Update(gametime);
            create_email_string.Update(gametime);
            create_password_string.Update(gametime);
            login_email_string.Update(gametime);
            login_password_string.Update(gametime);
   

            if (popup != null)
            {
                popup.Update();
                if (popup.action1bool)
                {
                    infotext = new string[] { "" };
                    popup = null;
                }
            }
        }

        private void Login()
        {
            try
            {
                var data = new NameValueCollection();
                data["email"] = login_email_string.text;
                data["password"] = login_password_string.text;
                var response = wb.UploadValues("http://squickilling.com/json/login.php", "POST", data);
                infotext2 = System.Text.Encoding.UTF8.GetString(response);
                List<string> myObj = new List<string>();
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(infotext2);
                
                session_email = values["email"];
                session_password = values["password"];
                session_id = values["id"];
                session_name = values["name"];
                infotext2 = values["error"];
                infotext[0] = infotext2;
                if(infotext2 == "")
                    session_isset = true;
                login_password_string.text = login_email_string.text = "";
           }
           catch
           {
                infotext[0] = "Sorry. We are unable to reach the database. Try again later.";
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
                create_name_string.Display(sb, false, newColor);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Email", AlignType.MiddleCenter, create_text_email);
                create_email_string.Display(sb, false, newColor);

                 Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Password", AlignType.MiddleCenter, create_text_password);
                 create_password_string.Display(sb, true, newColor);

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
                login_email_string.Display(sb, false, newColor);

                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Password", AlignType.MiddleCenter, login_text_password);

                login_password_string.Display(sb, true, newColor);

                //button
                sb.Draw(Textures.hitbox, login_account_button, Color.Red * newColor);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Login", AlignType.MiddleCenter, login_account_button);
                Tools.DisplayAlignedText(sb, Color.White * newColor, Textures.font_texture, "Forgot password ?", AlignType.MiddleCenter, login_forgot_password);
            

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

                if (popup != null)
                    popup.Display(sb);
            
            sb.End();
        }

     

    }
}
