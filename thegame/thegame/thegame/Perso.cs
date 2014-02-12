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

        public Vector2 positionPerso, tempCurrentFrame;
      //  Vector2 velocity;
      //  float gravity = 0.1f;
        public Rectangle hitBoxPerso;


        float sol = 380;
        float positionTop;
        float minnewYpos;
        bool jumping;
        bool Adapt;
        float Gravity;
        bool movedown;

        public float offset;
        float newYpos;
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
            offset = 0;

            movedown = true;
            newYpos = 0;
            Gravity = 18f; /* YOU CAN CHANGE THE GRAVITY BY WHATEVER YOU WANT */
            imagePerso = Textures.mario_texture;
            animationPerso.AnimationSprite = Textures.mario_texture;
            animationPerso.Position = positionPerso;
            hitBoxPerso = new Rectangle((int)(positionPerso.X - imagePerso.Width / 2), (int)(positionPerso.Y - imagePerso.Height / 2), imagePerso.Width, imagePerso.Height);
        }

        public void Update(GameTime gametime, KeyboardState keyboardState , KeyboardState oldkey, bool moveleft, bool moveright, List<Rectangle> blocksTop)
        {
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            movedown = true;
            Adapt = false;

            


            foreach (Rectangle top in blocksTop)
            {
                if ((new Rectangle(top.X + (int)offset, top.Y, top.Width, top.Height)).Intersects(hitBoxPerso))
                {
                    movedown = false;

                }
            }

            if (jumping)
            {
                float positioninitiale = positionPerso.Y;
                
                if (positionPerso.Y > positionTop)
                {
                    positionPerso.Y -= Gravity;
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
                    positionTop = positionPerso.Y - 100;
                }
               
                if(movedown && positionPerso.Y + Gravity > sol) /* The perso does not fall */
                    positionPerso.Y = sol;

                /* THIS PART IS VERY VERY IMPORTANT */
                minnewYpos = 50000;
                if (movedown)
                {
                    
                    for (float i = 0; i < Gravity; i++)
                    {
                        newYpos = minnewYpos;
                        foreach (Rectangle top in blocksTop)
                        {
                            newYpos = minnewYpos;
                            if ((new Rectangle(top.X + (int)offset, top.Y, top.Width, top.Height)).Intersects(new Rectangle(hitBoxPerso.X, hitBoxPerso.Y + (int)i + 28, 28, 1)))
                            {
                                Adapt = true;
                                minnewYpos = top.Top - 26;
                                minnewYpos = Math.Min(minnewYpos, newYpos);
                                
                            }
                        }
                    }
                }



                /* END OF THE IMPORTANT PART THAT WAS GENERATING BUGS. */


                if (movedown && positionPerso.Y + 1 < sol && !Adapt) /* The perso fall */
                    positionPerso.Y += Gravity; /* I putthree for a reason! Generates beug otherwise */

                /* THIS PART IS VERY IMPORTANT */
                if (Adapt)
                {
                    positionPerso.Y = minnewYpos;
                    
                }
                /* END OF THE IMPORTANT PART */
            }
        

            if (keyboardState.IsKeyDown(Keys.Right) && moveright)
            {
                tempCurrentFrame.Y = 0;
                if (positionPerso.X < 400)
                    positionPerso.X += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                else
                    offset -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && moveleft)
            {
                tempCurrentFrame.Y = 1;
                if(offset > 0)
                    positionPerso.X -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                else
                    offset += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            
          
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

