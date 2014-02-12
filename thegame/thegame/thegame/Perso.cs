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
      //  Vector2 velocity;
      //  float gravity = 0.1f;
        public Rectangle hitBoxPerso;

        public float gravity = 5f;
        float sol = 380;
        float positionTop;
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
            jumping = false;
 




            imagePerso = Textures.mario_texture;
            animationPerso.AnimationSprite = Textures.mario_texture;
            animationPerso.Position = positionPerso;
            hitBoxPerso = new Rectangle((int)(positionPerso.X - imagePerso.Width / 2), (int)(positionPerso.Y - imagePerso.Height / 2), imagePerso.Width, imagePerso.Height);
        }

        public void Update(GameTime gametime, KeyboardState keyboardState , KeyboardState oldkey, bool movedown, bool moveleft, bool moveright)
        {
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            if (jumping)
            {
                float positioninitiale = positionPerso.Y;
                
                if (positionPerso.Y > positionTop)
                {
                    positionPerso.Y -= 5;
                }
                else
                {
                    jumping = false;
                }
              
            }

            else
            {
                if (keyboardState.IsKeyDown(Keys.Space) && !oldkey.IsKeyDown(Keys.Space) && jumping == false && (!movedown || positionPerso.Y == sol))
                {
                    jumping = true;
                    positionTop = positionPerso.Y - 50;
                }
                if (movedown && positionPerso.Y + 1 < sol)
                    positionPerso.Y += 1 * 3f; /* I put one for a reason! Generates beug otherwise */
                if(movedown && positionPerso.Y + 5 > sol)
                    positionPerso.Y = sol;
            }
          /*  velocity.Y += gravity;
            positionPerso.X += velocity.X;
            positionPerso.Y += velocity.Y;
            if (gravity > 0.4f)
            {
                gravity = 0.4f;
            }*/

            if (keyboardState.IsKeyDown(Keys.Right) && moveright)
            {
                tempCurrentFrame.Y = 0;
                positionPerso.X += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && moveleft)
            {
                tempCurrentFrame.Y = 1;
                positionPerso.X -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            
           /* else if (keyboardState.IsKeyDown(Keys.Down) && movedown)
            {
                // tempCurrentFrame.Y = 1; /*Pour se baisser, placer les lignes de sprites respectivement en dessous de celles correspondant à la direction en cours. Et faire ".Y+1".
                positionPerso.Y += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }*/
            else
                animationPerso.Actif = false;

            

            tempCurrentFrame.X = animationPerso.CurrentFrame.X;
            animationPerso.Position = positionPerso;
            animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
            hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), 27, 28);
        }
        public  void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(imagePerso, positionPerso, new Rectangle(0, 0, imagePerso.Width / 3 , imagePerso.Height / 2), Color.White);
            animationPerso.Draw(spriteBatch);
           /* spriteBatch.Draw(Textures.hitbox, hitBoxPerso, Color.White); // debug perso hitbox */
        }
    }
}

