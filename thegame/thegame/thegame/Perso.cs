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
    class Perso
    {
        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public void Initialize()
        {
            position = new Vector2(0, 0);
            speed = 0.2f;
        }
        public void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }
        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gametime)
        {
            speed = 1f;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                position.X -= speed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                position.X += speed;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                position.Y -= speed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                position.Y += speed;
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
        public Rectangle hitBox 
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            }
        }
    }
}

