using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace YelloKiller.Moteur_Particule
{
    class MoteurParticule
    {
        #region Fields

        public static Vector2 Camera
        { get; set; }

        YellokillerGame game;
        SpriteBatch spriteBatch;

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        int direction_heros2_appele;
        int direction_heros1_appele;

        ExplosionParticleSystem hadoken_heros1;
        ExplosionParticleSystem hadoken_heros2;
        ExplosionSmokeParticleSystem fume_hadoken;
        Statue_Explosion explosion_statue;
        SmokePlumeParticleSystem fume;
        BallParticleSystem ball1;
        BallParticleSystem ball2;
        Fumigene fumigene1;
        Fumigene fumigene2;

        const float TimeBetweenExplosions = 2.0f;
        const float TimeBetweenSmokePlumePuffs = .5f;

        #endregion

        #region Rectangle tout moche Hero 1

        bool rectangle_hadoken_est_present_heros1 = true; // utiliser pour effacer le rectangle apres collision 
        public bool Rectangle_Hadoken_Est_Present_Hero1
        {
            get { return rectangle_hadoken_est_present_heros1; }
            set { rectangle_hadoken_est_present_heros1 = value; }
        }

        public Rectangle Rectangle_Hadoken_heros1(Heros heros) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero1 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero1 = false;
                GameplayScreen.Timer_Hero1 = 0;
            }

            if (hadoken_heros1.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Hadoken_Est_Present_Hero1 = true;// je reinitialise donc mon rectangle
                direction_heros1_appele = heros.SourceRectangle.Value.Y;
            }// direction du heros au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon heros.


            if (hadoken_heros1.FreeParticleCount < 100 && Rectangle_Hadoken_Est_Present_Hero1 && GameplayScreen.Timer_Hero1 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_heros1_appele == 133) // haut
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y - (hadoken_heros1.LongueurHadoken * 28), 28, (hadoken_heros1.LongueurHadoken * 28));

                else if (direction_heros1_appele == 198) // bas
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, 28, (hadoken_heros1.LongueurHadoken * 28));

                else if (direction_heros1_appele == 230) // gauche
                    return new Rectangle((int)heros.position.X - (hadoken_heros1.LongueurHadoken * 28), (int)heros.position.Y, (hadoken_heros1.LongueurHadoken * 28), 28);

                else // droite
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, (hadoken_heros1.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        bool rectangle_ball_est_present_heros1 = true;
        public bool Rectangle_Ball_Est_Present_Hero1
        {
            get { return rectangle_ball_est_present_heros1; }
            set { rectangle_ball_est_present_heros1 = value; }
        }

        public Rectangle Rectangle_Ball_heros1(Heros heros) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero1 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero1 = false;
                GameplayScreen.Timer_Hero1 = 0;
            }

            if (ball1.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Ball_Est_Present_Hero1 = true;// je reinitialise donc mon rectangle
                direction_heros1_appele = heros.SourceRectangle.Value.Y;
            }// direction du heros au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon heros.


            if (ball1.FreeParticleCount < 100 && Rectangle_Ball_Est_Present_Hero1 && GameplayScreen.Timer_Hero1 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_heros1_appele == 133) // haut
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y - (ball1.LongueurBall * 28), 28, (ball1.LongueurBall * 28));

                else if (direction_heros1_appele == 198) // bas
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, 28, (ball1.LongueurBall * 28));

                else if (direction_heros1_appele == 230) // gauche
                    return new Rectangle((int)heros.position.X - (ball1.LongueurBall * 28), (int)heros.position.Y, (ball1.LongueurBall * 28), 28);

                else  // droite
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, (ball1.LongueurBall * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Rectangle tout moche Hero 2

        bool rectangle_hadoken_est_present_heros2 = true;
        public bool Rectangle_Hadoken_Est_Present_Hero2
        {
            get { return rectangle_hadoken_est_present_heros2; }
            set { rectangle_hadoken_est_present_heros2 = value; }
        }

        public Rectangle Rectangle_Hadoken_heros2(Heros heros) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero2 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero2 = false;
                GameplayScreen.Timer_Hero2 = 0;
            }

            if (hadoken_heros2.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Hadoken_Est_Present_Hero2 = true;// je reinitialise donc mon rectangle
                direction_heros2_appele = heros.SourceRectangle.Value.Y;
            }// direction du heros au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon heros.


            if (hadoken_heros2.FreeParticleCount < 100 && Rectangle_Hadoken_Est_Present_Hero2 && GameplayScreen.Timer_Hero2 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_heros2_appele == 133) // haut
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y - (hadoken_heros2.LongueurHadoken * 28), 28, (hadoken_heros2.LongueurHadoken * 28));

                else if (direction_heros2_appele == 198) // bas
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, 28, (hadoken_heros2.LongueurHadoken * 28));

                else if (direction_heros2_appele == 230) // gauche
                    return new Rectangle((int)heros.position.X - (hadoken_heros2.LongueurHadoken * 28), (int)heros.position.Y, (hadoken_heros2.LongueurHadoken * 28), 28);

                else // droite
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, (hadoken_heros2.LongueurHadoken * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        bool rectangle_ball_est_present_heros2 = true;
        public bool Rectangle_Ball_Est_Present_Hero2
        {
            get { return rectangle_ball_est_present_heros2; }
            set { rectangle_ball_est_present_heros2 = value; }
        }

        public Rectangle Rectangle_Ball_heros2(Heros heros) // pour gerer les collisions
        {
            if (GameplayScreen.Timer_Hero2 > 1) // apres une seconde mon timer se remet a zero
            {
                GameplayScreen.Enable_Timer_Hero2 = false;
                GameplayScreen.Timer_Hero2 = 0;
            }

            if (ball2.FreeParticleCount == 100) // lorsque freeparticulecount = 100 le hadoken est termine 
            {
                Rectangle_Ball_Est_Present_Hero2 = true;// je reinitialise donc mon rectangle
                direction_heros2_appele = heros.SourceRectangle.Value.Y;
            }// direction du heros au moment de l'appel pour pas quelle change durant le meme appel si je tourne mon heros.


            if (ball2.FreeParticleCount < 100 && Rectangle_Ball_Est_Present_Hero2 && GameplayScreen.Timer_Hero2 > 0.5)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (direction_heros2_appele == 133) // haut
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y - (ball2.LongueurBall * 28), 28, (ball2.LongueurBall * 28));

                else if (direction_heros2_appele == 198) // bas
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, 28, (ball2.LongueurBall * 28));

                else if (direction_heros2_appele == 230) // gauche
                    return new Rectangle((int)heros.position.X - (ball2.LongueurBall * 28), (int)heros.position.Y, (ball2.LongueurBall * 28), 28);

                else // droite 
                    return new Rectangle((int)heros.position.X, (int)heros.position.Y, (ball2.LongueurBall * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Rectangle tout moche Statue

        bool rectangle_hadoken_est_present_statue = true; // utiliser pour effacer le rectangle apres collision 
        public bool Rectangle_Hadoken_Est_Present_Statue
        {
            get { return rectangle_hadoken_est_present_statue; }
            set { rectangle_hadoken_est_present_statue = value; }
        }

        public Rectangle Rectangle_Hadoken_Statue(Statue statue) // pour gerer les collisions
        {
            if (explosion_statue.FreeParticleCount == 2000) // lorsque freeparticulecount = 2000 le hadoken est termine 
                Rectangle_Hadoken_Est_Present_Statue = true;// je reinitialise donc mon rectangle

            if (explosion_statue.FreeParticleCount < 2000 && Rectangle_Hadoken_Est_Present_Statue)
            { // j'attend une demi seconde avant de créer le rectangle pour geré la collision
                if (statue.SourceRectangle.Value.Y == 357) // haut
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2), (int)statue.position.Y - (explosion_statue.LongueurExplosion * 28) + (statue.SourceRectangle.Value.Height / 2), 28, (explosion_statue.LongueurExplosion * 28));

                else if (statue.SourceRectangle.Value.Y == 0) // bas
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2), (int)statue.position.Y + (statue.SourceRectangle.Value.Height / 2), 28, (explosion_statue.LongueurExplosion * 28));

                else if (statue.SourceRectangle.Value.Y == 123) // gauche
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2) - (explosion_statue.LongueurExplosion * 28), (int)statue.position.Y + (statue.SourceRectangle.Value.Height / 2), (explosion_statue.LongueurExplosion * 28), 28);

                else // droite
                    return new Rectangle((int)statue.position.X + (statue.SourceRectangle.Value.Width / 2), (int)statue.position.Y + (statue.SourceRectangle.Value.Height / 2), (explosion_statue.LongueurExplosion * 28), 28);
            }
            else // pas de rectangle
                return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Rectangle tout moche Fumigene

        public Rectangle Rectangle_Fumigene_Heros1(Heros heros) // pour gerer les collisions
        {
            if (fumigene1.FreeParticleCount < 135 )
                return new Rectangle((int)heros.position.X - 28 * 3, (int)heros.position.Y - 28 * 3, 28 * 6, 28 * 6);
            else // pas de rectangle
                return Rectangle.Empty;
        }

        public Rectangle Rectangle_Fumigene_Heros2(Heros heros) // pour gerer les collisions
        {
            if (heros != null && fumigene2.FreeParticleCount < 135)
                return new Rectangle((int)heros.position.X - 28 * 3, (int)heros.position.Y - 28 * 3, 28 * 6, 28 * 6);
            else
                return Rectangle.Empty;
        }

        #endregion



        #region Initialization

        public MoteurParticule(YellokillerGame game, SpriteBatch spriteBatch, Carte carte, Heros heros1, Heros heros2, List<Statue> _statue)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            hadoken_heros1 = new ExplosionParticleSystem(game, 1, heros1, carte, 50);
            game.Components.Add(hadoken_heros1);

            ball1 = new BallParticleSystem(game, 1, heros1, carte);
            game.Components.Add(ball1);

            foreach (Statue statue in _statue)
            {
                explosion_statue = new Statue_Explosion(game, 20, carte, statue, statue.Distance_Statue_Mur(carte));
                game.Components.Add(explosion_statue);
            }

            fume_hadoken = new ExplosionSmokeParticleSystem(game, 2);
            game.Components.Add(fume_hadoken);

            fumigene1 = new Fumigene(game, 9, heros1, carte);
            game.Components.Add(fumigene1);

            fume = new SmokePlumeParticleSystem(game, 9);
            game.Components.Add(fume);

            try
            {
                hadoken_heros2 = new ExplosionParticleSystem(game, 1, heros2, carte, 50);
                game.Components.Add(hadoken_heros2);
                ball2 = new BallParticleSystem(game, 1, heros2, carte);
                fumigene2 = new Fumigene(game, 9, heros2, carte);
                game.Components.Add(fumigene2);
                game.Components.Add(ball2);
            }
            catch (NullReferenceException)
            { }
        }

        #endregion

        public void UpdateFumigene(Heros heros)
        {           
            if (heros.NumeroHero == 1)
                fumigene1.AddParticles(new Vector2(heros.position.X - Camera.X, heros.position.Y - Camera.Y), heros);
            else
                fumigene2.AddParticles(new Vector2(heros.position.X - Camera.X, heros.position.Y - Camera.Y), heros);
        }

        public void UpdateExplosions_heros(Heros heros)
        {
            if (heros.NumeroHero == 1)
                hadoken_heros1.AddParticles(new Vector2(heros.position.X - Camera.X, heros.position.Y - Camera.Y), heros);
            else
                hadoken_heros2.AddParticles(new Vector2(heros.position.X - Camera.X, heros.position.Y - Camera.Y), heros);

            fume_hadoken.AddParticles(new Vector2(heros.position.X - Camera.X, heros.position.Y - Camera.Y), heros);
        }

        public void UpdateExplosions_statue(Statue statue)
        {
            explosion_statue.AddParticles(new Vector2(statue.position.X - Camera.X + (statue.SourceRectangle.Value.Width / 2), statue.position.Y - Camera.Y + (statue.SourceRectangle.Value.Height / 2)), statue);
        }

        public void UpdateBall(Heros heros)
        {
            if (heros.NumeroHero == 1)
                ball1.AddParticles(new Vector2(heros.position.X - Camera.X, heros.position.Y - Camera.Y), heros);
            else
                ball2.AddParticles(new Vector2(heros.position.X - Camera.X, heros.position.Y - Camera.Y), heros);
        }

        public static float RandomBetween(float min, float max) // random utiliser pour toutes les proprietes 
        {                                                       // des particules
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}

