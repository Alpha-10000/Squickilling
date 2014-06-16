using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Net.Mail;
using System.Net;

namespace thegame
{
    class DevelopperMap
    {
        public Vector2 cameraPos = Vector2.Zero;
        public int[,] array;

        string email;

        Rectangle buche = new Rectangle(22, 452, Textures.buche_texture.Width, Textures.buche_texture.Height);
        Rectangle eraser = new Rectangle(722, 452, Textures.eraser.Width, Textures.eraser.Height);
        Rectangle bomb = new Rectangle(180, 452, Textures.mine_gray.Width, Textures.mine_gray.Height);
        Rectangle objectMap = new Rectangle(250, 452, 10, 10);
        Rectangle medecine = new Rectangle(280, 452, Textures.medecine.Width, Textures.medecine.Height);

        Rectangle Alpha = new Rectangle(409, 470, 59, 25);
        Rectangle Elise = new Rectangle(470, 470, 55, 25);
        Rectangle Thibault = new Rectangle(525, 471, 81, 25);
        Rectangle Victor = new Rectangle(614, 470, 60, 25);

        float timeElaspedShow = 0;

        bool bucheSelected = false;
        bool eraseSelected = false;
        bool bombSelected = false;
        bool objectSelect = false;
        bool medecineSelect = false;

        bool showinfoMail = false;
        bool showError = false;

        int DrawMouseX;
        int DrawMouseY;

        public bool showGrid = false;

        public DevelopperMap(int width, int height)
        {
            array = new int[width, height];
            for (int x = 0; x < array.GetLength(1); x++)
                for (int y = 0; y < array.GetLength(0); y++)
                    array[y, x] = 0;
        }

        public void UpdateMap(GameTime gametime)
        {

            DrawMouseX = (int)Inputs.getMousePos().X - (int)cameraPos.X;
            DrawMouseY = (int)Inputs.getMousePos().Y + (int)cameraPos.Y;

            if (Inputs.isKeyDown(Keys.S))
                showGrid = true;
            if (Inputs.isKeyDown(Keys.H))
                showGrid = false;

            if (Inputs.isKeyDown(Keys.Right))
            {
                float changement = 500f * (float)gametime.ElapsedGameTime.TotalSeconds;
                if (cameraPos.X < 5500)
                    cameraPos = new Vector2(cameraPos.X - changement, cameraPos.Y);
            }
            if (Inputs.isKeyDown(Keys.Left))
            {
                float changement = 500f * (float)gametime.ElapsedGameTime.TotalSeconds;
                if (cameraPos.X < 5500)
                    cameraPos = new Vector2(cameraPos.X + changement, cameraPos.Y);
            }

            if (buche.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                bucheSelected = true;
                eraseSelected = bombSelected = objectSelect = medecineSelect = false;
            }

            if (medecine.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                bucheSelected = eraseSelected = bombSelected = objectSelect = false;
                medecineSelect = true;
            }

            if (eraser.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                bucheSelected = bombSelected = objectSelect = medecineSelect = false;
                eraseSelected = true;
            }

            if (bomb.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                bucheSelected = eraseSelected = objectSelect = medecineSelect = false;
                bombSelected = true;
            }

            if (objectMap.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                bucheSelected = eraseSelected = bombSelected = medecineSelect = false;
                objectSelect = true;
            }

            if (Alpha.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                email = "alphamft@gmail.com";
                SendMail();
            }
            if (Elise.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                email = "elise.schrav@gmail.com";
                SendMail();
            }
            if (Thibault.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                email = "thibault.liotier@gmail.com";
                SendMail();
            }
            if (Victor.Contains(Inputs.getMousePoint()) && Inputs.isLMBClick())
            {
                email = "victor.boissiere@gmail.com";
                SendMail();
            }

            if (Inputs.isRightClick())
            {
                bucheSelected = eraseSelected = bombSelected = objectSelect = medecineSelect = false;
            }

            if (bucheSelected && Inputs.isLMBClick())
                UpdateArray(DrawMouseX, DrawMouseY, 1);
            if (eraseSelected && Inputs.isLMBClick())
                UpdateArray(DrawMouseX, DrawMouseY, 0);
            if (bombSelected && Inputs.isLMBClick())
                UpdateArray(DrawMouseX, DrawMouseY, 3);
            if (objectSelect && Inputs.isLMBClick())
                UpdateArray(DrawMouseX, DrawMouseY, 2);
            if (medecineSelect && Inputs.isLMBClick())
                UpdateArray(DrawMouseX, DrawMouseY, 4);

        }

        public void UpdateArray(int x, int y, int type)
        {
            int arrayY = x / Textures.buche_texture.Width;
            int arrayX = y / Textures.buche_texture.Height + 3;

            if (arrayX < array.GetLength(1) && arrayX >= 0)
                if (arrayY < array.GetLength(0) && arrayY >= 0)
                    array[arrayY, arrayX] = type;
        }

        public void SendMail()
        {
            try
            {
                //create the mail message
                MailMessage mail = new MailMessage();
                //set the FROM address
                mail.From = new MailAddress("developper@tivbroc.com");
                //set the RECIPIENTS
                mail.To.Add(email);
                //enter a SUBJECT
                mail.Subject = "Code for the map";
                //Enter the message BODY

                string text = "tilemap = new int[,] \n { \n";

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    text += "{";
                    for (int j = 0; j < array.GetLength(0); j++)
                    {
                        text += array[j, i] + ",";
                    }
                    text = text.Substring(0, text.Length - 1);
                    text += "}, \n";

                }
                text += "};";

                mail.Body = text;

                //set the mail server (default should be smtp.1and1.com)
                SmtpClient smtp = new SmtpClient("auth.smtp.1and1.fr");
                //Enter your full e-mail address and password
                smtp.Credentials = new NetworkCredential("developper@tivbroc.com", "epitapower94");

                smtp.EnableSsl = true;
                //send the message 
                smtp.Send(mail);

                mail.Dispose();
            }
            catch
            {
                showError = true;
            }
            showinfoMail = true;
        }


        public void ShowSuccessSendMail(SpriteBatch sb, GameTime gametime)
        {

            if (showinfoMail)
            {
                timeElaspedShow += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (timeElaspedShow < 2)
                {
                    Drawable ecri = new Drawable(drawable_type.font);
                    if (showError)
                        ecri.Draw(sb, "Error. Not sent", new Vector2(50, 250), Color.Black, "osef");
                    else
                        ecri.Draw(sb, "Successfully sent", new Vector2(50, 250), Color.Black, "osef");
                }
                else
                {
                    showinfoMail = showError = false;
                    timeElaspedShow = 0;
                }
            }
        }

        public void Display(SpriteBatch sb, GameTime gametime)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (showGrid)
                    {
                        sb.Draw(Textures.hitbox, new Rectangle(i * Textures.buche_texture.Width, j * Textures.buche_texture.Height - 95, Textures.buche_texture.Width, 1), Color.White * 0.5f);
                        sb.Draw(Textures.hitbox, new Rectangle(i * Textures.buche_texture.Width, j * Textures.buche_texture.Height - 95, 1, Textures.buche_texture.Height), Color.White * 0.5f);
                        sb.Draw(Textures.hitbox, new Rectangle(i * Textures.buche_texture.Width + Textures.buche_texture.Width, j * Textures.buche_texture.Height - 95 + Textures.buche_texture.Height, 1, Textures.buche_texture.Height), Color.White * 0.5f);
                        sb.Draw(Textures.hitbox, new Rectangle(i * Textures.buche_texture.Width, j * Textures.buche_texture.Height - 95 + Textures.buche_texture.Height, Textures.buche_texture.Width, 1), Color.White * 0.5f);
                    }
                    if (array[i, j] == 1)
                    {
                        sb.Draw(Textures.buche_texture, new Rectangle(i * Textures.buche_texture.Width, j * Textures.buche_texture.Height - 95, Textures.buche_texture.Width, Textures.buche_texture.Height), Color.Wheat);
                    }
                    else if (array[i, j] == 2)
                    {
                        sb.Draw(Textures.acorn_texture, new Rectangle(i * Textures.buche_texture.Width + 50, j * Textures.buche_texture.Height - 86, 10, 10), Color.White);
                    }
                    else if (array[i, j] == 3)
                    {
                        sb.Draw(Textures.mine_gray, new Rectangle(i * Textures.buche_texture.Width + 50, j * Textures.buche_texture.Height - 72, Textures.mine_gray.Width, Textures.mine_gray.Height), Color.White);
                    }
                    else if (array[i, j] == 4)
                    {
                        sb.Draw(Textures.medecine, new Rectangle(i * Textures.buche_texture.Width + 50, j * Textures.buche_texture.Height - 81, Textures.medecine.Width, Textures.medecine.Height), Color.White);
                    }
                }

            if (bucheSelected)
                sb.Draw(Textures.buche_texture, new Rectangle(DrawMouseX - (Textures.buche_texture.Width / 2), DrawMouseY - (Textures.buche_texture.Height / 2), Textures.buche_texture.Width, Textures.buche_texture.Height), Color.White);
            if (eraseSelected)
                sb.Draw(Textures.eraser, new Rectangle(DrawMouseX - (Textures.eraser.Width / 2), DrawMouseY - (Textures.eraser.Height / 2), Textures.eraser.Width, Textures.eraser.Height), Color.White);
            if (bombSelected)
                sb.Draw(Textures.mine_gray, new Rectangle(DrawMouseX - (Textures.mine_gray.Width / 2), DrawMouseY - (Textures.mine_gray.Height / 2), Textures.mine_gray.Width, Textures.mine_gray.Height), Color.White);
            if (objectSelect)
                sb.Draw(Textures.acorn_texture, new Rectangle(DrawMouseX - (Textures.nut_texture.Width / 2), DrawMouseY - (Textures.nut_texture.Height / 2), Textures.nut_texture.Width, Textures.nut_texture.Height), Color.White);
            if (medecineSelect)
                sb.Draw(Textures.medecine, new Rectangle(DrawMouseX - (Textures.medecine.Width / 2), DrawMouseY - (Textures.medecine.Height), Textures.medecine.Width, Textures.medecine.Height), Color.White);


            ShowSuccessSendMail(sb, gametime);
        }
    }
}
