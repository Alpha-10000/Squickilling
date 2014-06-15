﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;

namespace thegame
{
    class Search_friends
    {
        private Rectangle go_back = new Rectangle(600, 20, 600, 40);
        public bool goback = false;

        /* Content create */
        private Rectangle titlecreate = new Rectangle(100, 77, 600, 50);
        private Rectangle contentcreate = new Rectangle(100, 123, 600, 350);
        private bool finish = false;

        private string error;
        private List<Dictionary<string, string>> getFriends;
        private Popup popup;

        private Textbox essai_textbox;

        private List<string> ResultIdAddFriend = new List<string>();
        private List<Button> ResultButton = new List<Button>();

        public Search_friends()
        {
            essai_textbox = new Textbox(50, 50, 200, 40);

        }

        public void Update(GameTime gametime)
        {
            essai_textbox.Update(gametime);
            if (essai_textbox.text != "" && essai_textbox.HasJustType)
                finish = true;

            if ((go_back.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick()))
                goback = true;

            for (int i = 0; i < ResultButton.Count; i++)
            {
                if (ResultButton[i] != null)
                {
                    ResultButton[i].Update();
                    if (ResultButton[i].Clicked && popup == null)
                        AddFriend(ResultIdAddFriend[i]);
                }

            }


            if (popup != null)
            {
                popup.Update(gametime);
                if (popup.action1bool)
                    popup = null;
            }

            if(essai_textbox.HasJustType)
                GetFriends();
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
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/search_friends.php"), "POST", data);
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
                    ResultButton = new List<Button>();
                    ResultIdAddFriend = new List<string>();
                    finish = true;
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    error = values["error"].ToString();
                    List<Dictionary<string, string>> ValueList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(values["thearray"].ToString());
                    getFriends = ValueList;

                    for (int i = 0; i < ValueList.Count; i++)
                    {
                        if (ValueList[i]["relation"] == "Add friend")
                        {
                            ResultButton.Add(new Button("Add friend", contentcreate.X + 300, contentcreate.Y + 50 + i * 50, Textures.font_texture));
                            ResultIdAddFriend.Add(ValueList[i]["id"]);
                        }
                        else
                        {
                            ResultButton.Add(null);
                            ResultIdAddFriend.Add("0");
                        }

                    }
                        
                }
            }
            catch
            {

            }
        }

        private void AddFriend(string id)
        {
            try
            {
                WebClient wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted2);
                data["id"] = Session.session_id;
                data["pwd"] = Session.session_password;
                data["otherid"] = id;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/add_friends.php"), "POST", data);
                
            }
            catch
            {
                popup = new Popup("OK", "", "Information", new string[] { "Sorry we are unable to connect to the website" }, Textures.font_texture, 450);
            }
        }

        void client_UploadFileCompleted2(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    string result = System.Text.Encoding.UTF8.GetString(e.Result);
                    popup = new Popup("OK", "", "Information", new string[] { result }, Textures.font_texture, 450);
                    finish = true;
                    GetFriends();
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

            sb.DrawString(Textures.font_texture, "Go back", new Vector2(go_back.X, go_back.Y), Color.White);

            sb.Draw(Textures.hitbox, titlecreate, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, titlecreate, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Search people", AlignType.MiddleCenter, titlecreate);
            sb.Draw(Textures.hitbox, contentcreate, Color.Beige);
            Tools.DisplayBorder(sb, Color.Black, contentcreate, 4);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, "Results", AlignType.MiddleCenter, new Rectangle(contentcreate.X, contentcreate.Y, contentcreate.Width, 50));


            if (getFriends != null)
                for (int i = 0; i < getFriends.Count; i++)
                {
                    Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, getFriends[i]["name"], AlignType.MiddleCenter, new Rectangle(contentcreate.X + 100, contentcreate.Y + 50 + i * 50, 100, 50));
                        if (ResultButton[i] != null)
                            ResultButton[i].Display(sb);
                        else
                            Tools.DisplayAlignedText(sb, Color.Black, Textures.font_texture, getFriends[i]["relation"], AlignType.MiddleCenter, new Rectangle(contentcreate.X + 300, contentcreate.Y + 50 + i * 50, 200, 40));
                }

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Create game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            if (popup != null)
                popup.Display(sb);

            essai_textbox.Display(sb, false);
            sb.End();
        }
    }
}