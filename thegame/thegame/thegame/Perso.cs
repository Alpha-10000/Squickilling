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
using System.Threading;

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

        float sol = 330;
        float minnewYpos;
        bool jumping;
        bool Adapt;
        float Gravity;
        bool movedown;
        float vel;
        float acc;

        public bool gameover = false;
        public static bool game_over;

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



        private Rectangle toGetNeighborsTiles;

        private List<Rectangle> neihborsTiles = new List<Rectangle>();

        public bool activateDevelopper = false;



        public Perso(Vector2 pos, CharacType typePerso)
        {
            animationPerso = new Animation(positionPerso, new Vector2(8, 2));
            tempCurrentFrame = Vector2.Zero;
            positionPerso = pos;
            speed = 100f;
            jumping = false;

            movedown = true;

            this.typePerso = typePerso;
            this.initPos = pos;
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

        public void Update(GameTime gametime, KeyboardState keyboardState, KeyboardState oldkey, bool moveleft, bool moveright, List<Rectangle> blocksTop , List<Rectangle> blocksBottom, List<Rectangle> blocksLeft, List<Rectangle> blocksRight, List<Rectangle> blocks,  List<Projectile> proj, List<Rectangle> objects, ref int nb_nuts, bool activateDeveloper)
        {
            activateDevelopper = activateDeveloper;
            this.objects = objects;
           
            /* INITIALISATION */
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            Adapt = false;


            //In developper mode by pressing at the same time the keys T,E,A,M it is the white part.
            toGetNeighborsTiles = new Rectangle(hitBoxPerso.X - 20, hitBoxPerso.Y - 20, hitBoxPerso.Width + 40, hitBoxPerso.Height + 40);


            moveleft = true;
            moveright = true;

            
            /* Keep perso inside the map */
            if (hitBoxPerso.X <= 0)
                moveleft = false;

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
                projs.Add(noix);

            /* CHECK OBJECT COLLISION WITH HITBOX PERSO. ADD NUTS THEN */
                for (int j = objects.Count - 1; j >= 0; j--)
                {
                    if (hitBoxPerso.Intersects(objects[j]))
                    {
                        objects.Remove(objects[j]);
                        nb_nuts++;
                    }
                }

            /* Update list*/
            for (int i = 0; i < projs.Count; i++)
            {
                projs[i].Update(gametime);
                if (projs[i].Visible == false)
                    projs.Remove(projs[i]);
            }

            neihborsTiles = new List<Rectangle>();

            //get nearest tiles. In developper mode by pressing at the same time the letter T,E,A,M it is the block in green.
            foreach (Rectangle blocky in blocks)
                if (blocky.Intersects(toGetNeighborsTiles))
                    neihborsTiles.Add(blocky);


            /* THE PERSO IS JUMPING - PART FROM BOTTOM TO TOP */
            if (jumping)
            {
                if (Gravity > 0)
                {
                    float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                    vel -= acc * dt;// v = u + a*t
                    Gravity += vel * dt;// s = u*t + 0.5*a*t*t,
                    bool check = CheckCollisionTooFar(ref Gravity, blocks, "top");
                    if (check)
                    {
                        jumping = false;
                        movedown = true;
                        GravityInit();
                    }
                    positionPerso.Y -= Gravity;
                }
                else
                {
                    movedown = true;
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
            }



            

            /* GRAVITY - PERSO NOT JUMPING AND ON GROUND */
            if ( !jumping && positionPerso.Y + 1 < sol)
            {
                float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                vel += acc * dt;// v = u + a*t
                Gravity += vel * dt;// s = u*t + 0.5*a*t*t,
                bool check = CheckCollisionTooFar(ref Gravity, blocks, "bottom");
                if (check)
                    movedown = false;
                else
                    movedown = true;
                positionPerso.Y += Gravity; /* I putthree for a reason! Generates beug otherwise */
            }

            /* PERSO JUST TOUCHED THE GROUND SO INITIALIZE VALUE */
            if (!jumping && (positionPerso.Y == sol || !movedown))
                GravityInit();

            if (keyboardState.IsKeyDown(Keys.Right) && moveright && !keyboardState.IsKeyDown(Keys.Left) && (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt)))
            {
                tempCurrentFrame.Y = 0;
                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds + 10;
                bool check = CheckCollisionTooFar(ref changement, blocks, "right");// this is to check right collision
                if (check)
                    moveright = false;
                positionPerso.X += changement;
                if (positionPerso.X > 400 && positionPerso.X < 5000)
                    cameraPos = new Vector2(cameraPos.X - changement, cameraPos.Y);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && moveleft && !keyboardState.IsKeyDown(Keys.Right) && (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt)))
            {
                tempCurrentFrame.Y = 1;

                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds + 10;
                bool check = CheckCollisionTooFar(ref changement, blocks, "left");//this is to check left collsion
                if (check)
                    moveleft = false;
                positionPerso.X -= changement;
                if (positionPerso.X > 400 && positionPerso.X < 5000)
                    cameraPos = new Vector2(cameraPos.X + changement, cameraPos.Y);
            }

            if (keyboardState.IsKeyDown(Keys.Right) && moveright && !keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.LeftAlt) && keyboardState.IsKeyUp(Keys.RightAlt))
            {
                tempCurrentFrame.Y = 0;
                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                bool check = CheckCollisionTooFar(ref changement, blocks, "right");//this is to check right collision
                if (check)
                    moveright = false;
                positionPerso.X += changement;
                if (positionPerso.X > 400 && positionPerso.X < 5000)
                    cameraPos = new Vector2(cameraPos.X - changement, cameraPos.Y);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && moveleft && !keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.LeftAlt) && keyboardState.IsKeyUp(Keys.RightAlt))
            {
                tempCurrentFrame.Y = 1;

                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                bool check = CheckCollisionTooFar(ref changement, blocks, "left");//this is to check left collision
                if (check)
                    moveleft = false;
                positionPerso.X -= changement;
                if (positionPerso.X > 400 && positionPerso.X < 5000)
                    cameraPos = new Vector2(cameraPos.X + changement, cameraPos.Y);
            }


            else
                animationPerso.Actif = false;
            

            tempCurrentFrame.X = animationPerso.CurrentFrame.X;
            animationPerso.Position = positionPerso;
            animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
            hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), 27, 24);

        }

        /* VERY IMPORTANT FUNCTION. CHECK IF COLLISION EXIST AND HANDLE IT! */
        public bool CheckCollisionTooFar(ref float vitesse, List<Rectangle> blocks, string direction)
        {
               bool check = false;

               if (direction == "right")//check right collision
               {
                   for (int i = 0; i < vitesse; i++)
                       foreach (Rectangle block in blocks)
                           if (block.Intersects(new Rectangle(hitBoxPerso.X + hitBoxPerso.Width + i, hitBoxPerso.Y, 1, hitBoxPerso.Height)) && hitBoxPerso.X + hitBoxPerso.Width <= block.X)
                           {
                               check = true;
                               vitesse = block.X - 1 - positionPerso.X - hitBoxPerso.Width;
                               break;
                           }
               }
               else if (direction == "left")//check bottom collision
               {
                   for (int i = 0; i < vitesse; i++)
                       foreach (Rectangle block in blocks)
                           if (block.Intersects(new Rectangle(hitBoxPerso.X - i, hitBoxPerso.Y, 1, hitBoxPerso.Height)) && hitBoxPerso.X >= block.X + block.Width)
                           {
                               check = true;
                               vitesse = hitBoxPerso.X -  block.X - block.Width ;
                               break;
                           }
               }
               else if (direction == "top")//check bottom collision
               {
                   for (int i = 0; i < vitesse; i++)
                       foreach (Rectangle block in blocks)
                           if (block.Intersects(new Rectangle(hitBoxPerso.X, hitBoxPerso.Y + i, hitBoxPerso.Width, 1)))
                           {
                               check = true;
                               vitesse = hitBoxPerso.Y - block.Y + 1;
                               break;
                           }
               }
               else if (direction == "bottom")//check bottom collision
               {
                   for (int i = 0; i < vitesse; i++)
                       foreach (Rectangle block in blocks)
                           if (block.Intersects(new Rectangle(hitBoxPerso.X, hitBoxPerso.Y + hitBoxPerso.Height + i, hitBoxPerso.Width, 1)) && hitBoxPerso.Y + hitBoxPerso.Height <= block.Y)
                           {
                               check = true;
                               vitesse = block.Y -  positionPerso.Y  - hitBoxPerso.Height;
                               break;
                           }
               }


               return check;
        }

        public List<Perso> CollisionIAProjec(List<Perso> checkIA, ref int score)
        {
            for (int i = 0; i < checkIA.Count; i++)
            {
                for (int j = 0; j < projs.Count; j++)
                {
                    if (projs[j].hitbox.Intersects(checkIA[i].hitBoxPerso))
                    {
                        checkIA.Remove(checkIA[i]);
                        score++;
                        break;
                    }
                }
            }


            return checkIA;
        }

        public int TryToKill(ref int Health, Rectangle hitboxPlayer)
        {
            /* CHECK PERSO COLLISION WITH PROJECTILES */
            int check = 0;
            for (int i = 0; i < projIA.Count; i++)
            {
                if (projIA[i].hitbox.Intersects(hitboxPlayer))
                {
                    Health -= 3;
                    Textures.gamePunch_Effect.Play();
                    Health = (int)MathHelper.Clamp(Health, 0, 10);
                    projIA.Remove(projIA[i]);
                    if (Health == 0)
                    {
                        gameover = true;
                        game_over = true;
                    }
                    check++;
                }

            }

            return check; // execute blood screen
        }


        public void UpdateIA(GameTime gametime, bool moveleft, bool moveright, List<Rectangle> blocksTop, Rectangle hitboxPlayer)
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


            if (gametime.TotalGameTime.TotalMilliseconds >= nextprojec && ThrowProjectiles.Intersects(hitboxPlayer))
            {
                projIA.Add(noix);
                nextprojec = gametime.TotalGameTime.TotalMilliseconds + 1000;
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
            hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), 27, 26);

        }
        public void Draw(SpriteBatch spriteBatch)
        {

            animationPerso.Draw(spriteBatch);
            if (typePerso == CharacType.player)
                foreach (Projectile nut in projs)
                    nut.Draw(spriteBatch);
            else
                foreach (Projectile nut in projIA)
                    nut.Draw(spriteBatch);

    

            
            if (typePerso == CharacType.player && activateDevelopper)
            {

          /*      Drawable debug = new Drawable(drawable_type.font);
                if (typePerso == CharacType.player)
                {
                    debug.Draw(spriteBatch, "h : " + debug1 + " " + debug2, new Vector2(300, 50), Color.White, "normal");
                    debug.Draw(spriteBatch, "h : " + debug3 + " " + debug4, new Vector2(300, 80), Color.White, "normal");
                }
                */
                spriteBatch.Draw(Textures.hitbox, hitBoxPerso, Color.Red *0.5f);

                spriteBatch.Draw(Textures.hitbox, toGetNeighborsTiles, Color.White * 0.5f);

               

                foreach (Rectangle top in neihborsTiles)
                    spriteBatch.Draw(Textures.hitbox, top, Color.Green * 0.4f);
            }
        }

     
    }
}

