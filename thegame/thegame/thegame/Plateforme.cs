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

namespace thegame
{
    class Plateforme
    {
        Vector2 positionPlatform;
       // Rectangle hitBoxPlatform;

        protected Texture2D imagePlatform;
        public Texture2D ImagePlatform
        {
            get { return imagePlatform; }
            set { imagePlatform = value; }
        }

        public Vector2 Position
        {
            get { return positionPlatform; }
            set { positionPlatform = value; }
        }

        public void Initialize(int x, int y)
        {
            positionPlatform = new Vector2(x, y);
        }

        public void LoadContent(ContentManager Content, string assetName)
        {

            imagePlatform = Content.Load<Texture2D>(assetName);
            Rectangle hitBoxPerso = new Rectangle((int)(positionPlatform.X - imagePlatform.Width / 2), (int)(positionPlatform.Y - imagePlatform.Height / 2), imagePlatform.Width, imagePlatform.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(imagePlatform, positionPlatform, new Rectangle(0, 0, imagePlatform.Width, imagePlatform.Height), Color.White);
        }
    }
}
