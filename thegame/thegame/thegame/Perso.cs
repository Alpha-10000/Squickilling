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
        float minnewYpos;
        bool jumping;
        bool Adapt;
        float Gravity;
        bool movedown;
        float vel;
        float acc;

        public float offset;
        float newYpos;
        protected Texture2D imagePerso { get; private set; }


        public Vector2 Position { get; private set; }

        float speed;
        public float Speed { get; private set; }

        Animation animationPerso;
        List<Projectile> projs = new List<Projectile>();

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
            GravityInit();
            imagePerso = Textures.mario_texture;
            animationPerso.AnimationSprite = Textures.mario_texture;
            animationPerso.Position = positionPerso;
            hitBoxPerso = new Rectangle((int)(positionPerso.X - imagePerso.Width / 2), (int)(positionPerso.Y - imagePerso.Height / 2), imagePerso.Width, imagePerso.Height);
        }

        private void GravityInit()
        {
            vel = 10f; /* Kind of like the hight of the jump */
            acc = 150f; /* Duration of jump */
            Gravity = 0.5f; /* Start folling with this speed */
        }

        public void Update(GameTime gametime, KeyboardState keyboardState, KeyboardState oldkey, bool moveleft, bool moveright, List<Rectangle> blocksTop, List<Projectile> proj)
        {

            /* INITIALISATION */
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            movedown = true;
            Adapt = false;

            /*PROJECTILE*/

            Vector2 direction = new Vector2(1, 0);
            Vector2 positionNoix;
            if (animationPerso.CurrentFrame.Y == 1)
                positionNoix = new Vector2(positionPerso.X - 7, positionPerso.Y);
            else
                positionNoix = new Vector2(positionPerso.X + 27, positionPerso.Y);

            Projectile noix = new Projectile(drawable_type.Nut, positionNoix, positionNoix, 100, direction);

            if (keyboardState.IsKeyDown(Keys.Space) && !oldkey.IsKeyDown(Keys.Space))
            {
                projs.Add(noix);
            }
            for (int i = 0; i < projs.Count; i++)
            {
                projs[i].Update(gametime);
                if (projs[i].Visible == false)
                    projs.Remove(projs[i]);
            }

            /* CHECK TOP COLLISION */
            foreach (Rectangle top in blocksTop)
            {
                if ((new Rectangle(top.X + (int)offset, top.Y, top.Width, top.Height)).Intersects(hitBoxPerso))
                {
                    movedown = false;

                }
            }

            /* THE PERSO IS JUMPING - PART FROM BOTTOM TO TOP */
            if (jumping)
            {
                if (Gravity > 0)
                {
                    float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                    vel -= acc * dt;// v = u + a*t
                    Gravity += vel * dt;// s = u*t + 0.5*a*t*t,
                    positionPerso.Y -= Gravity;
                }
                else
                {
                    GravityInit();
                    jumping = false;
                }

            }
            else
            {
                /* IF PERSO CAN JUMP THEN JUMP */
                if (keyboardState.IsKeyDown(Keys.Up) && !oldkey.IsKeyDown(Keys.Up) && (!movedown || positionPerso.Y == sol))
                {
                    jumping = true;
                    Gravity = 5f; /* First jump speed while pressing the button*/
                }

                /* KEEP PERSO ON GROUND */
                if (movedown && positionPerso.Y + Gravity > sol)
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
                if (Adapt)
                {
                    positionPerso.Y = minnewYpos;

                }
            }
            /* END OF THE IMPORTANT PART THAT WAS GENERATING BUGS. */


            /* PERSO JUST TOUCHED THE GROUND SO INITIALIZE VALUE */
            if (!jumping && (positionPerso.Y == sol || !movedown || Adapt))
            {
                GravityInit();
            }

            /* GRAVITY - PERSO NOT JUMPING AND ON GROUND */
            if (movedown && !jumping && positionPerso.Y + 1 < sol && !Adapt)
            {
                float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                vel += acc * dt;// v = u + a*t
                Gravity += vel * dt;// s = u*t + 0.5*a*t*t,
                positionPerso.Y += Gravity; /* I putthree for a reason! Generates beug otherwise */
            }





            if (keyboardState.IsKeyDown(Keys.Right) && moveright && !keyboardState.IsKeyDown(Keys.Left))
            {
                tempCurrentFrame.Y = 0;
                if (positionPerso.X < 400)
                    positionPerso.X += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                else
                    offset -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && moveleft && !keyboardState.IsKeyDown(Keys.Right))
            {
                tempCurrentFrame.Y = 1;
                if (offset > 0)
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
        public void Draw(SpriteBatch spriteBatch)
        {

            animationPerso.Draw(spriteBatch);
            foreach (Projectile nut in projs)
                nut.Draw(spriteBatch);

            /*
            Drawable debug = new Drawable(drawable_type.font);
            debug.Draw(spriteBatch, "grav: " + Gravity.ToString()  + " I : " + (!jumping || positionPerso.Y == sol).ToString(), new Vector2(300, 50), Color.White, "normal");
            */
            /*  spriteBatch.Draw(Textures.hitbox, hitBoxPerso, Color.White * 0.5f); // debug perso hitbox */
        }
    }
}

