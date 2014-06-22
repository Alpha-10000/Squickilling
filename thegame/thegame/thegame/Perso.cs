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
using Lidgren.Network;

namespace thegame
{
    public delegate void UpdateScore(int points);   // call this to increment score

    class Perso
    {
        public int score;
        public int nbNuts;
        public int health;
        public Vector2 positionPerso, tempCurrentFrame;
        //  Vector2 velocity;
        //  float gravity = 0.1f;
        public Rectangle hitBoxPerso;
        public Rectangle ThrowProjectiles;

        float sol = 330;
        float Gravity;
        bool movedown;
        float vel;
        float acc;

        public bool gameover = false;
        public static bool game_over;
        public bool PersoHitted = false;
        private float timeElaspedHitted;
        public int compteurHitted = 0;// public because it needs to know if the perso has been hitted by explosion. So should continue... ask me if questions.

        protected Texture2D imagePerso { get; private set; }

        public List<Projectile> projIA = new List<Projectile>();
        public Vector2 Position { get; private set; }
        public Vector2 initPos { get; private set; }

        float speed;
        public float Speed { get; private set; }

        public Animation animationPerso;
        List<Projectile> projs = new List<Projectile>();
        public List<Rectangle> objects = new List<Rectangle>();
        /* For the IA */

        double nextprojec = 0;
        public CharacType typePerso;

        public Vector2 cameraPos = Vector2.Zero;

        private bool moveleft;
        private bool moveright;

        private Rectangle toGetNeighborsTiles;

        private List<Rectangle> neihborsTiles = new List<Rectangle>();

        public bool activateDevelopper = false;

        private bool jumping;


        public bool utilisable = false;

        public NetConnection Connection { get; set; }

        public Perso(Vector2 pos, CharacType typePerso)
        {
            if (typePerso == CharacType.player)
            {
                animationPerso = new Animation(positionPerso, new Vector2(8, 2));
                imagePerso = Textures.mario_texture;
                animationPerso.AnimationSprite = Textures.mario_texture;
                hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), imagePerso.Width / 4, imagePerso.Height / 2 - 4);
            }
            else if (typePerso == CharacType.ia)
            {
                animationPerso = new Animation(positionPerso, new Vector2(8, 2));
                imagePerso = Textures.ennemy_texture;
                animationPerso.AnimationSprite = Textures.ennemy_texture;
                hitBoxPerso = new Rectangle((int)(positionPerso.X + 30), (int)(positionPerso.Y), imagePerso.Width / 8, imagePerso.Height / 2 - 4);
            }
            tempCurrentFrame = Vector2.Zero;
            positionPerso = pos;
            speed = 120f;

            movedown = true;
            jumping = false;
            this.typePerso = typePerso;
            this.initPos = pos;
            GravityInit();
            animationPerso.Position = positionPerso;
            ThrowProjectiles = new Rectangle((int)positionPerso.X, (int)positionPerso.Y, 150, 40);

            moveleft = true;
            moveright = true;
            health = 20;
            score = 0;
            nbNuts = 0;
        }

        public Perso(Vector2 pos, CharacType typePerso, NetConnection conn)
        {

            if (typePerso == CharacType.player)
            {
                animationPerso = new Animation(positionPerso, new Vector2(4, 2));
                imagePerso = Textures.mario_texture;
                animationPerso.AnimationSprite = Textures.mario_texture;
                hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), imagePerso.Width / 4, imagePerso.Height / 2 - 8);
            }
            else if (typePerso == CharacType.ia)
            {
                animationPerso = new Animation(positionPerso, new Vector2(8, 2));
                imagePerso = Textures.ennemy_texture;
                animationPerso.AnimationSprite = Textures.ennemy_texture;
                hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), imagePerso.Width / 8, imagePerso.Height / 2 - 8);
            }
            tempCurrentFrame = Vector2.Zero;
            positionPerso = pos;
            speed = 120f;

            movedown = true;
            jumping = false;
            this.typePerso = typePerso;
            this.initPos = pos;
            GravityInit();
            imagePerso = Textures.mario_texture;
            ThrowProjectiles = new Rectangle((int)positionPerso.X, (int)positionPerso.Y, 150, 40);

            moveleft = true;
            moveright = true;
            Connection = conn;
            health = 20;
            score = 0;
            nbNuts = 0;
        }
        public Perso()
        {
        }

        private void GravityInit()
        {
            vel = 10f;          // Kind of like the hight of the jump
            acc = 150f;         // Duration of jump
            Gravity = 0.5f;     // Start falling with this speed
        }

        public void Update(GameTime gametime, List<Rectangle> blocks, List<Projectile> proj, List<Rectangle> objects, ref int nbNuts, bool activateDeveloper)
        {
            this.activateDevelopper = activateDeveloper;
            this.objects = objects;
            /* INITIALISATION */
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;


            //In developper mode by pressing at the same time the keys T,E,A,M it is the white part.
            toGetNeighborsTiles = new Rectangle(hitBoxPerso.X - 40, hitBoxPerso.Y - 40, hitBoxPerso.Width + 80, hitBoxPerso.Height + 80);


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
                positionNoix = new Vector2(positionPerso.X - 3, positionPerso.Y + 7);
                directionNoix = new Vector2(-1, 0);
            }
            else
            {
                positionNoix = new Vector2(positionPerso.X + 20, positionPerso.Y + 7);
                directionNoix = new Vector2(1, 0);
            }

            Projectile noix = new Projectile(drawable_type.Nut, positionNoix, positionNoix, 200, directionNoix);

            if (Inputs.isKeyRelease(Keys.Space))
                if (utilisable)
                    projs.Add(noix);

            /* CHECK OBJECT COLLISION WITH HITBOX PERSO. ADD NUTS THEN */
            for (int j = objects.Count - 1; j >= 0; j--)
                if (hitBoxPerso.Intersects(objects[j]))
                {
                    objects.Remove(objects[j]);
                    nbNuts++;
                    break;
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
                if (Inputs.isKeyRelease(Keys.Up) && (!movedown || positionPerso.Y == sol))
                {
                    if (utilisable)
                    {
                        jumping = true;
                        Gravity = 5f; /* First jump speed while pressing the button*/
                    }
                }

                /* KEEP PERSO ON GROUND */
                if (movedown && positionPerso.Y + Gravity > sol)
                    positionPerso.Y = sol;
            }





            /* GRAVITY - PERSO NOT JUMPING AND ON GROUND */
            if (!jumping && positionPerso.Y + 1 < sol)
            {
                float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                vel += acc * dt;// v = u + a*t
                Gravity += vel * dt;// s = u*t + 0.5*a*t*t,
                bool check = CheckCollisionTooFar(ref Gravity, blocks, "bottom");
                movedown = check ? false : true;
                positionPerso.Y += Gravity;
            }

            hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), imagePerso.Width / 16, imagePerso.Height / 2 - 8);

            /* PERSO JUST TOUCHED THE GROUND SO INITIALIZE VALUE */
            if (!jumping && (positionPerso.Y == sol || !movedown))
                GravityInit();

            if (Inputs.isKeyDown(Keys.Right) && moveright && !Inputs.isKeyDown(Keys.Left) && (Inputs.isKeyDown(Keys.LeftAlt) || Inputs.isKeyDown(Keys.RightAlt)) && activateDevelopper)
            {
                if (utilisable)
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
            }
            else if (!Inputs.isKeyDown(Keys.Right) && moveleft && Inputs.isKeyDown(Keys.Left) && (Inputs.isKeyDown(Keys.LeftAlt) || Inputs.isKeyDown(Keys.RightAlt)) && activateDevelopper)
            {
                if (utilisable)
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
            }

            if (Inputs.isKeyDown(Keys.Right) && moveright && !Inputs.isKeyDown(Keys.Left) && ((activateDeveloper) ? (Inputs.isKeysUP(Keys.LeftAlt) && Inputs.isKeysUP(Keys.RightAlt)) : true))
            {
                if (utilisable)
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
            }
            else if (!Inputs.isKeyDown(Keys.Right) && moveleft && Inputs.isKeyDown(Keys.Left) && ((activateDeveloper) ? (Inputs.isKeysUP(Keys.LeftAlt) && Inputs.isKeysUP(Keys.RightAlt)) : true))
            {
                if (utilisable)
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
            }


            else
                animationPerso.Actif = false;

            if (utilisable)
                tempCurrentFrame.X = animationPerso.CurrentFrame.X;
            animationPerso.Position = positionPerso;
            animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
            if (typePerso == CharacType.player)
                hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), imagePerso.Width / 16, imagePerso.Height / 2 - 8);
            else if (typePerso == CharacType.ia)
                hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), imagePerso.Width / 8, imagePerso.Height - 2);

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
                            vitesse = hitBoxPerso.X - block.X - block.Width;
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
                            vitesse = block.Y - positionPerso.Y - hitBoxPerso.Height;
                            break;
                        }
            }


            return check;
        }

        public List<Perso> CollisionIAProjec(List<Perso> checkIA, ref int score)
        {
            for (int i = 0; i < checkIA.Count; i++)
                for (int j = 0; j < projs.Count; j++)
                    if (projs[j].hitbox.Intersects(checkIA[i].hitBoxPerso))
                    {
                        checkIA.Remove(checkIA[i]);
                        score++;
                        break;
                    }


            return checkIA;
        }

        public int TryToKill(ref int health, Rectangle hitboxPlayer, bool soundIs, bool Sounddeveloper)
        {
            /* CHECK PERSO COLLISION WITH PROJECTILES */
            int check = 0;
            for (int i = 0; i < projIA.Count; i++)
            {
                if (projIA[i].hitbox.Intersects(hitboxPlayer))
                {
                    health -= 3;
                    if (soundIs && !Sounddeveloper)
                        Textures.gamePunch_Effect.Play();
                    projIA.Remove(projIA[i]);
                    if (health == 0)
                    {
                        gameover = true;
                        game_over = true;
                    }
                    check++;
                }
            }

            return check; // execute blood screen
        }


        public void UpdateIA(GameTime gametime, List<Rectangle> blocks, Rectangle hitboxPlayer, bool activateDeveloper2)
        {

            sol = 303;
            this.activateDevelopper = activateDeveloper2;
            /* INITIALISATION */
            positionPerso = animationPerso.Position;
            animationPerso.Actif = true;
            movedown = true;

            Vector2 directionNoix;
            Vector2 positionNoix;
            if (animationPerso.CurrentFrame.Y == 1)
            {
                if (typePerso == CharacType.player)
                    positionNoix = new Vector2(positionPerso.X - 5, positionPerso.Y + 5);
                else
                    positionNoix = new Vector2(positionPerso.X - 30, positionPerso.Y + 20);
                directionNoix = new Vector2(-1, 0);
            }
            else
            {
                if (typePerso == CharacType.player)
                    positionNoix = new Vector2(positionPerso.X + 27, positionPerso.Y + 5);
                else
                    positionNoix = new Vector2(positionPerso.X - 10, positionPerso.Y + 20);
                directionNoix = new Vector2(1, 0);
            }
            Projectile noix;
            if (animationPerso.CurrentFrame.Y == 1)
                noix = new Projectile(drawable_type.axe2, positionNoix, positionNoix, 230, directionNoix);
            else
                noix = new Projectile(drawable_type.axe, positionNoix, positionNoix, 230, directionNoix);


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


            neihborsTiles = new List<Rectangle>();

            //get nearest tiles. In developper mode by pressing at the same time the letter T,E,A,M it is the block in green.
            foreach (Rectangle blocky in blocks)
                if (blocky.Intersects(toGetNeighborsTiles))
                    neihborsTiles.Add(blocky);

            /* KEEP PERSO ON GROUND */
            if (movedown && positionPerso.Y + Gravity > sol)
                positionPerso.Y = sol;

            /* PERSO JUST TOUCHED THE GROUND SO INITIALIZE VALUE */
            if (positionPerso.Y == sol || !movedown)
                GravityInit();

            /* GRAVITY */
            if (movedown && !jumping && positionPerso.Y + 1 < sol)
            {
                float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                vel += acc * dt;// v = u + a*t
                Gravity += vel * dt;// s = u*t + 0.5*a*t*t,
                bool check = CheckCollisionTooFar(ref Gravity, blocks, "bottom");
                movedown = check ? false : true;
                positionPerso.Y += Gravity; /* I putthree for a reason! Generates beug otherwise */
            }

            hitBoxPerso = new Rectangle((int)(positionPerso.X + 30) - imagePerso.Width / 12, (int)(positionPerso.Y), imagePerso.Width / 8, imagePerso.Height / 2 - 2);

            if (moveright)
            {
                tempCurrentFrame.Y = 0;
                ThrowProjectiles = new Rectangle((int)positionPerso.X, (int)positionPerso.Y, 150, 40);
                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                bool check = CheckCollisionTooFar(ref changement, blocks, "right");//this is to check right collision
                bool check2 = positionPerso.X > initPos.X;// this is to move the perso on the right if it hits something
                if (check || check2)
                {
                    moveleft = true;
                    moveright = false;
                    if (check)
                        initPos = new Vector2(initPos.X - 9, initPos.Y);
                }
                positionPerso.X += changement;
            }
            else if (moveleft)
            {
                tempCurrentFrame.Y = 1;
                ThrowProjectiles = new Rectangle((int)positionPerso.X - 150, (int)positionPerso.Y, 150, 40);
                float changement = speed * (float)gametime.ElapsedGameTime.TotalSeconds;
                bool check = CheckCollisionTooFar(ref changement, blocks, "left");//this is to check left collision
                bool check2 = positionPerso.X < initPos.X - 190;
                if (check || check2)
                {
                    moveright = true;
                    moveleft = false;
                    if (check)
                        initPos = new Vector2(initPos.X + 9, initPos.Y);
                }
                positionPerso.X -= changement;
            }


            else
                animationPerso.Actif = false;




            tempCurrentFrame.X = animationPerso.CurrentFrame.X;
            animationPerso.Position = positionPerso;
            animationPerso.CurrentFrame = tempCurrentFrame;
            animationPerso.Update(gametime);
            if (typePerso == CharacType.player)
                hitBoxPerso = new Rectangle((int)(positionPerso.X), (int)(positionPerso.Y), imagePerso.Width / 16, imagePerso.Height / 2 - 8);
            else if (typePerso == CharacType.ia)
                hitBoxPerso = new Rectangle((int)(positionPerso.X + 30) - imagePerso.Width / 12, (int)(positionPerso.Y), imagePerso.Width / 8, imagePerso.Height / 2 - 2);

        }
        float rot = 0;
        public void Draw(SpriteBatch spriteBatch, GameTime gametime)
        {
            /* Fait clignoter le personnage */
            if (PersoHitted)
            {
                timeElaspedHitted += gametime.ElapsedGameTime.Milliseconds;
                if (compteurHitted > 6)
                {
                    PersoHitted = false;
                    compteurHitted = 0;
                    timeElaspedHitted = 0;
                }
                else
                {
                    if (timeElaspedHitted > 200)
                    {
                        compteurHitted++;
                        timeElaspedHitted = 0;
                    }
                    if (compteurHitted % 2 == 1 && CharacType.player == typePerso)
                        animationPerso.Draw(spriteBatch, false);
                    else if (compteurHitted % 2 == 1)
                        animationPerso.Draw(spriteBatch);
                }

            }
            else if (CharacType.player == typePerso)
                animationPerso.Draw(spriteBatch, true);
            else
                animationPerso.Draw(spriteBatch);

            rot -= 0.30f;
            if (typePerso == CharacType.player)
                foreach (Projectile nut in projs)
                    nut.Draw(spriteBatch);
            else
                foreach (Projectile nut in projIA)
                {
                    nut.Draw(spriteBatch, nut.PositionProjectile, rot, true);
                    //nut.Draw(spriteBatch, new Vector2(nut.PositionProjectile.X, nut.PositionProjectile.Y + 15), rot, true);
                }




            if (activateDevelopper)
            {

                /*      Drawable debug = new Drawable(drawable_type.font);
                      if (typePerso == CharacType.player)
                      {
                          debug.Draw(spriteBatch, "h : " + debug1 + " " + debug2, new Vector2(300, 50), Color.White, "normal");
                          debug.Draw(spriteBatch, "h : " + debug3 + " " + debug4, new Vector2(300, 80), Color.White, "normal");
                      }
                      */
                spriteBatch.Draw(Textures.hitbox, hitBoxPerso, Color.Red * 0.5f);

                spriteBatch.Draw(Textures.hitbox, toGetNeighborsTiles, Color.White * 0.5f);



                foreach (Rectangle top in neihborsTiles)
                    spriteBatch.Draw(Textures.hitbox, top, Color.Green * 0.4f);
            }
        }


    }
}

