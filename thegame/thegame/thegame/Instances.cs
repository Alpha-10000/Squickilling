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

        private List<string> Text_Game;     // Contains text of menu options
        private const int _mnuPlay = 0;     // Use this constant to refer to the menu text.
        private const int _mnuOptions = 1;
        private const int _mnuExit = 2;
        private const int _mnuLanguage = 3;
        private const int _mnuFullscreen = 4;
        private const int _mnuBack = 5;
        private const int _mnuEnglish = 6;
        private const int _mnuFrench = 7;
        private const int _mnuSound = 8;
        private const int _mnuOn = 9;
        private const int _mnuOff = 10;
        private const int _mnuDutch = 11;

        private SoundEffectInstance instancesound;
        public static Rectangle vidRectangle;
        public static VideoPlayer vidPlayer, vidPlayer2;

        private int[,] tilemap;
        public List<Rectangle> blocks;
        public List<Rectangle> blocksTop;
        public List<Rectangle> blocksLeft;
        public List<Rectangle> blocksRight;
        public List<Rectangle> tile;
        private List<Projectile> projectiles;
        private List<Rectangle> objects = new List<Rectangle> { };

        private List<Perso> iaPerso = new List<Perso>();

        private int score = 0;          // Score
                                        // TODO: Should score stay here or should it be elsewhere?

        private bool Fullscreen;        // Set to true to switch to fullscreen
        private bool SoundIsTrue;       // Set to true to switch the sound (on / off)
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
        public bool game_over_i = false;
        MouseState mouse = Mouse.GetState();


        /*LANGUAGE OPTION */
        private void GetText(string language)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            switch (language)
            {
                case "english":
                        Text_Game = new List<string> { "Play", 
                                                       "Options", 
                                                       "Exit", 
                                                       "Language", 
                                                       "Full screen", 
                                                       "Back", 
                                                       "English", 
                                                       "Français", //has to be français
                                                       "Sound", 
                                                       "On", 
                                                       "Off",
                                                       "Nederlands" };
                         break;

                case "french":
                    Text_Game = new List<string> { "Jouer",
                                                   "Options", 
                                                   "Quitter", 
                                                   "Langue", 
                                                   "Plein écran", 
                                                   "Retour", 
                                                   "English", 
                                                   "Français", 
                                                   "Son", 
                                                   "Actif", 
                                                   "Inactif",
                                                   "Nederlands" };
                    break;

                case "nederlands":
                    Text_Game = new List<string> { "Spelen", 
                                                   "Opties", 
                                                   "Afsluiten", 
                                                   "Taal",
                                                   "Volledig scherm",
                                                   "Terug",
                                                   "English",
                                                   "Français",
                                                   "Geluid",
                                                   "Aan",
                                                   "Uit",
                                                   "Nederlands" };
                    break;
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
            
            

            if (!pause && !game_over_i)
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
                                        //this.selected = 5;
                                        Fullscreen = !Fullscreen;       // Toggle between fullscreen and window
                                        Game1.graphics.ToggleFullScreen();
                                        Execute();
                                    }
                                    break;

                                case 2:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)) // SOUND SETTINGS
                                    {
                                        //this.selected = 4;
                                        SoundIsTrue = !SoundIsTrue;

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
                                case 2:
                                    if (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter))
                                    {
                                        GetText("nederlands");
                                        this.selected = 0;      // This takes it to the first menu page
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

                        // ES :  I have moved the fullscreen toggle to the option menu.
                        //       This is section is no longer active.
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

                                        game_over_i = true;
                                        /*this.type = instances_type.Game;
                                        this.selected = 2;
                                        Execute();
                                        break;*/
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
                
                if (Textures.btnPlay.isClicked|| (keyboardState.IsKeyDown(Keys.Enter) && !oldkey.IsKeyDown(Keys.Enter)))
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

            if (game_over_i)
            { 
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    game_over_i = false;
                    this.type = instances_type.Game;
                    this.selected = 2;
                    Execute();
                    
                }
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
                    (execute as Menu).AddElements(Text_Game[_mnuPlay]);//Play
                    (execute as Menu).AddElements(Text_Game[_mnuOptions]);//Options
                    (execute as Menu).AddElements(Text_Game[_mnuExit]);//Exit game
            
                    break;
                case 1: /* OPTION MENU */
                    Sound("menu");
                    execute = new Menu(4, Text_Game[_mnuOptions]);//options
                    (execute as Menu).AddElements(Text_Game[_mnuLanguage]);//language 
                    //(execute as Menu).AddElements(Text_Game[4]);//fullscreen  
                    //     Next section modifies text on display mode.
                    if (Fullscreen)
                    {
                        (execute as Menu).AddElements(Text_Game[_mnuFullscreen] + " (" + Text_Game[_mnuOn] + ")"); //fullscreen on
                    }
                    else
                    {
                        (execute as Menu).AddElements(Text_Game[_mnuFullscreen] + " (" + Text_Game[_mnuOff] + ")"); //fullscreen off
                    }

                    //(execute as Menu).AddElements(Text_Game[8]);//sound
                    //      Next section modifies text on sound mode.
                    if (SoundIsTrue)
                    {
                        (execute as Menu).AddElements(Text_Game[_mnuSound] + " (" + Text_Game[_mnuOn] + ")"); //sound on
                    }
                    else
                    {
                        (execute as Menu).AddElements((Text_Game[_mnuSound] + " (" + Text_Game[_mnuOff] + ")")); // sound off
                    }
                    (execute as Menu).AddElements(Text_Game[_mnuBack]);//back
   
                    break;

                case 3: /* Select language */
                    Sound("menu");
                    execute = new Menu(3, Text_Game[_mnuLanguage]);
                    (execute as Menu).AddElements(Text_Game[_mnuEnglish]);
                    (execute as Menu).AddElements(Text_Game[_mnuFrench]);
                    (execute as Menu).AddElements(Text_Game[_mnuDutch]);
                    break;

                case 4: /* SOUND MENU */
                    Sound("menu");
                    execute = new Menu(2, Text_Game[_mnuSound]);//sound
                    (execute as Menu).AddElements(Text_Game[_mnuOn]);//on
                    (execute as Menu).AddElements(Text_Game[_mnuOff]);//off
                    
                   

                    break;
                case 5: /* FULL SCREEN */
                    Sound("menu");
                    execute = new Menu(2, Text_Game[_mnuFullscreen]);//fullscreen
                    (execute as Menu).AddElements(Text_Game[_mnuOn]);//on
                    (execute as Menu).AddElements(Text_Game[_mnuOff]);//off
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
                    score = 0;              // We start the game with the score = 0
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

   int[,] objectsMap = new int[,]
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
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        };

                    int[] iaMap = new int[]{0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0};

                    

                    objects = new List<Rectangle>();
                    iaPerso = new List<Perso>();



                    for (int x = 0; x < objectsMap.GetLength(1); x++)
                    {
                        for (int y = 0; y < objectsMap.GetLength(0); y++)
                        {
                            if (objectsMap[y, x] == 1)
                            {
                                objects.Add(new Rectangle(x * Textures.buche_texture.Width + 50, y  * Textures.buche_texture.Height - 100, 10, 10));
                            }
                        }
                    }


            /* IA CHARACTERS */

            for (int x = 0; x < iaMap.Length; x++)
            {
                    if (iaMap[x] == 1)
                    {
                        iaPerso.Add(new Perso(new Vector2(x * Textures.buche_texture.Width, 0), CharacType.ia));
                    }
            }
            


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
	    scoreDisplay = new Drawable(drawable_type.font);
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
                else if (Perso.game_over && game_over_i)
                {
                    Rectangle rec = new Rectangle(0, 0, 800, 480);
                    
                    sb.Draw(Textures.background, Vector2.Zero, Color.White);
                    sb.Draw(Textures.ground_texture, new Vector2(0, 408), Color.White);
                    sb.Draw(Textures.ground_texture, new Vector2(790, 408), Color.White);
                    sb.Draw(Textures.game_overTexture, rec, Color.White);
                    
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
                    // TODO: Display current score
                    scoreDisplay.Draw(sb, "Score: " + score, new Vector2(10 - cameraPos.X, 10), Color.Black, "normal");
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
