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
           positionPerso = new Vector2(0, 500);
           speed = 100f;
        }
        public void LoadContent(ContentManager Content, string assetName)
        {
            imagePerso = Content.Load<Texture2D>(assetName);
            animationPerso.AnimationSprite = imagePerso;
        }

        
        public void Update(GameTime gametime)
        {
            keyboardState = Keyboard.GetState();
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            positionPerso = animationPerso.Position;
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                positionPerso.X += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                tempCurrentFrame.Y = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                positionPerso.X -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                tempCurrentFrame.Y = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                positionPerso.Y -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                tempCurrentFrame.Y = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                positionPerso.Y += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                tempCurrentFrame.Y = 1;
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

