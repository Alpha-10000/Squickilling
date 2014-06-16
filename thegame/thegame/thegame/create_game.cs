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
        private Rectangle go_back = new Rectangle(600, 20, 600, 40);
        public bool goback = false;

        /* Content create */
        private Rectangle contentcreate = new Rectangle(175, 70, 450, 220);
        private Rectangle BottomBox = new Rectangle(50, 315, 700, 185);
        private bool finish = false;

        private string error;
        private List<Dictionary<string, string>> getFriends;
        private List<Button> inviteButtonList = new List<Button>();
        private Popup popup;

        private Textbox essai_textbox;

        public Create_game()
        {
            essai_textbox = new Textbox(400, 75, 200, 40);

        }

        public void Update(GameTime gametime)
        {
            essai_textbox.Update(gametime);
            if (essai_textbox.text != "" && essai_textbox.HasJustType)
                finish = true;

            if ((go_back.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick()))
                goback = true;
        
            if (popup != null)
            {
                popup.Update(gametime);
                if (popup.action1bool)
                    popup = null;
            }

            GetFriends(gametime);
            if (getFriends != null)
                for (int i = 0; i < getFriends.Count; i++)
                {
                    inviteButtonList[i].Update();
                    if(inviteButtonList[i].Clicked)
                        popup = new Popup("OK", "", "Information", new string[] { "We are developping the functionnality" }, Textures.font_texture, 450);
                }
        }

        private void GetFriends(GameTime gametime)
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

                    for (int i = 0; i < ValueList.Count; i++)
                        inviteButtonList.Add(new Button("Invite", contentcreate.X + 300, contentcreate.Y + 50 + i * 50, Textures.fonthelp_texture));
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

            sb.Draw(Textures.hitbox, contentcreate, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White, contentcreate, 2);
            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Search friends : ", AlignType.MiddleCenter, new Rectangle(contentcreate.X, contentcreate.Y, contentcreate.Width / 2, 50));

            sb.Draw(Textures.hitbox, BottomBox, Color.Black * 0.4f);
            Tools.DisplayBorder(sb, Color.White,  BottomBox, 2);

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Invited friends", AlignType.MiddleCenter, new Rectangle(BottomBox.X, BottomBox.Y, BottomBox.Width / 4, 50));
            Tools.DisplayAlignedText(sb, Color.White, Textures.fonthelp_texture, "Nobody has been invited yet", AlignType.MiddleCenter, new Rectangle(BottomBox.X + 25, BottomBox.Y, BottomBox.Width / 3, BottomBox.Height));

            if (getFriends != null)
                for (int i = 0; i < getFriends.Count; i++)
                {
                    Tools.DisplayAlignedText(sb, Color.White, Textures.fonthelp_texture, getFriends[i]["name"] + " : " + getFriends[i]["status"], AlignType.MiddleCenter, new Rectangle(contentcreate.X + 100, contentcreate.Y + 50 + i * 50, 100, 50));
                    inviteButtonList[i].Display(sb);
                    
                }

            Tools.DisplayAlignedText(sb, Color.White, Textures.font_texture, "Create game", AlignType.MiddleCenter, new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, 50));

            Tools.DrawCircle(sb, 653, 405, 50, 600, Color.White, 120);
            Tools.DisplayAlignedText(sb, Color.Black, Textures.fonthelp_texture, "Launch Game", AlignType.MiddleCenter, new Rectangle(653 - 50, 405 - 50, 100, 100));

            sb.DrawString(Textures.fonthelp_texture, "Make it public", new Vector2(450, 390), Color.White);
            sb.Draw(Textures.hitbox, new Rectangle(420, 392, 20, 20), Color.White);

            if (popup != null)
                popup.Display(sb);

            essai_textbox.Display(sb, false);
            sb.End();
        }
    }
}
