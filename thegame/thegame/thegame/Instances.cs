using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Data.SqlClient;
using System.Globalization;

namespace thegame
{

    public enum instances_type
    {
        Game,
        Menu,
        SplashScreen
    }

    public enum CharacType
    {
        player,
        ia
    }

    public class Instances
    {
        public Game1 game { get; private set; }
        KeyboardState keyboardState;
        KeyboardState oldkey;
        public instances_type type { get; set; }
        public object execute { get; private set; }
        public int selected { get; private set; }
        public SoundEffect sound { get; private set; }
        private List<string> Text_Game;
        private SoundEffectInstance instancesound;
        public static Rectangle vidRectangle;
        public static VideoPlayer vidPlayer;

        private int[,] tilemap;
        public List<Rectangle> blocks;
        public List<Rectangle> blocksTop;
        public List<Rectangle> blocksLeft;
        public List<Rectangle> blocksRight;
        public List<Rectangle> tile;
        private List<Projectile> projectiles;
        private List<Rectangle> objects = new List<Rectangle> { };

        private List<Perso> iaPerso = new List<Perso>();

        private bool SoundIsTrue;
        private bool moveleft;
        private bool moveright;
        private Drawable tree;
        private Drawable Ground;

        private Drawable debug;

        private List<Texture2D>   texlis;
        private int mapSizeX;
        private int mapSizeY;

        public Vector2 cameraPos = Vector2.Zero;
        

        public bool pause = false;
        MouseState mouse = Mouse.GetState();


        /*LANGUAGE OPTION */
        private void GetText(string language)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (language == "english")
            {
                Text_Game = new List<string> { "Play", "Options", "Exit", "Language", "Full screen", "Back", "English", "French", "Sound", "On", "Off"  };
            }
            else
            {
                Text_Game = new List<string> { "Jouer", "Options", "Quitter", "Langue", "Plein ecran", "Retour", "Anglais", "Français", "Son", "On", "Off" };
            }
        }

        public Instances(Game1 game)
        {
            
                /* LANGUAGE PAR DÉFAUT AU CHARGEMENT */
                this.type = instances_type.Menu;
                this.selected = 0;
                if (CultureInfo.InstalledUICulture.ToString() == "fr-FR")
                {
                    GetText("french");
                }
                else
                {
                    GetText("english");
                }

                
                instancesound = Textures.gameSound_Effect.CreateInstance();
                instancesound.IsLooped = true;


                /* DEFAUT LOADING  : EVERYTHING THAT HAS TO LOAD BY DEFAULT */




                
                
                this.game = game;

                SoundIsTrue = true;
                moveleft = true;
                moveright = true;
                this.selected = 6;
                this.Execute();
        }

    

        public void UpdateByKey(GameTime gametime)
        {
            oldkey = keyboardState;
            keyboardState = Keyboard.GetState();
            MouseState mouse1 = Mouse.GetState();


            if (keyboardState.IsKeyDown(Keys.Escape)) /* Exit the game */
            {
                game.Exit();
            }
            
            

            if (!pause)
            {
                    if (type == instances_type.Menu)// MENU
                    {
                        (execute as Menu).Update(gametime, keyboardState, oldkey, SoundIsTrue);
                    }


                        if (this.selected == 0)
                        {
                            switch ((execute as Menu).selected)
                            {
                                case 0: 
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // START GAME
                                    {
                                        (execute as Menu).MenuBool = false;
                                        this.type = instances_type.Game;
                                        this.selected = 2;
                                        Execute();
                                    }
                                    break;
                                case 1:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // OPTIONS SETTINGS
                                    {
                                        this.selected = 1;
                                        Execute();
                                    }
                                    break;
                                case 2: 
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // EXIT GAME
                                    {
                                        game.Exit();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (selected == 1) // OPTION PANNEL
                        {
                            switch ((execute as Menu).selected) 
                            {
                                case 0:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // LANGUAGE SETTINGS
                                    {
                                        this.selected = 3;
                                        Execute();
                                    }
                                    break;
                                case 1: 
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // FUllSCREEN
                                    {
                                        this.selected = 5;
                                        Execute();
                                    }
                                    break;

                                case 2:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // SOUND SETTINGS
                                    {
                                        this.selected = 4;
                                        Execute();
                                    }
                                    break;
                                case 3:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // GO BACK
                                    {
                                        this.selected = 0;
                                        Execute();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (selected == 3) // LANGUAGE SETTINGS
                        {

                            switch ((execute as Menu).selected)
                            {
                                case 0:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // LANGUAGE 1
                                    {
                                        GetText("english");
                                        this.selected = 0;
                                        Execute();
                                    }
                                    break;
                                case 1:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // LANGUAGE 2
                                    {
                                        GetText("french");
                                        this.selected = 0;
                                        Execute();
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        else if (selected == 4) // SOUND SETTINGS
                        {

                            switch ((execute as Menu).selected)
                            {
                                case 0:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // ON BUTTON
                                    {
                                        SoundIsTrue = true;
                                        this.selected = 0;
                                        Execute();
                                    }
                                    break;
                                case 1:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // OFF BUTTON
                                    {
                                        SoundIsTrue = false;
                                        this.selected = 0;
                                        Execute();
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }

                        else if (selected == 5) // FULLSCREEN SETTINGS
                        {

                            switch ((execute as Menu).selected)
                            {
                                case 0:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // ON BUTTON
                                    {
                                        if (!Game1.graphics.IsFullScreen)
                                        {
                                            Game1.graphics.ToggleFullScreen();
                                        }
                                        this.selected = 0;
                                        Execute();
                                    }
                                    break;
                                case 1:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // OFF  BUTTON
                                    {
                                        if (Game1.graphics.IsFullScreen)
                                        {
                                            Game1.graphics.ToggleFullScreen();
                                        }
                                        this.selected = 0;
                                        Execute();
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        else if (selected == 6) // SPLASHSCREEN
                        {
                            
                            if (vidPlayer.State == MediaState.Stopped || mouse1.LeftButton == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter))
                            {
                                vidPlayer.Stop();
                                this.type = instances_type.Menu;
                                this.selected = 0;
                                Execute();
                                 Textures.openingSound_Effect1.Play();
                            }
                        }



                        else // THIS IS THE GAME 
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.P))
                            {
                                pause = true;
                                Textures.btnPlay.isClicked = false;
                                Textures.btnMenu.isClicked = false;
                            }
                            else
                            {
                                cameraPos = (execute as Perso).cameraPos;
                                /* START OF THE GAME CODE */
                                moveleft = true;
                                moveright = true;


                                foreach (Rectangle left in blocksLeft)
                                {
                                    if ((new Rectangle(left.X, left.Y, left.Width, left.Height)).Intersects((execute as Perso).hitBoxPerso))
                                    {
                                        moveleft = false;
                                    }
                                }

                                foreach (Rectangle right in blocksRight)
                                {
                                    if ((new Rectangle(right.X, right.Y, right.Width, right.Height)).Intersects((execute as Perso).hitBoxPerso))
                                    {
                                        moveright = false;
                                    }
                                }

                                projectiles = new List<Projectile>();
                                (execute as Perso).Update(gametime, keyboardState, oldkey, moveleft, moveright, blocksTop, projectiles, objects);

                                this.objects = (execute as Perso).objects;

                                
                                iaPerso = (execute as Perso).CollisionIAProjec(iaPerso);

                                foreach (Perso iathings in iaPerso)
                                {
                                    moveleft = true;
                                    moveright = true;


                                    foreach (Rectangle left in blocksLeft)
                                    {
                                        if ((new Rectangle(left.X, left.Y, left.Width, left.Height)).Intersects(iathings.hitBoxPerso))
                                        {
                                            moveleft = false;
                                        }
                                    }

                                    foreach (Rectangle right in blocksRight)
                                    {
                                        if ((new Rectangle(right.X, right.Y, right.Width, right.Height)).Intersects(iathings.hitBoxPerso))
                                        {
                                            moveright = false;
                                        }
                                    }
                                    iathings.UpdateIA(gametime, moveleft, moveright, blocksTop, (execute as Perso).hitBoxPerso);
                                    if (iathings.gameover == true)
                                    {
                                        this.type = instances_type.Game;
                                        this.selected = 2;
                                        Execute();
                                        break;
                                    }
                                }



                                if (keyboardState.IsKeyDown(Keys.Back)) /* Go to options settings */
                                {
                                    this.selected = 0;
                                    instancesound.Stop();
                                    this.type = instances_type.Menu;
                                    Execute();
                                    Thread.Sleep(150);
                                }


                            }

                        }
                }
            

            if (pause)
            {
                
                if (Textures.btnPlay.isClicked)
                {
                    pause = false;
                }
                if (Textures.btnMenu.isClicked)
                {
                    pause = false;
                    this.selected = 0;
                    instancesound.Stop();
                    this.type = instances_type.Menu;
                    Execute();
                    Thread.Sleep(150);
                    
                }
                if (Textures.btnQuit.isClicked)
                {
                    game.Exit();
                }

                Textures.btnPlay.Update(mouse);
                Textures.btnMenu.Update(mouse);
                Textures.btnQuit.Update(mouse);
            }
        }

        /* END OF THE GAME CODE */


        public void Execute()
        {
            cameraPos = Vector2.Zero;
            switch (this.selected)
            {
                case 0: /* MAIN MENU */
                    Sound("menu");
                    execute = new Menu(3, "Squickilling");
            (execute as Menu).AddElements(Text_Game[0]);//Play
            (execute as Menu).AddElements(Text_Game[1]);//Options
            (execute as Menu).AddElements(Text_Game[2]);//Exit game
            
                    break;
                case 1: /* OPTION MENU */
                    Sound("menu");
                    execute = new Menu(4, Text_Game[1]);//options
            (execute as Menu).AddElements(Text_Game[3]);//language 
            (execute as Menu).AddElements(Text_Game[4]);//fullscreen
            (execute as Menu).AddElements(Text_Game[8]);//sound
            (execute as Menu).AddElements(Text_Game[5]);//back
   
                    break;

                case 4: /* SOUND MENU */
                    Sound("menu");
                    execute = new Menu(2, Text_Game[8]);//sound
                    (execute as Menu).AddElements(Text_Game[9]);//on
                    (execute as Menu).AddElements(Text_Game[10]);//off
                    

                    break;
                case 5: /* FULL SCREEN */
                    Sound("menu");
                    execute = new Menu(2, Text_Game[4]);//fullscreen
                    (execute as Menu).AddElements(Text_Game[9]);//on
                    (execute as Menu).AddElements(Text_Game[10]);//off
                    break;

                case 6: /* INTRODUCTION : SPLASHSCREEN */
                    this.type = instances_type.SplashScreen;
                    vidPlayer = new VideoPlayer();
                    vidRectangle = new Rectangle(0, 0, 800, 480);
                    vidPlayer.Play(Textures.vid);
                    break;
                case 2: /* GAME START */
                    Sound("menu");
                    Sound("Game");
                    tilemap = new int[,]
                        {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,1,0,0,1,0,1,1,1,1,1,1,1,0,1,1,0,1,0,1,1,1,1,0,0,0,0,0,0,0,0},
                            {0,0,0,1,1,1,0,0,0,1,1,1,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                            {0,0,1,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        };

                    objects = new List<Rectangle>();
                    iaPerso = new List<Perso>();

            /* AT THE TIME IT IS MANUAL : LATER WE WILL HAVE TO USE A MATRIX */
            objects.Add(new Rectangle(320, 320, 10, 10));
            objects.Add(new Rectangle(484, 284, 10, 10));
            objects.Add(new Rectangle(781, 254, 10, 10));
            objects.Add(new Rectangle(996, 194, 10, 10));
            objects.Add(new Rectangle(1333, 284, 10, 10));

            /* IA CHARACTERS */

            iaPerso.Add(new Perso(new Vector2(490, 0), CharacType.ia));
            iaPerso.Add(new Perso(new Vector2(1300, 0), CharacType.ia));


            texlis = new List<Texture2D>();
            mapSizeX = tilemap.GetLength(1);
            mapSizeY = tilemap.GetLength(0);
                    blocks = new List<Rectangle>();
            blocksTop = new List<Rectangle>();
            blocksLeft = new List<Rectangle>();
            blocksRight = new List<Rectangle>();
                    tile = new List<Rectangle>();

                    for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    if (tilemap[y, x] == 1)
                    {
                        blocks.Add(new Rectangle(x * Textures.buche_texture.Width, y * Textures.buche_texture.Height - 80, Textures.buche_texture.Width, Textures.buche_texture.Height));

                    }
                }
            }

            foreach (Rectangle block in blocks)
            {
                blocksTop.Add(new Rectangle(block.X, block.Y, Textures.buche_texture.Width, 1));
            }
            foreach (Rectangle block in blocks)
            {
                blocksRight.Add(new Rectangle(block.X, block.Y + 3, 1, Textures.buche_texture.Height));
            }
            foreach (Rectangle block in blocks)
            {
                blocksLeft.Add(new Rectangle(block.X + Textures.buche_texture.Width, block.Y + 3, 1, Textures.buche_texture.Height));
            }


       

                   
                    execute = new Perso(new Vector2(200, 0), CharacType.player);
                    tree = new Drawable(drawable_type.tree);
                    Ground = new Drawable(drawable_type.Ground);
                    moveright = true;
                    moveleft = true;
                    debug = new Drawable(drawable_type.font);
                    break;
                case 3: /* Start the game */
                    Sound("menu");
                    execute = new Menu(2, Text_Game[3]);
            (execute as Menu).AddElements(Text_Game[6]);
            (execute as Menu).AddElements(Text_Game[7]);
                    break;
                default:
                    break;
            }
        }

        public void Sound(string type)
        {
            if (SoundIsTrue)
            {
                if (type == "menu")
                {
                    Textures.buttonSound_Effect.Play();
                }
                else
                {
                    instancesound.Play();
                }
            }
        }

        public void Display(SpriteBatch sb)
        {

            if (this.selected != 6)
            {
                if (type == instances_type.Menu)
                {
                    (execute as Menu).Display(sb);
                    
                }
                else if (pause)
                {
                 
                        sb.Draw(Textures.background, Vector2.Zero, Color.White);
                        sb.Draw(Textures.ground_texture, new Vector2(0, 408), Color.White);
                        sb.Draw(Textures.ground_texture, new Vector2(790, 408), Color.White);
                        sb.Draw(Textures.pausedTexture, Textures.pausedRectangle, Color.White);
                        Textures.btnPlay.Draw(sb);
                        Textures.btnMenu.Draw(sb);
                        Textures.btnQuit.Draw(sb);
                    

                }
                else
                {

                    tree.Draw(sb, new Vector2(500, 50));
                    tree.Draw(sb, new Vector2(400, 50));
                    tree.Draw(sb, new Vector2(900, 50));
                    tree.Draw(sb, new Vector2(1050, 50));
                    tree.Draw(sb, new Vector2(1400, 50));
                    tree.Draw(sb, new Vector2(1800, 50));
                    tree.Draw(sb, new Vector2(2200, 50));
                    tree.Draw(sb, new Vector2(2400, 50));
                    tree.Draw(sb, new Vector2(3000, 50));
                    tree.Draw(sb, new Vector2(3400, 50));
                    tree.Draw(sb, new Vector2(3900, 50));
                    tree.Draw(sb, new Vector2(4050, 50));
                    tree.Draw(sb, new Vector2(4900, 50));


                    for (int truc = 0; truc < 9; truc++)
                    {
                        Ground.Draw(sb, new Vector2(truc * Textures.ground_texture.Width, 408));
                    }
                    foreach (Rectangle top in blocks)
                    {
                        sb.Draw(Textures.buche_texture, new Rectangle(top.X, top.Y, top.Width, top.Height), Color.White);
                    }

                    /* On affiche les objets */
                    foreach (Rectangle dessine in objects)
                    {
                        sb.Draw(Textures.nut_texture, new Rectangle(dessine.X, dessine.Y, dessine.Width, dessine.Height), Color.White);
                    }

                    /*  foreach (Rectangle top in blocksRight)
                     {
                         sb.Draw(Textures.hitbox, top, Color.White);
                     } // debug right collision 
                     */
                    /* foreach (Rectangle top in blocksRight)
                     {
                         sb.Draw(Textures.hitbox, new Rectangle(top.X + (int)(execute as Perso).offset, top.Y, top.Width, top.Height), Color.Red);
                     } // debug left collision 
                    */
                    /*        debug.Draw(sb, "X : " + (execute as Perso).positionPerso.X.ToString() + " offset : " + (execute as Perso).offset.ToString(), new Vector2(300, 50), Color.White, "normal");
                        */
                    (execute as Perso).Draw(sb); /* Should be execute in the Drawable class */

                    foreach (Perso iathings in iaPerso)
                    {
                        iathings.Draw(sb);
                    }

                    tree.Draw(sb, new Vector2(-100, 50));

                    /* DRAW GROUND */
                }

            }
            else // draw splashscreen
            {
                Drawable.vidTexture = vidPlayer.GetTexture();

                sb.Draw(Drawable.vidTexture, vidRectangle, Color.White);

            }
        }

    }
}
