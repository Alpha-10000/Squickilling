using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace thegame
{

    class Dashboard
    {
        private Button back_main_menu;
        public bool mainmenu = false;

        //for friends lists request
        private bool finish = true;
        private float TimeIntervalRequest = 0;
        private float TimeInterval = 10000;
        private List<Dictionary<string, string>> myfriendslist;
        private string friends_error = "";

        private bool finish2 = true;
        private float TimeIntervalRequest2 = 0;
        private float TimeInterval2 = 10000;
        private List<Dictionary<string, string>> myrequest;

        private List<string> Invitationsid = new List<string>();
        private List<Button> InvitationsButtonAccept = new List<Button>();
        private List<Button> InvitationsButtonIgnore = new List<Button>();

        public Button searchFriendButton;

        /* Coordonnates right and left circles */
        private int xCircle = 144;
        private int yTopCircle = 177;
        private int yBottomCircle = 392;
        private int radius = 100;

        private int ColorTop = 70;
        private int ColorBottom = 70;
        private float AnimatedColor_TopCircle = 0;
        private float AnimatedColor_BottomCircle = 0;

        /* Coordonates friends */
        private Rectangle titlefriends = new Rectangle(285, 77, 465, 50);
        private Rectangle contentfriends = new Rectangle(285, 123, 465, 150);
        private Rectangle titleinvitation = new Rectangle(285, 300, 465, 50);
        private Rectangle contentinvitation = new Rectangle(285, 346, 465, 150);

        public bool Create_game { get; private set; }
        public bool Join_game { get; private set; }

        public Dashboard()
        {
            TimeIntervalRequest = TimeInterval;
            TimeIntervalRequest2 = TimeInterval2;
            searchFriendButton = new Button(Language.Text_Game["_btnSearchFri"], 551, 83, Textures.font_texture, new Color(129, 130, 134), Color.White, new Color(14, 15, 15));
            back_main_menu = new Button(Language.Text_Game["_btnBackM"], 580, 10, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
        }

        public void Update(GameTime gametime)
        {
            back_main_menu.Update();
            if (back_main_menu.Clicked)
                mainmenu = true;
            Point themouse = Inputs.getMousePoint();

            if(Inputs.isKeyRelease(Microsoft.Xna.Framework.Input.Keys.Up))
                Create_game = true;
            if(Inputs.isKeyRelease(Microsoft.Xna.Framework.Input.Keys.Down))
                Join_game = true;

            if (radius >= Math.Sqrt(Math.Pow(themouse.X -xCircle, 2) + Math.Pow(themouse.Y - yTopCircle, 2)))
            {
                if (Inputs.isLMBClick())
                    Create_game = true;

                AnimatedColor_TopCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_TopCircle > 5)
                {
                    if (ColorTop < 150)
                        ColorTop++;
                }
                else
                    AnimatedColor_TopCircle = 0;
            }
            else
            {
                AnimatedColor_TopCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_TopCircle > 50)
                {
                    if (ColorTop > 75)
                        ColorTop--;
                }
                else
                    AnimatedColor_TopCircle = 0;
            }

            if (radius >= Math.Sqrt(Math.Pow(themouse.X - xCircle, 2) + Math.Pow(themouse.Y - yBottomCircle, 2)))
            {

                if (Inputs.isLMBClick())
                    Join_game = true;

                AnimatedColor_BottomCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_BottomCircle > 5)
                {
                    if (ColorBottom < 150)
                        ColorBottom++;
                }
                else
                    AnimatedColor_BottomCircle = 0;
            }
            else
            {
                AnimatedColor_BottomCircle += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (AnimatedColor_BottomCircle > 50)
                {
                    if (ColorBottom > 75)
                        ColorBottom--;
                }
                else
                    AnimatedColor_BottomCircle = 0;
            }

            if (myrequest != null)
            {
                for (int i = 0; i < myrequest.Count; i++)
                {
                    InvitationsButtonAccept[i].Update();
                    
                    InvitationsButtonIgnore[i].Update();
                    if (InvitationsButtonAccept[i].Clicked)
                        ButtonClickAcceptOrIgnore("1", Invitationsid[i]);
                    else if (InvitationsButtonIgnore[i].Clicked)
                        ButtonClickAcceptOrIgnore("0", Invitationsid[i]);

                }
            }

            GetFriends(gametime);
            GetInvitations(gametime);
            searchFriendButton.Update();

        }

        private void GetFriends(GameTime gametime)
        {
            TimeIntervalRequest += gametime.ElapsedGameTime.Milliseconds;

            if (finish && TimeIntervalRequest >= TimeInterval)
            {
                try
                {
                    finish = false;
                    TimeIntervalRequest = 0;
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/friends_lists.php"), "POST", data);
                }
                catch
                {
                    //TODO
                }
            }
        }

        void client_UploadFileCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null)
                {
                    finish = true;
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);


                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    friends_error = values["error"].ToString();
                    List<Dictionary<string, string>> ValueList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(values["thearray"].ToString());
                    myfriendslist = ValueList;
                }
            }
            catch
            {

            }
        }

        private void GetInvitations(GameTime gametime)
        {
            TimeIntervalRequest2 += gametime.ElapsedGameTime.Milliseconds;

            if (finish2 && TimeIntervalRequest2 >= TimeInterval2)
            {
                try
                {
                    finish = false;
                    TimeIntervalRequest2 = 0;
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted2);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/friends_request.php"), "POST", data);
                }
                catch
                {
                    //TODO
                }
            }
        }

        void client_UploadFileCompleted2(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    Invitationsid = new List<string>();
                    InvitationsButtonAccept = new List<Button>();
                    InvitationsButtonIgnore = new List<Button>();
                    finish2 = true;
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    myrequest = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(values["thearray"].ToString());

                    for (int i = 0; i < myrequest.Count; i++)
                    {
                        InvitationsButtonAccept.Add(new Button(Language.Text_Game["_btnAccept"], contentinvitation.X + 150, contentinvitation.Y + i * 40, Textures.font_texture, new Color(129, 130, 134), Color.White, new Color(14, 15, 15)));
                        InvitationsButtonIgnore.Add(new Button(Language.Text_Game["_btnIgnore"], contentinvitation.X + 300, contentinvitation.Y + i * 40, Textures.font_texture, new Color(129, 130, 134), Color.White, new Color(14, 15, 15)));
                        Invitationsid.Add(myrequest[i]["id"]);

                    }

                }
            }
            catch
            {

            }
        }

        private void ButtonClickAcceptOrIgnore(string accepted, string id)
        {
                try
                {
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted3);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    data["otherid"] = id;
                    data["accept"] = accepted;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/accept.php"), "POST", data);
                }
                catch
                {
                    //TODO
                }
        }

        void client_UploadFileCompleted3(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    finish2 = true;
                    TimeIntervalRequest2 = TimeInterval2;
                }
            }
            catch
            {

            }
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();

            if (!Game1.graphics.IsFullScreen)
                sb.Draw(Textures.menu_main_page, new Vector2(0, 0), Color.White);
            else
                sb.Draw(Textures.menu_main_page, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth + 40, Game1.graphics.PreferredBackBufferHeight + 5), Color.White);

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "WELCOME " + Session.session_name, AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            /* Circles */
            Tools.DrawCircle(sb, xCircle, yTopCircle, radius, 50, new Color(227, ColorTop, 73), 140);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Create a game", AlignType.MiddleCenter, new Rectangle(xCircle - radius, yTopCircle - radius, radius * 2, radius * 2));

            Tools.DrawCircle(sb, xCircle, yBottomCircle, radius, 50, new Color(227, ColorBottom, 73), 140);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Join a game", AlignType.MiddleCenter, new Rectangle(xCircle - radius, yBottomCircle - radius, radius * 2, radius * 2));

            /* MY FRIENDS */
            sb.Draw(Textures.hitbox, titlefriends, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White, titlefriends, 2);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Online friends", AlignType.MiddleCenter, new Rectangle(titlefriends.X, titlefriends.Y, titlefriends.Width / 2, titlefriends.Height));
            sb.Draw(Textures.hitbox, contentfriends, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White, contentfriends, 2);

            if (myfriendslist != null && myfriendslist.Count > 0)
                for (int i = 0; i < myfriendslist.Count; i++)
                    Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, myfriendslist[i]["name"] + " : " + myfriendslist[i]["status"], AlignType.MiddleCenter, new Rectangle(contentfriends.X, contentfriends.Y + i * 40, contentfriends.Width, 40));
            else
                Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "No friends are online", AlignType.MiddleCenter, new Rectangle(contentfriends.X, contentfriends.Y + 40, contentfriends.Width, 40));
            /* INVITATIONS */
            sb.Draw(Textures.hitbox, titleinvitation, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White, titleinvitation, 2);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Friends request", AlignType.MiddleCenter, new Rectangle(titleinvitation.X, titleinvitation.Y, titleinvitation.Width / 2, titleinvitation.Height));
            sb.Draw(Textures.hitbox, contentinvitation, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White, contentinvitation, 2);
           
                

            if (myrequest != null && myrequest.Count > 0)
            {
                for (int i = 0; i < myrequest.Count; i++)
                {
                    Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, myrequest[i]["name"], AlignType.MiddleCenter, new Rectangle(contentinvitation.X, contentinvitation.Y + i * 40, contentinvitation.Width / 2, 40));
                    InvitationsButtonAccept[i].Display(sb);
                    InvitationsButtonIgnore[i].Display(sb);
                }
            }
            else
                Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "No friends request", AlignType.MiddleCenter, new Rectangle(contentinvitation.X, contentinvitation.Y + 40, contentinvitation.Width, 40));

            searchFriendButton.Display(sb);
            back_main_menu.Display(sb);
            sb.End();

            
        }
    }
}
