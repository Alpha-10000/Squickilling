using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace thegame
{
    class Create_game
    {
        private Button go_back;
        public bool goback = false;

        public bool IsOkayToGoToTheGame = false;
        public bool PlayerToGame = false;
        /* Content create */
        private Rectangle contentcreate = new Rectangle(175, 70, 450, 220);
        private Rectangle BottomBox = new Rectangle(50, 315, 700, 185);
        private bool finish = false;

        private string error;
        private List<Dictionary<string, string>> getFriends;
        private List<Button> inviteButtonList = new List<Button>();
        private List<string> ListOfId = new List<string>();
        private Popup popup;
        private List<bool> alreadysent = new List<bool>();

        private List<Button> TheInvitedCancelButton = new List<Button>();
        private List<Dictionary<string, string>> getInvitations;
        private Textbox essai_textbox;

        private float RefreshInvitedBox = 0;
        private float RefreshBoxIntervall = 9000;

        public Create_game()
        {
            essai_textbox = new Textbox(400, 75, 200, 40);
            go_back = new Button("Go back", 620, 10, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
            InvitedBox();
        }

        private void RefreshBox(GameTime gametime)//refresh the invitations to know if accepted or not
        {
            RefreshInvitedBox += gametime.ElapsedGameTime.Milliseconds;
            if (RefreshInvitedBox >= RefreshBoxIntervall)
            {
                InvitedBox();
                RefreshInvitedBox = 0;
            }
        }

        public void Update(GameTime gametime)
        {

            RefreshBox(gametime);

            essai_textbox.Update(gametime);
            if (essai_textbox.text != "" && essai_textbox.HasJustType)
                finish = true;

            go_back.Update();
            if (go_back.Clicked)
                goback = true;
        
            if (popup != null)
            {
                popup.Update(gametime);
                if (popup.action1bool && !IsOkayToGoToTheGame)
                    popup = null;
                else if (popup.action1bool && IsOkayToGoToTheGame)
                    PlayerToGame = true;
            }
            Point themouse = Inputs.getMousePoint();
            if (50 >= Math.Sqrt(Math.Pow(themouse.X - 653, 2) + Math.Pow(themouse.Y - 405, 2)) && Inputs.isLMBClick())
                CreateGame();

            GetFriends();
            if (getFriends != null)
                for (int i = 0; i < getFriends.Count; i++)
                {
                    inviteButtonList[i].Update();
                    if (inviteButtonList[i].Clicked)
                        if (!alreadysent[i])
                            GetInvitations(ListOfId[i]);
                        else
                            CancelInvitation(getFriends[i]["id"]);
                }

            if (getInvitations != null)
                for (int i = 0; i < getInvitations.Count; i++)
                    if (TheInvitedCancelButton[i] != null)
                    {
                        TheInvitedCancelButton[i].Update();
                        if (TheInvitedCancelButton[i].Clicked)
                            CancelInvitation(getInvitations[i]["id"]);
                    }
        }

        private void GetFriends()
        {

            if (finish)
            {
                try
                {
                    finish = false;
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    data["name"] = essai_textbox.text;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/invite_friends.php"), "POST", data);
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
                    inviteButtonList = new List<Button>();
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);


                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    error = values["error"].ToString();
                    List<Dictionary<string, string>> ValueList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(values["thearray"].ToString());
                    getFriends = ValueList;

                    alreadysent = new List<bool>();
                    inviteButtonList = new List<Button>();
                    ListOfId = new List<string>();
                    for (int i = 0; i < ValueList.Count; i++)
                    {
                        if (ValueList[i]["invitation"] == "sent")
                        {
                            inviteButtonList.Add(new Button("Cancel invitation", contentcreate.X + 290, contentcreate.Y + 56 + i * 50, Textures.fonthelp_texture, new Color(129, 130, 134), Color.White, new Color(14, 15, 15)));
                            alreadysent.Add(true);
                        }
                        else
                        {
                            inviteButtonList.Add(new Button("Invite", contentcreate.X + 290, contentcreate.Y + 56 + i * 50, Textures.fonthelp_texture, new Color(129, 130, 134), Color.White, new Color(14, 15, 15)));
                            alreadysent.Add(false);
                        }
                        ListOfId.Add(ValueList[i]["id"]);
                    }
                }
            }
            catch
            {

            }
        }

        private void GetInvitations(string otherid)
        {
                try
                {
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted2);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    data["otherid"] = otherid;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/push_invitation.php"), "POST", data);
                }
                catch
                {
                    //TODO
                }
        }

        void client_UploadFileCompleted2(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null)
                {
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    popup = new Popup("OK", "", "Information", new string[] { text }, Textures.font_texture, 450);
                    InvitedBox();
                }
            }
            catch
            {

            }
        }

        private void InvitedBox()
        {

                try
                {
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted3);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/invitations_box.php"), "POST", data);
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
                    TheInvitedCancelButton = new List<Button>();
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    error = values["error"].ToString();
                    List<Dictionary<string, string>> ValueList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(values["thearray"].ToString());
                    getInvitations = ValueList;

                    for (int i = 0; i < ValueList.Count; i++)
                    {
                        if (ValueList[i]["joined"] == "0")
                            TheInvitedCancelButton.Add(new Button("Cancel invitation", BottomBox.X + 290, BottomBox.Y + 45 * i + 37, Textures.fonthelp_texture, new Color(129, 130, 134), Color.White, new Color(14, 15, 15)));
                        else
                            TheInvitedCancelButton.Add(null);
                    }
                }
            }
            catch
            {

            }
        }

        private void CancelInvitation(string otherid)
        {

            try
            {
                WebClient wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted4);
                data["pwd"] = Session.session_password;
                data["id"] = Session.session_id;
                data["otherid"] = otherid;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/cancel_invitation.php"), "POST", data);
            }
            catch
            {
                //TODO
            }

        }

        void client_UploadFileCompleted4(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null)
                {

                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    popup = new Popup("OK", "", "Information", new string[] { text }, Textures.font_texture, 450);

                    InvitedBox();
                    finish = true;
                    GetFriends();
                }
            }
            catch
            {

            }
        }

        private void CreateGame()
        {

            try
            {
                WebClient wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted5);
                data["pwd"] = Session.session_password;
                data["id"] = Session.session_id;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/create_game.php"), "POST", data);
            }
            catch
            {
                //TODO
            }

        }

        void client_UploadFileCompleted5(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null)
                {

                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    popup = new Popup("OK", "", "Information", new string[] { text }, Textures.font_texture, 450);
                    if (text == "The game has been created")
                        IsOkayToGoToTheGame = true;
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

            go_back.Display(sb);

            sb.Draw(Textures.hitbox, contentcreate, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White, contentcreate, 2);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Search friends : ", AlignType.MiddleCenter, new Rectangle(contentcreate.X, contentcreate.Y, contentcreate.Width / 2, 50));

            sb.Draw(Textures.hitbox, BottomBox, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White,  BottomBox, 2);

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Invited friends", AlignType.MiddleCenter, new Rectangle(BottomBox.X, BottomBox.Y, BottomBox.Width / 4, 50));

            if (getInvitations != null && getInvitations.Count > 0)
                for (int i = 0; i < getInvitations.Count; i++)
                {
                    Tools.DisplayAlignedText(sb, Color.White, Textures.fonthelp_texture, getInvitations[i]["name"] + " : " + getInvitations[i]["status"], AlignType.MiddleCenter, new Rectangle(BottomBox.X + 25, BottomBox.Y + i * 45 - 35, BottomBox.Width / 3, BottomBox.Height));
                    if(TheInvitedCancelButton[i] != null)
                        TheInvitedCancelButton[i].Display(sb);
                    else
                        Tools.DisplayAlignedText(sb, Color.White, Textures.fonthelp_texture, "Has joined", AlignType.MiddleCenter, new Rectangle(BottomBox.X + 290, BottomBox.Y + i * 45 - 35, BottomBox.Width / 3, BottomBox.Height));
                }
            else
                Tools.DisplayAlignedText(sb, Color.White, Textures.fonthelp_texture, "Nobody has been invited yet", AlignType.MiddleCenter, new Rectangle(BottomBox.X + 25, BottomBox.Y, BottomBox.Width / 3, BottomBox.Height));

            if (getFriends != null)
                for (int i = 0; i < getFriends.Count; i++)
                {
                    Tools.DisplayAlignedText(sb, Color.White, Textures.fonthelp_texture, getFriends[i]["name"] + " : " + getFriends[i]["status"], AlignType.MiddleCenter, new Rectangle(contentcreate.X + 100, contentcreate.Y + 50 + i * 50, 80, 50));
                    inviteButtonList[i].Display(sb);
                }

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Create game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            Tools.DrawCircle(sb, 653, 405, 50, 600, Color.White, 120);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.fonthelp_texture, "Launch Game", AlignType.MiddleCenter, new Rectangle(653 - 50, 405 - 50, 100, 100));

            //sb.DrawString(Textures.fonthelp_texture, "Make it public", new Vector2(450, 390), Color.White);
            //sb.Draw(Textures.hitbox, new Rectangle(420, 392, 20, 20), Color.White);

            if (popup != null)
                popup.Display(sb);

            essai_textbox.Display(sb, false);
            sb.End();
        }
    }
}
