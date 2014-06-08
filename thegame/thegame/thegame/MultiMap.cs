using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.IO;

namespace thegame
{
    class MultiMap
    {
        /* TWO METHOD HERE. JUST SOME TESTS 
        /* PLAYER 1  */
        public bool isPlayer1 = true;
        private Rectangle mybox, otherplayerbox;
        private WebClient wb;
        int X, Y;
        string url;
        float buffer = 0;
        float bufferWaitingTime = 50;
        private bool finish = true;

        private bool moveright, moveleft, movedown, moveup;
        private bool moveright2, moveleft2, movedown2, moveup2;

        public MultiMap()
        {
            wb = new WebClient();
            mybox = new Rectangle(0, 0, 50, 50);
            otherplayerbox = new Rectangle(50, 50, 50, 50);
            moveright = moveleft = movedown = moveup = moveright2 = moveleft2 = movedown2 = moveup2 = false;
        }

        public void Update(GameTime gametime)
        {
            moveright = moveleft = movedown = moveup = false;

            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                mybox = new Rectangle(mybox.X + 1, mybox.Y, mybox.Width, mybox.Height);
                moveright = true;
            }
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                mybox = new Rectangle(mybox.X, mybox.Y - 1, mybox.Width, mybox.Height);
                moveup = true;
            }
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                mybox = new Rectangle(mybox.X - 1, mybox.Y, mybox.Width, mybox.Height);
                moveleft = true;
            }
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                mybox = new Rectangle(mybox.X, mybox.Y + 1, mybox.Width, mybox.Height);
                movedown = true;
            }

            if (moveright2)
                otherplayerbox = new Rectangle(otherplayerbox.X + 1, otherplayerbox.Y, otherplayerbox.Width, otherplayerbox.Height);  
            if (moveup2)
                otherplayerbox = new Rectangle(otherplayerbox.X, otherplayerbox.Y - 1, otherplayerbox.Width, otherplayerbox.Height);
            if (moveleft2)
                otherplayerbox = new Rectangle(otherplayerbox.X - 1, otherplayerbox.Y, otherplayerbox.Width, otherplayerbox.Height);
            if (movedown2)
                otherplayerbox = new Rectangle(otherplayerbox.X, otherplayerbox.Y + 1, otherplayerbox.Width, otherplayerbox.Height);


            UpdateDDB();



        }

        private void UpdateDDB()
        {
            if (finish)
            {
                finish = false;
                wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                data["whichuser"] = "player1";
                string poss = "";
                if (moveright)
                    poss += "moveright,";
                else
                    poss += " ,";
                if (moveup)
                    poss += "moveup,";
                else
                    poss += " ,";
                if (moveleft)
                    poss += "moveleft,";
                else
                    poss += " ,";
                if (movedown)
                    poss += "movedown";
                else
                    poss += " ";
                data["player1pos"] = poss;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/test.php"), "POST", data);
            }
        }

        void client_UploadFileCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                finish = true;
                string text = System.Text.Encoding.UTF8.GetString(e.Result);

                List<string> myObj = new List<string>();
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                moveright2 = values["player2response"].Split(',')[0].Trim() == "moveright" ? true : false;
                moveup2 = values["player2response"].Split(',')[1].Trim() == "moveup" ? true : false;
                moveleft2 = values["player2response"].Split(',')[2].Trim() == "moveleft" ? true : false;
                movedown2 = values["player2response"].Split(',')[3].Trim() == "movedown" ? true : false;

            }
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Textures.hitbox, mybox, Color.Blue);
            sb.Draw(Textures.hitbox, otherplayerbox, Color.Red);
            sb.End();

        }
        


        /* player 2
        public bool isPlayer1 = false;
        private Rectangle mybox, otherplayerbox;
        private WebClient wb;
        int X, Y;
        string url;
        float buffer = 0;
        float bufferWaitingTime = 50;
        private bool finish = true;

        private bool moveright, moveleft, movedown, moveup ;
        private bool moveright2, moveleft2, movedown2, moveup2 ;

        public MultiMap()
        {
            wb = new WebClient();
            mybox = new Rectangle(0, 0, 50, 50);
            otherplayerbox = new Rectangle(50, 50, 50, 50);
            moveright = moveleft = movedown = moveup = moveright2 = moveleft2 = movedown2 = moveup2 = false;
        }

        public void Update(GameTime gametime)
        {
            moveright2 = moveleft2 = movedown2 = moveup2 = false;

            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                otherplayerbox = new Rectangle(otherplayerbox.X + 1, otherplayerbox.Y, otherplayerbox.Width, otherplayerbox.Height);
                moveright2 = true;
            }
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                otherplayerbox = new Rectangle(otherplayerbox.X, otherplayerbox.Y - 1, otherplayerbox.Width, otherplayerbox.Height);
                moveup2 = true;
            }
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                otherplayerbox = new Rectangle(otherplayerbox.X - 1, otherplayerbox.Y, otherplayerbox.Width, otherplayerbox.Height);
                moveleft2 = true;
            }
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                otherplayerbox = new Rectangle(otherplayerbox.X, otherplayerbox.Y + 1, otherplayerbox.Width, otherplayerbox.Height);
                movedown2 = true;
            }

            if(moveright)
                mybox = new Rectangle(mybox.X + 1, mybox.Y, mybox.Width, mybox.Height);
            if (moveup)
                mybox = new Rectangle(mybox.X, mybox.Y - 1, mybox.Width, mybox.Height);
            if (moveleft)
                mybox = new Rectangle(mybox.X - 1, mybox.Y, mybox.Width, mybox.Height);
            if (movedown)
                mybox = new Rectangle(mybox.X, mybox.Y + 1, mybox.Width, mybox.Height);


            UpdateDDB();



        }

        private void UpdateDDB()
        {
            if (finish)
            {
                finish = false;
                wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                data["whichuser"] = "player2";
                string poss = "";
                if (moveright2)
                    poss += "moveright,";
                else
                    poss += " ,";
                if (moveup2)
                    poss += "moveup,";
                else
                    poss += " ,";
                if (moveleft2)
                    poss += "moveleft,";
                else
                    poss += " ,";
                if (movedown2)
                    poss += "movedown";
                else
                    poss += " ";
                data["player2pos"] = poss;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/test.php"), "POST", data);
            }
        }

        void client_UploadFileCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                finish = true;
                string text = System.Text.Encoding.UTF8.GetString(e.Result);

                List<string> myObj = new List<string>();
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                moveright = values["player1response"].Split(',')[0].Trim() == "moveright" ? true : false ;
                moveup = values["player1response"].Split(',')[1].Trim() == "moveup" ? true : false;
                moveleft = values["player1response"].Split(',')[2].Trim() == "moveleft" ? true : false;
                movedown = values["player1response"].Split(',')[3].Trim() == "movedown" ? true : false;
            }
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Textures.hitbox, mybox, Color.Blue);
            sb.Draw(Textures.hitbox, otherplayerbox, Color.Red);
            sb.End();

        }
         
         

        /*
        
        public bool isPlayer1 = false;
        private Rectangle mybox, otherplayerbox;
        private WebClient wb;
        int X, Y;
        string url;
        float buffer = 0;
        float bufferWaitingTime = 50;
        private bool finish = true;
        public MultiMap()
        {
            wb = new WebClient();
            mybox = new Rectangle(0, 0, 50, 50);
            otherplayerbox = new Rectangle(50, 50, 50, 50);
        }

        public void Update(GameTime gametime)
        {
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                otherplayerbox = new Rectangle(otherplayerbox.X + 1, otherplayerbox.Y, otherplayerbox.Width, otherplayerbox.Height);
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                otherplayerbox = new Rectangle(otherplayerbox.X , otherplayerbox.Y - 1, otherplayerbox.Width, otherplayerbox.Height);
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                otherplayerbox = new Rectangle(otherplayerbox.X - 1, otherplayerbox.Y, otherplayerbox.Width, otherplayerbox.Height);
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                otherplayerbox = new Rectangle(otherplayerbox.X, otherplayerbox.Y + 1, otherplayerbox.Width, otherplayerbox.Height);

   
                UpdateDDB();
       

            
        }

        private void UpdateDDB()
        {
            if (finish)
            {
                finish = false;
                wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                data["whichuser"] = "player2";
                data["player2pos"] = otherplayerbox.X + "," + otherplayerbox.Y;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/test.php"), "POST", data);
            }
        }

        void client_UploadFileCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                finish = true;
                string text = System.Text.Encoding.UTF8.GetString(e.Result);

                List<string> myObj = new List<string>();
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                X = Convert.ToInt32(values["player1response"].Split(',')[0].Trim());
                Y = Convert.ToInt32(values["player1response"].Split(',')[1].Trim());
                mybox = new Rectangle(X, Y, 50, 50);
            }
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Textures.hitbox, mybox, Color.Blue);
            sb.Draw(Textures.hitbox, otherplayerbox, Color.Red);
            sb.End();

        }

        

        /* PLAYER 2 
        public bool isPlayer1 = true;
        private Rectangle mybox, otherplayerbox;
        private WebClient wb;
        int X, Y;
        string url;
        float buffer = 0;
        float bufferWaitingTime = 50;
        private bool finish = true;

        public MultiMap()
        {
            wb = new WebClient();
            mybox = new Rectangle(0, 0, 50, 50);
            mybox = new Rectangle(50, 50, 50, 50);
        }

        public void Update(GameTime gametime)
        {
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                mybox = new Rectangle(mybox.X + 1, mybox.Y, mybox.Width, mybox.Height);
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                mybox = new Rectangle(mybox.X, mybox.Y - 1, mybox.Width, mybox.Height);
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                mybox = new Rectangle(mybox.X - 1, mybox.Y, mybox.Width, mybox.Height);
            if (Inputs.isKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                mybox = new Rectangle(mybox.X, mybox.Y + 1, mybox.Width, mybox.Height);

        
                UpdateDDB();
        

            
        }

        private void UpdateDDB()
        {
            if (finish)
            {
                finish = false;
                wb = new WebClient();
                var data = new NameValueCollection();
                wb.UploadValuesCompleted += new UploadValuesCompletedEventHandler(client_UploadFileCompleted);
                data["whichuser"] = "player1";
                data["player1pos"] = mybox.X + "," + mybox.Y;
                wb.UploadValuesAsync(new Uri("http://squickilling.com/json/test.php"), "POST", data);
            }
        }

        void client_UploadFileCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                finish = true;
                string text = System.Text.Encoding.UTF8.GetString(e.Result);

                List<string> myObj = new List<string>();
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                X = Convert.ToInt32(values["player2response"].Split(',')[0].Trim());
                Y = Convert.ToInt32(values["player2response"].Split(',')[1].Trim());
                otherplayerbox = new Rectangle(X, Y, 50, 50);
            }
        }

        public void Display(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Textures.hitbox, mybox, Color.Blue);
            sb.Draw(Textures.hitbox, otherplayerbox, Color.Red);
            sb.End();

        }
        */
    }
}
