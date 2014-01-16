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
        protected Texture2D imagePerso;
        public Texture2D ImagePerso
        {
            get { return imagePerso; }
            set { imagePerso = value; }
        }

        protected Vector2 positionPerso;
        public Vector2 Position
        {
            get { return positionPerso; }
            set { positionPerso = value; }
        }

        protected float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        protected Vector2 tempCurrentFrame;
        Animation animationPerso = new Animation();
        public void Initialize()
        {
            animationPerso.Initialize(positionPerso, new Vector2(11, 1));
            tempCurrentFrame = Vector2.Zero;
        //    position = new Vector2(0, 0);
          //  speed = 0.2f;
        }
        public void LoadContent(ContentManager Content, string assetName)
        {
            imagePerso = Content.Load<Texture2D>(assetName);
            animationPerso.AnimationSprite = imagePerso;
        }

        KeyboardState keyboardState;
        public void Update(GameTime gametime)
        {
          //  keyboardState = Keyboard.GetState();
            animationPerso.Actif = true;
          //  positionPerso = animationPerso.Position;
          //  if (keyboardState.IsKeyDown(Keys.Right))
          //  {
           //     positionPerso.X += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
           //     tempCurrentFrame.X= 0;
          //  }
          //  animationPerso.Position = positionPerso;
          //  animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            animationPerso.Draw(spriteBatch);
        }
    }
}

