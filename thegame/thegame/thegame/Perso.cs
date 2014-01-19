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
        Vector2 positionPerso, tempCurrentFrame;
        KeyboardState keyboardState;
        protected Texture2D imagePerso;
        public Texture2D ImagePerso
        {
            get { return imagePerso; }
            set { imagePerso = value; }
        }

        
        public Vector2 Position
        {
            get { return positionPerso; }
            set { positionPerso = value; }
        }

        float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        Animation animationPerso = new Animation();
        public void Initialize()
        {
            animationPerso.Initialize(positionPerso, new Vector2(3, 2));
            tempCurrentFrame = Vector2.Zero;
            speed = 100f;
        }
        public void LoadContent(ContentManager Content, string assetName)
        {
            imagePerso = Content.Load<Texture2D>(assetName);
            positionPerso = new Vector2(100, 100);
            animationPerso.AnimationSprite = imagePerso;
            animationPerso.Position = positionPerso;
        }

        
        public void Update(GameTime gametime)
        {
            keyboardState = Keyboard.GetState();
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                tempCurrentFrame.Y = 0;
                positionPerso.X += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                tempCurrentFrame.Y = 1;
                positionPerso.X -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                tempCurrentFrame.Y = 0;
                positionPerso.Y -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                tempCurrentFrame.Y = 1;
                positionPerso.Y += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else
                animationPerso.Actif = false;
            tempCurrentFrame.X = animationPerso.CurrentFrame.X;
            animationPerso.Position = positionPerso;
            animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            animationPerso.Draw(spriteBatch);
        }
    }
}

