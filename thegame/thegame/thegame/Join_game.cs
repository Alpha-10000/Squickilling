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
    class Join_game
    {
        private Button go_back;
        public bool goback = false;


        /* Content create */
        private Rectangle contentjoin = new Rectangle(20, 70, 350, 350);

        private List<Dictionary<string, string>> MyFriendsGame = new List<Dictionary<string, string>>();
        private List<Button> JoinButton = new List<Button>();
        private bool finish;
        private float TimeInterval = 10000;
        private float GameTimeGetInvitations;

        public bool HasJoined = false;

        public Join_game()
        {
            finish = true;
            GameTimeGetInvitations = TimeInterval;
            go_back = new Button(Language.Text_Game["_btnBack"], 620, 10, Textures.font_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
        }

        public void Update(GameTime gametime)
        {
            go_back.Update();
            if (go_back.Clicked)
                goback = true;

            
           
            GetInvitations(gametime);

            if (JoinButton != null && JoinButton.Count > 0)
                for (int i = 0; i < JoinButton.Count; i++)
                {
                    JoinButton[i].Update();
                    if (JoinButton[i].Clicked)
                        Join(MyFriendsGame[i]["otherid"]);
                }
        }

        private void GetInvitations(GameTime gametime)
        {
            GameTimeGetInvitations += gametime.ElapsedGameTime.Milliseconds;
            if (finish && GameTimeGetInvitations >= TimeInterval)
            {
                try
                {
                    GameTimeGetInvitations = 0;
                    finish = false;
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/list_join.php"), "POST", data);
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
                    JoinButton = new List<Button>();
                    string text = System.Text.Encoding.UTF8.GetString(e.Result);
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    List<Dictionary<string, string>> ValueList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(values["thearray"].ToString());
                    MyFriendsGame = ValueList;

                    for (int i = 0; i < ValueList.Count; i++)
                        JoinButton.Add(new Button(Language.Text_Game["_btnJoin"], contentjoin.X + 128, contentjoin.Y + 115 + i * 100, Textures.font_texture, new Color(129, 130, 134), Color.White, new Color(14, 15, 15)));

                }
            }
            catch
            {

            }
        }

        private void Join(string otherid)
        {
                try
                {
                    GameTimeGetInvitations = 0;
                    finish = false;
                    WebClient wb = new WebClient();
                    var data = new NameValueCollection();
                    wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted2);
                    data["pwd"] = Session.session_password;
                    data["id"] = Session.session_id;
                    data["otherid"] = otherid;
                    wb.UploadValuesAsync(new Uri("http://squickilling.com/json/join_game.php"), "POST", data);
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
                    if (text == "Success")
                        HasJoined = true;
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

          
            sb.Draw(Textures.hitbox, contentjoin, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White, contentjoin, 2);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Friends game", AlignType.MiddleCenter, new Rectangle(contentjoin.X, contentjoin.Y, contentjoin.Width / 2, 60));
            sb.Draw(Textures.hitbox, new Rectangle(contentjoin.X, contentjoin.Y + 52, contentjoin.Width, 1), Color.White);
            if (MyFriendsGame.Count == 0)
            {
                Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "No games at the time", AlignType.MiddleCenter, new Rectangle(contentjoin.X, contentjoin.Y + 65, contentjoin.Width, 60));
                Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Try to add some friends", AlignType.MiddleCenter, new Rectangle(contentjoin.X, contentjoin.Y + 100, contentjoin.Width, 60));
            }
            else
            {
                for(int i = 0; i < MyFriendsGame.Count; i++)
                    Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, MyFriendsGame[i]["name"] + " has invited you", AlignType.MiddleCenter, new Rectangle(contentjoin.X, contentjoin.Y + 65 + i * 60, contentjoin.Width, 60));
            }

            if (JoinButton != null && JoinButton.Count > 0)
                for (int i = 0; i < JoinButton.Count; i++)
                    JoinButton[i].Display(sb);

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Join game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            

            sb.End();
        }
    }
}
