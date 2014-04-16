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
    public delegate void UpdateScore(int points);   // call this to increment score
   
    class Perso
    {

        public Vector2 positionPerso, tempCurrentFrame;
        //  Vector2 velocity;
        //  float gravity = 0.1f;
        public Rectangle hitBoxPerso;
        public Rectangle ThrowProjectiles;

        float sol = 380;
        float minnewYpos;
        bool jumping;
        bool Adapt;
        float Gravity;
        bool movedown;
        float vel;
        float acc;

        public bool gameover = false;
        public static bool game_over;
        bool debugbool = false;

        float newYpos;
        protected Texture2D imagePerso { get; private set; }

        public List<Projectile> projIA = new List<Projectile>();
        public Vector2 Position { get; private set; }
        public Vector2 initPos { get; private set; }

        float speed;
        public float Speed { get; private set; }

        Animation animationPerso;
        List<Projectile> projs = new List<Projectile>();
        public List<Rectangle> objects = new List<Rectangle>();

        /* For the IA */
        bool goright = false;
        bool goleft = true;

        double nextprojec = 0;
        public CharacType typePerso;

        public Vector2 cameraPos = Vector2.Zero;


        public Perso(Vector2 pos, CharacType typePerso)
        {
            animationPerso = new Animation(positionPerso, new Vector2(8, 2));
            tempCurrentFrame = Vector2.Zero;
            positionPerso = pos;
            speed = 100f;
            jumping = false;

            this.typePerso = typePerso;
            this.initPos = pos;
            movedown = true;
            newYpos = 0;
            GravityInit();
            imagePerso = Textures.mario_texture;
            animationPerso.AnimationSprite = Textures.mario_texture;
            animationPerso.Position = positionPerso;
            hitBoxPerso = new Rectangle((int)(positionPerso.X - imagePerso.Width / 2), (int)(positionPerso.Y - imagePerso.Height / 2), imagePerso.Width, imagePerso.Height);
            ThrowProjectiles = new Rectangle((int)positionPerso.X, (int)positionPerso.Y, 150, 40);
        }

        private void GravityInit()
        {
            vel = 10f;          // Kind of like the hight of the jump
            acc = 150f;         // Duration of jump
            Gravity = 0.5f;     // Start falling with this speed
        }

        public void Update(GameTime gametime, KeyboardState keyboardState, KeyboardState oldkey, bool moveleft, bool moveright, List<Rectangle> blocksTop, List<Projectile> proj, List<Rectangle> objects)
        {
            this.objects = objects;
           
            /* INITIALISATION */
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            movedown = true;
            Adapt = false;

            /*PROJECTILE*/
            Vector2 directionNoix;
            Vector2 positionNoix;
            if (animationPerso.CurrentFrame.Y == 1)
            {
                positionNoix = new Vector2(positionPerso.X - 5, positionPerso.Y + 5);
                directionNoix = new Vector2(-1, 0);
            }
            else
            {
                positionNoix = new Vector2(positionPerso.X + 27, positionPerso.Y + 5);
                directionNoix = new Vector2(1, 0);
            }

            Projectile noix = new Projectile(drawable_type.Nut, positionNoix, positionNoix, 200, directionNoix);

            if (keyboardState.IsKeyDown(Keys.Space) && !oldkey.IsKeyDown(Keys.Space))
            {
                projs.Add(noix);
            }

            /* CHECK OBJECT COLLISION WITH PROJECTILES */
            for (int i = 0; i < projs.Count; i++)
            {
                bool ifitdoes = false;
                for (int j = objects.Count - 1; j >= 0; j--)
                {
                    if ((new Rectangle(objects[j].X, objects[j].Y, objects[j].Width, objects[j].Height)).Intersects(projs[i].hitbox))
                    {
                        ifitdoes = true;
                        objects.Remove(objects[j]);
                    }
                }
                if (ifitdoes)
                {
                    projs.Remove(projs[i]);
                }
            }

            /* Update list*/
            for (int i = 0; i < projs.Count; i++)
            {
                projs[i].Update(gametime);
                if (projs[i].Visible == false)
                    projs.Remove(projs[i]);
            }
           

            /* CHECK TOP COLLISION */
            foreach (Rectangle top in blocksTop)
            {
                if ((new Rectangle(top.X, top.Y, top.Width, top.Height)).Intersects(hitBoxPerso))
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
                            if ((new Rectangle(top.X, top.Y, top.Width, top.Height)).Intersects(new Rectangle(hitBoxPerso.X, hitBoxPerso.Y + (int)i + 28, 28, 1)))
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


            if (keyboardState.IsKeyDown(Keys.Right) && moveright && !keyboardState.IsKeyDown(Keys.Left) && (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt)))
            {
                tempCurrentFrame.Y = 0;
                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                positionPerso.X += changement + 10;
                if (positionPerso.X > 400)
                    cameraPos = new Vector2(cameraPos.X - changement - 10, cameraPos.Y);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && moveleft && !keyboardState.IsKeyDown(Keys.Right) && (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt)))
            {
                tempCurrentFrame.Y = 1;

                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                positionPerso.X -= changement + 10;
                if (positionPerso.X > 400)
                    cameraPos = new Vector2(cameraPos.X + changement + 10, cameraPos.Y);
            }

            if (keyboardState.IsKeyDown(Keys.Right) && moveright && !keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.LeftAlt) && keyboardState.IsKeyUp(Keys.RightAlt))
            {
                tempCurrentFrame.Y = 0;
                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                    positionPerso.X += changement;
                    if (positionPerso.X > 400)
                        cameraPos = new Vector2(cameraPos.X - changement, cameraPos.Y);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && moveleft && !keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.LeftAlt) && keyboardState.IsKeyUp(Keys.RightAlt))
            {
                tempCurrentFrame.Y = 1;

                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                positionPerso.X -= changement;
                    if (positionPerso.X > 400)
                        cameraPos = new Vector2(cameraPos.X + changement, cameraPos.Y);
            }


            else
                animationPerso.Actif = false;

           
            tempCurrentFrame.X = animationPerso.CurrentFrame.X;
            animationPerso.Position = positionPerso;
            animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
            hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), 27, 28);
        }

        public List<Perso> CollisionIAProjec(List<Perso> checkIA)
        {
            for (int i = 0; i < checkIA.Count; i++)
            {
                for (int j = 0; j < projs.Count; j++)
                {
                    if (projs[j].hitbox.Intersects(checkIA[i].hitBoxPerso))
                    {
                        checkIA.Remove(checkIA[i]);
                        break;
                    }
                }
            }


            return checkIA;
        }


        public void UpdateIA(GameTime gametime, bool moveleft, bool moveright, List<Rectangle> blocksTop, Rectangle hitboxPlayer, ref int Health)
        {

            
            /* INITIALISATION */
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            movedown = true;
            Adapt = false;

            Vector2 directionNoix;
            Vector2 positionNoix;
            if (animationPerso.CurrentFrame.Y == 1)
            {
                positionNoix = new Vector2(positionPerso.X - 5, positionPerso.Y + 5);
                directionNoix = new Vector2(-1, 0);
            }
            else
            {
                positionNoix = new Vector2(positionPerso.X + 27, positionPerso.Y + 5);
                directionNoix = new Vector2(1, 0);
            }

            Projectile noix = new Projectile(drawable_type.Nut, positionNoix, positionNoix, 230, directionNoix);

            debugbool = ThrowProjectiles.Intersects(hitboxPlayer);

            if (gametime.TotalGameTime.TotalMilliseconds >= nextprojec && ThrowProjectiles.Intersects(hitboxPlayer))
            {
                projIA.Add(noix);
                nextprojec = gametime.TotalGameTime.TotalMilliseconds + 1000;
            }




          

            /* CHECK PERSO COLLISION WITH PROJECTILES */
            for (int i = 0; i < projIA.Count; i++)
            {
                if (projIA[i].hitbox.Intersects(hitboxPlayer))
                {
                    Health -= 3;
                    Health = (int)MathHelper.Clamp(Health, 0, 10);
                    projIA.Remove(projIA[i]);
                    if (Health == 0)
                    {
                        gameover = true;
                        game_over = true;
                    }
                }

            }

            /* Update list*/
            for (int i = 0; i < projIA.Count; i++)
            {
                projIA[i].Update(gametime);
                if (projIA[i].Visible == false)
                    projIA.Remove(projIA[i]);
            }
           

            /* CHECK TOP COLLISION */
            foreach (Rectangle top in blocksTop)
            {
                if ((new Rectangle(top.X, top.Y, top.Width, top.Height)).Intersects(hitBoxPerso))
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
                            if ((new Rectangle(top.X, top.Y, top.Width, top.Height)).Intersects(new Rectangle(hitBoxPerso.X, hitBoxPerso.Y + (int)i + 28, 28, 1)))
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




            if (positionPerso.X > initPos.X)
            {
                goleft = true;
                goright = false;
            }
            else if (positionPerso.X < initPos.X - 190)
            {
                goleft = false;
                goright = true;
            }

            if (!moveright)
            {
                initPos = new Vector2(initPos.X - 8, initPos.Y);
                goleft = true;
                goright = false;
            }

            if (!moveleft)
            {
                initPos = new Vector2(initPos.X + 8, initPos.Y);
                goleft = false;
                goright = true;
            }


            if (moveright && goright)
            {
               
                tempCurrentFrame.Y = 0;
                ThrowProjectiles = new Rectangle((int)positionPerso.X, (int)positionPerso.Y, 150, 40);
                    positionPerso.X += speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else if (moveleft && goleft)
            {
                tempCurrentFrame.Y = 1;
                ThrowProjectiles = new Rectangle((int)positionPerso.X - 150, (int)positionPerso.Y, 150, 40);
                    positionPerso.X -= speed * (float)gametime.ElapsedGameTime.TotalSeconds;
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
            if (typePerso == CharacType.player)
            {
                foreach (Projectile nut in projs)
                    nut.Draw(spriteBatch);
            }
            else
            {
                foreach (Projectile nut in projIA)
                    nut.Draw(spriteBatch);
            }

    
      /*          Drawable debug = new Drawable(drawable_type.font);
                if(typePerso == CharacType.player)
                    debug.Draw(spriteBatch, "h : " + Health, new Vector2(300, 50), Color.White, "normal");


            */
        }

     
    }
}

