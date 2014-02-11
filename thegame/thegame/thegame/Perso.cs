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
      //  Vector2 velocity;
      //  float gravity = 0.1f;
        public Rectangle hitBoxPerso;
        public Rectangle HitboxPerso { get; private set; }

        float sol, jumpspeed = 0;
        bool jumping;
        protected Texture2D imagePerso { get; private set; }


        public Vector2 Position { get; private set; }

        float speed;
        public float Speed { get; private set; }

        Animation animationPerso;

        public Perso(Vector2 pos)
        {
            animationPerso = new Animation(positionPerso, new Vector2(8, 2));
            tempCurrentFrame = Vector2.Zero;
            positionPerso = pos;
            speed = 100f;
            sol = positionPerso.Y;
            jumping = false;
            jumpspeed = 0;




            imagePerso = Textures.mario_texture;
            animationPerso.AnimationSprite = Textures.mario_texture;
            animationPerso.Position = positionPerso;
            hitBoxPerso = new Rectangle((int)(positionPerso.X - imagePerso.Width / 2), (int)(positionPerso.Y - imagePerso.Height / 2), imagePerso.Width, imagePerso.Height);

        }

        public void Update(GameTime gametime)
        {
            keyboardState = Keyboard.GetState();
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            if (jumping)
            {
                positionPerso.Y += jumpspeed;
                jumpspeed += 1;
                if (positionPerso.Y >= sol)
                {
                    positionPerso.Y = sol;
                    jumping = false;
                }
            }

            else
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    jumping = true;
                    jumpspeed = -18;
                }
            }
          /*  velocity.Y += gravity;
            positionPerso.X += velocity.X;
            positionPerso.Y += velocity.Y;
            if (gravity > 0.4f)
            {
                gravity = 0.4f;
            }*/

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
                // tempCurrentFrame.Y = 0; 
                positionPerso.Y -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                // tempCurrentFrame.Y = 1; /*Pour se baisser, placer les lignes de sprites respectivement en dessous de celles correspondant à la direction en cours. Et faire ".Y+1".
                positionPerso.Y += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else
                animationPerso.Actif = false;

            tempCurrentFrame.X = animationPerso.CurrentFrame.X;
            animationPerso.Position = positionPerso;
            animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
            hitBoxPerso = new Rectangle((int)(positionPerso.X - imagePerso.Width / 2), (int)(positionPerso.Y - imagePerso.Height / 2), imagePerso.Width, imagePerso.Height);
            
        }
        public  void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(imagePerso, positionPerso, new Rectangle(0, 0, imagePerso.Width / 3 , imagePerso.Height / 2), Color.White);
            animationPerso.Draw(spriteBatch);
        }
    }
}

