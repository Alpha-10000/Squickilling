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
    class HasJoined
    {


        private Rectangle TheBox = new Rectangle(50, 50, 750, 400);
        private Button go_back;
        public bool goback = false;
        public bool thereisone = false;

        private List<Dictionary<string, string>> ListOfParticipatings = new List<Dictionary<string, string>>();
        private float timeintervall = 8000;
        private float timelapsed = 0;
        private string debug = "";
        public string theidToJoin;

        public HasJoined()
        {
            go_back = new Button("Go back", 620, 10, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
            GetInvitations();
        }

        public void Update(GameTime gametime)
        {
            go_back.Update();
            if (go_back.Clicked)
                goback = true;

            timelapsed += gametime.ElapsedGameTime.Milliseconds;
            if (timelapsed >= timeintervall)
            {
                timelapsed = 0;
                GetInvitations();
                CheckGame();
            }
            
        }

        private void GetInvitations()
        {

                try
                {
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/has_joined.php"), "POST", data);
                }
                catch
                {
                    //TODO
                }
     
        }

        void client_UploadFileCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {

                if (e.Result != null)
                {
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    List<Dictionary<string, string>> ValueList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(values["thearray"].ToString());
                    ListOfParticipatings = ValueList;
                    debug = values["error"].ToString();
                }
            }
            catch
            {

            }
        }


        private void CheckGame()
        {

            try
            {
                WebClient wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted2);
                data["pwd"] = Session.session_password;
                data["id"] = Session.session_id;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/waiting_game.php"), "POST", data);
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
                    if (text != "Fuck" && text != "Login error" && text != "")
                    {
                        thereisone = true;
                        theidToJoin = text;
                    }
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


            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Friends game", AlignType.MiddleCenter, new Rectangle(TheBox.X, TheBox.Y, TheBox.Width / 2, 60));
            sb.Draw(Textures.hitbox, new Rectangle(TheBox.X, TheBox.Y + 52, TheBox.Width, 1), Color.White);

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "You have joined a game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            if (ListOfParticipatings != null)
                for(int i = 0; i < ListOfParticipatings.Count; i++)
                    Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, ListOfParticipatings[i]["name"] + " has joined the game", AlignType.MiddleCenter, new Rectangle(TheBox.X, TheBox.Y + 100 + i * 50, TheBox.Width / 2, 60));


            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Waiting for users...", AlignType.MiddleCenter, new Rectangle(TheBox.X, TheBox.Y + 450 , TheBox.Width / 2, 60));
            sb.End();
        }


    }
}
