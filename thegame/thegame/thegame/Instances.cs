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
    //------------------------------------------------------------------
    // Define the game modes.
    //------------------------------------------------------------------
    public enum instances_type
    {
        Game,
        Menu,
        SplashScreen,
        MapDevelopper
    }

    //------------------------------------------------------------------
    // Define the character types. (player character / NPC)
    //------------------------------------------------------------------
    public enum CharacType
    {
        player,
        ia
    }
    public enum gameState
    {
        MainMenu = 0,
        OptionMenu = 1,
        AutumnLevel = 2,
        LanguageMenu = 3,
        SoundMenu = 4,
        FullscreenMenu = 5,
        SplashScreen = 6,
        DeveloperMode = 7,
        WinterLevel = 8,
        SpringLevel,
        SummerLevel
    }

    public class Instances
    {
        public Game1 game { get; private set; }

        KeyboardState keyboardState;        // Used to manage the keyboard input.
        KeyboardState oldkey;
        MouseState OldMouse;
        MouseState mouse1;

        public instances_type curGameMode { get; set; }        // Current game mode.
        public object execute { get; private set; }            // Current activ object (Menu / Perso) 
        public gameState selected { get; private set; }              // Selected menu page.
        public SoundEffect sound { get; private set; }

        private bool drawBloodScreen = false;//variable for the bloodscreen
        private float elapsedTimeBloodScreen = 0;

        private Camera cameraClass = new Camera();

        private Drawable scoreDisplay;
        private Dictionary<string, string> Text_Game; // Contains text of menu options

        private SoundEffectInstance instancesound;
        public static Rectangle vidRectangle;
        public static VideoPlayer vidPlayer, vidPlayer2;
        string language;
        private int[,] tilemap;
        public List<Rectangle> blocks;
        public List<Rectangle> blocksTop;
        public List<Rectangle> blocksLeft;
        public List<Rectangle> blocksRight;
        public List<Rectangle> blocksBottom;
        public List<Rectangle> tile;
        private List<Projectile> projectiles;
        private List<Rectangle> objects = new List<Rectangle> { };
        private List<Bomb> bomb = new List<Bomb> { };

        private List<Perso> iaPerso = new List<Perso>();

        private int score = 0;          // Score
        private int nb_nuts;
        // TODO: Should score stay here or should it be elsewhere?

        private bool Fullscreen;        // Set to true to switch to fullscreen
        private bool SoundIs;       // Set to true to switch the sound (on / off)
        private Drawable tree;
        private Drawable tree_autumn_entrance_inside;
        private Drawable tree_autumn_exit;
        private Drawable tree_autumn_exit_inside;
        private Drawable tree_winter_entrance_inside;
        private Drawable Ground;

        private Drawable debug;
        //private Drawable scoreDisplay;

        private List<Texture2D> texlis;
        private int mapSizeX;
        private int mapSizeY;

        public Vector2 cameraPos = Vector2.Zero;


        private float timeElaspedGameOver = 0;
        private bool playerActivate = true;
        private float transparencyAnimation = 0;
        private int Health; //BASIC LEVEL OF PERSO
        public bool pause = false;
        public bool game_over_i = false;
        public bool help = false;
        public bool endLevel = false;
        private bool developpermap = false;

        private float elaspedTimeGetBackHealth = 0;
        private float timeInSecond_GetOnePoint = 30;//Get back 1 point of life every 30 seconds. 
        MouseState mouse = Mouse.GetState();

        public bool Developpermode = false;//this is just for us. Activate the developper mode

        private float developperXMouse;
        private float developperYMouse;
        private bool developperCoord = false;

        /* EVERYTHING THAT HAS TO BE RESET AT GAME OVER OR BEGINNING OF THE GAME */
        private void Init_Game()
        {
            this.nb_nuts = 0;
            this.Health = 20;
            drawBloodScreen = false;
            cameraClass.shake = false;
            bomb = new List<Bomb>();
        }

        /*LANGUAGE OPTION */
        private void GetText(string language)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            switch (language)
            {
                case "english":
                    Text_Game = new Dictionary<string, string>()
                    {
                                                   {"_mnuPlay","Play"},
                                                   {"_mnuOptions","Options"}, 
                                                   {"_mnuExit","Exit"},
                                                   {"_mnuLanguage","Language"},
                                                   {"_mnuFullscreen","Full screen"}, 
                                                   {"_mnuBack","Back"},
                                                   {"_mnuEnglish","English"},
                                                   {"_mnuFrench","Français"}, //has to be français
                                                   {"_mnuSound","Sound"}, 
                                                   {"_mnuOn","On"},
                                                   {"_mnuOff","Off"},
                                                   {"_mnuDutch","Nederlands"},
                                                   {"_gamePause","Press P to pause"},
                                                   {"_gameHelp","Press H to get help"},
                                                   {"_gameHealth","Health"},
                                                   {"_gamescore","score"},
                                                   {"_gamebonus","bonus"},
                                                   {"_gameHelpLine1","Use the left and right arrow to move the character"},
                                                   {"_gameHelpLine2","Use the top arrow to jump"},
                                                   {"_gameHelpLine3","To fire on ennemies use the space bar"},
                                                   {"_gameHelpLine4","Press any key to exit"},
                                                   {"congrats","End of the level, so badass..."},
                                                   {"finalScore","Ennemies Killed : "},
                                                   {"finalBonus","Bonus Score : "},
                                                   {"total","Total : "},
                                                   {"space","Press Space key"}
                    
                    };
                    break;

                case "french":
                    Text_Game = new Dictionary<string, string>()
                    {                              
                                                   {"_mnuPlay","Jouer"},
                                                   {"_mnuOptions","Options"},
                                                   {"_mnuExit","Quitter"},
                                                   {"_mnuLanguage","Langue"},
                                                   {"_mnuFullscreen","Plein écran"}, 
                                                   {"_mnuBack","Retour"},
                                                   {"_mnuEnglish","English"},
                                                   {"_mnuFrench","Français"},
                                                   {"_mnuSound","Son"},
                                                   {"_mnuOn","Actif"},
                                                   {"_mnuOff","Inactif"},
                                                   {"_mnuDutch","Nederlands"},
                                                   {"_gamePause","P = pause"},
                                                   {"_gameHelp","H = aide"},
                                                   {"_gameHealth","Vie"},
                                                   {"_gamescore","score"},
                                                   {"_gamebonus","bonus"},
                                                   {"_gameHelpLine1","Utilisez la flèche gauche/droite pour se déplacer"},
                                                   {"_gameHelpLine2","Utilisez la flèche du haut pour sauter"},
                                                   {"_gameHelpLine3","Utilisez la barre espace pour tirer"},
                                                   {"_gameHelpLine4","Appuyez sur une touche pour quitter"},
                                                   {"congrats","Fin du niveau, trop badass..."},
                                                   {"finalScore","Nombre d'ennemis morts : "},
                                                   {"finalBonus","Score bonus : "},
                                                   {"total","Total : "},
                                                   {"space","Appuyer sur la touche Espace"}
                    };
                    break;

                case "nederlands":
                    Text_Game = new Dictionary<string, string>()
                    {
                                                   {"_mnuPlay","Spelen"},
                                                   {"_mnuOptions","Opties"}, 
                                                   {"_mnuExit","Afsluiten"}, 
                                                   {"_mnuLanguage","Taal"},
                                                   {"_mnuFullscreen","Volledig scherm"},
                                                   {"_mnuBack","Terug"},
                                                   {"_mnuEnglish","English"},
                                                   {"_mnuFrench","Français"},
                                                   {"_mnuSound","Geluid"},
                                                   {"_mnuOn","Aan"},
                                                   {"_mnuOff","Uit"},
                                                   {"_mnuDutch","Nederlands"},
                                                   {"_gamePause","Druk P voor pause"},
                                                   {"_gameHelp","Druk H voor help"},
                                                   {"_gameHealth","Health"},
                                                   {"_gamescore","score"},
                                                   {"_gamebonus","bonus"},
                                                   {"_gameHelpLine1","Cursor links en rechts om te bewegen"},
                                                   {"_gameHelpLine2","Cursor omhoog om te springen"},
                                                   {"_gameHelpLine3","Spaciebalk om te schieten"},
                                                   {"_gameHelpLine4","Druk een willekeurige om werder te spelen"},
                                                   {"congrats","Geweldig! Je hebt het level gehaald"},
                                                   {"finalScore","Je score is : "},
                                                   {"finalBonus","Bonus score : "},
                                                   {"total","Totaal : "},
                                                   {"space","Druk spacie balk"}
                    };
                    break;
            }
        }

        public Instances(Game1 game)
        {
            /* LANGUAGE PAR DÉFAUT AU CHARGEMENT */
            this.curGameMode = instances_type.Menu;
            this.selected = 0;
            if (CultureInfo.InstalledUICulture.ToString() == "fr-FR")
            {
                GetText("french");
                language = "french";
            }
            else
            {
                GetText("english");
                language = "english";
            }

            instancesound = Textures.gameSound_Effect.CreateInstance();
            instancesound.IsLooped = true;

            /* DEFAUT LOADING  : EVERYTHING THAT HAS TO LOAD BY DEFAULT */

            this.game = game;

            SoundIs = true;
            this.selected = gameState.SplashScreen;
            Init_Game();
            this.Execute();
        }


        public void UpdateByKey(GameTime gametime)
        {
            oldkey = keyboardState;
            keyboardState = Keyboard.GetState();

            OldMouse = mouse1;
            mouse1 = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape)) /* Exit the game */
            {
                game.Exit();
            }

            if (!pause && !game_over_i)
            {
                if (curGameMode == instances_type.Menu)// MENU
                {
                    (execute as Menu).Update(gametime, keyboardState, oldkey, SoundIs, mouse1, OldMouse);
                }

                if (this.selected == gameState.MainMenu && (execute as Menu).IChooseSomething)
                {
                    switch ((execute as Menu).selected)
                    {
                        case 0:
                                (execute as Menu).MenuBool = false;
                                this.curGameMode = instances_type.Game;
                                this.selected = gameState.AutumnLevel;
                                Execute();
                            break;
                        case 1:
                                this.selected = gameState.OptionMenu;
                                Execute();
                            break;
                        case 2:
                                game.Exit();
                            break;
                        default:
                            break;
                    }
                }
                else if (selected == gameState.OptionMenu && (execute as Menu).IChooseSomething) // OPTION PANNEL
                {
                    switch ((execute as Menu).selected)
                    {
                        case 0:
                                this.selected = gameState.LanguageMenu;
                                Execute();
                            break;
                        case 1:
                                //this.selected = 5;
                                Fullscreen = !Fullscreen;       // Toggle between fullscreen and window
                                Game1.graphics.ToggleFullScreen();
                                //Game1.graphics.IsFullScreen = !Game1.graphics.IsFullScreen;
                                //Game1.graphics.ApplyChanges();
                                if (Fullscreen)
                                    (execute as Menu).tab[1] = Text_Game["_mnuFullscreen"] + " (" + Text_Game["_mnuOn"] + ")"; //fullscreen on
                                else
                                    (execute as Menu).tab[1] = Text_Game["_mnuFullscreen"] + " (" + Text_Game["_mnuOff"] + ")"; //fullscreen off
                                (execute as Menu).IChooseSomething = false;
                            break;

                        case 2:
                                //this.selected = 4;
                                SoundIs = !SoundIs;
                                if (SoundIs)
                                    (execute as Menu).tab[2] = Text_Game["_mnuSound"] + " (" + Text_Game["_mnuOn"] + ")"; //sound on
                                else
                                    (execute as Menu).tab[2] = Text_Game["_mnuSound"] + " (" + Text_Game["_mnuOff"] + ")"; // sound off
                                (execute as Menu).IChooseSomething = false;
                            break;
                        case 3:
                                this.selected = gameState.MainMenu;
                                Execute();
                            break;
                        default:
                            break;
                    }
                }
                else if (selected == gameState.LanguageMenu && (execute as Menu).IChooseSomething) // LANGUAGE SETTINGS
                {

                    switch ((execute as Menu).selected)
                    {
                        case 0:
                                GetText("english");
                                language = "english";
                                this.selected = 0;
                                Execute();
                            break;
                        case 1:
                                GetText("french");
                                language = "french";
                                this.selected = 0;
                                Execute();
                            break;
                        case 2:
                                GetText("nederlands");
                                language = "nederlands";
                                this.selected = 0;      // This takes it to the first menu page
                                Execute();
                            break;
                        default:
                            break;
                    }
                }
                else if (selected == gameState.SoundMenu && (execute as Menu).IChooseSomething) // SOUND SETTINGS
                {

                    switch ((execute as Menu).selected)
                    {
                        case 0:
                                SoundIs = true;
                                this.selected = gameState.MainMenu;
                                Execute();
                            break;
                        case 1:
                                SoundIs = false;
                                this.selected = gameState.MainMenu;
                                Execute();
                            break;
                        default:
                            break;
                    }
                }
                               
                //------------------------------------------------------------------
                // ES 15APR14
                // Moved details of splashscreen handling near the end of this class.
                //------------------------------------------------------------------
                else if (selected == gameState.SplashScreen) HandleSplashScreen(keyboardState, mouse1);
                else if (selected == gameState.DeveloperMode)
                {
                    cameraPos = (execute as DevelopperMap).cameraPos;
                    (execute as DevelopperMap).UpdateMap(keyboardState, gametime, mouse1);
                }

                else if (selected == gameState.AutumnLevel || selected == gameState.WinterLevel || selected == gameState.SummerLevel || selected == gameState.SpringLevel)// THIS IS THE GAME 
                {

                    if (Developpermode)//just a little something for us
                        Health = 20;

                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        pause = true;
                        Textures.btnPlay.isClicked = false;
                        Textures.btnMenu.isClicked = false;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.H) && !oldkey.IsKeyDown(Keys.H))
                    {
                        if (help)
                            help = false;
                        else
                            help = true;
                    }

                    if ((execute as Perso).positionPerso.X > 5350)
                    {
                        endLevel = true;
                        if (keyboardState.IsKeyDown(Keys.Space))
                        {
                            endLevel = false;
                            this.curGameMode = instances_type.Game;
                            this.selected = gameState.WinterLevel;
                            Execute();
                        }
                    }
                    else if (!pause && !help && !endLevel)
                    {
                        int checkBlood = 0;

                        foreach (Perso iathings in iaPerso)
                        {
                            if(playerActivate)
                                checkBlood += iathings.TryToKill(ref Health, (execute as Perso).hitBoxPerso);

                            iathings.UpdateIA(gametime, blocks, (execute as Perso).hitBoxPerso);
                        }

                        bool touchedByBomb = false;

                        /* CHECK IF CHARACTER CROSS A MINE */
                        foreach (Bomb checkCrossed in bomb)
                        {
                            if ((execute as Perso).hitBoxPerso.Intersects(checkCrossed.Object) && !checkCrossed.activateExplosion)
                            {
                                touchedByBomb = true;
                                checkCrossed.activateExplosion = true;
                                drawBloodScreen = true;
                                if (checkCrossed.checkBlood)
                                    Textures.gameExplosion_Effect.Play();
                                checkCrossed.BloodOnce(ref Health);
                                break;
                            }
                            if (checkCrossed.activateExplosion)// important to keep the blood screen active until the end of the explosion
                                drawBloodScreen = true;
                        }

                        if (touchedByBomb)//fait clignoter le perso
                        {
                            (execute as Perso).PersoHitted = true;
                            (execute as Perso).compteurHitted = 0;
                        }
                        
                        bomb.RemoveAll(x => x.checkIfFinish);//remove bomb when explosion animation is complete

                        if (playerActivate)
                        {
                            cameraPos = (execute as Perso).cameraPos;
                            
                            elaspedTimeGetBackHealth += (float)gametime.ElapsedGameTime.TotalSeconds;
                            if (elaspedTimeGetBackHealth > timeInSecond_GetOnePoint && Health < 20)
                            {
                                Health++;
                                elaspedTimeGetBackHealth = 0;
                            }

                            projectiles = new List<Projectile>();
                            (execute as Perso).Update(gametime, keyboardState, oldkey, blocks, projectiles, objects, ref nb_nuts, Developpermode);
                            this.objects = (execute as Perso).objects;
                            iaPerso = (execute as Perso).CollisionIAProjec(iaPerso, ref score);
                            
                            if (checkBlood > 0)
                            {
                                drawBloodScreen = true;
                                (execute as Perso).PersoHitted = true;
                                (execute as Perso).compteurHitted = 0;
                            }

                            if (Health <= 0)
                            {
                                Health = 0;
                                playerActivate = false;
                                drawBloodScreen = false;//solve a bug. Otherwise the bloodscreen will continue to force the camera to shake
                            }
                        }
                        else
                        {
                            timeElaspedGameOver += gametime.ElapsedGameTime.Milliseconds;
                            if (timeElaspedGameOver > 2500)
                            {
                                timeElaspedGameOver = 0;
                                playerActivate = true;
                                game_over_i = true;
                            }
                            if (timeElaspedGameOver > 1500)
                            {
                                transparencyAnimation = (timeElaspedGameOver - 1500) / 1000;
                            }
                        }

                        if (keyboardState.IsKeyDown(Keys.Back)) /* Go to options settings */
                        {
                            this.selected = 0;
                            instancesound.Stop();
                            this.curGameMode = instances_type.Menu;
                            Execute();
                        }
                    }
                }
            }
            
            if (pause)
            {
                /* Pause menu with keyboard*/

                if (Textures.btnPlay.isSelected == false && Textures.btnMenu.isSelected == false && Textures.btnQuit.isSelected == false)
                    Textures.btnPlay.isSelected = true;

                if (keyboardState.IsKeyDown(Keys.Down) && !oldkey.IsKeyDown(Keys.Down))
                {
                    if (Textures.btnMenu.isSelected)
                    {
                        Textures.btnMenu.isSelected = false;
                        Textures.btnQuit.isSelected = true;
                    }
                    if (Textures.btnPlay.isSelected && !oldkey.IsKeyDown(Keys.Down))
                    {
                        Textures.btnPlay.isSelected = false;
                        Textures.btnMenu.isSelected = true;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Up) && !oldkey.IsKeyDown(Keys.Up))
                {
                    if (Textures.btnMenu.isSelected)
                    {
                        Textures.btnMenu.isSelected = false;
                        Textures.btnPlay.isSelected = true;
                    }
                    if (Textures.btnQuit.isSelected && !oldkey.IsKeyDown(Keys.Up))
                    {
                        Textures.btnQuit.isSelected = false;
                        Textures.btnMenu.isSelected = true;
                    }
                }

                if (Textures.btnPlay.isClicked)
                    pause = false;

                if (Textures.btnMenu.isClicked)
                {
                    pause = false;
                    this.selected = 0;
                    instancesound.Stop();
                    this.curGameMode = instances_type.Menu;
                    Execute();
                }
                if (Textures.btnQuit.isClicked)
                    game.Exit();

                Textures.btnPlay.Update(mouse, keyboardState);
                Textures.btnMenu.Update(mouse, keyboardState);
                Textures.btnQuit.Update(mouse, keyboardState);
            }

            if (help && !Keyboard.GetState().IsKeyDown(Keys.H) && (Keyboard.GetState().GetPressedKeys().Length > 0 || mouse1.LeftButton == ButtonState.Pressed))
                help = false;

            if (game_over_i)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    game_over_i = false;
                    Init_Game();
                    this.curGameMode = instances_type.Game;
                    this.selected = gameState.AutumnLevel;
                    Execute();
                }
            }

            Keys[] getkey = Keyboard.GetState().GetPressedKeys();

            //ACTIVATE DEVELOPER MODE BY PRESSING THE WORD TEAM. SAME TIME
            if (getkey.Contains(Keys.T) && getkey.Contains(Keys.E) && getkey.Contains(Keys.A) && getkey.Contains(Keys.M))
                Developpermode = true;

            if (getkey.Contains(Keys.N) && getkey.Contains(Keys.O) && Developpermode)
            {
                Developpermode = false;
                if (developpermap)
                {
                    developpermap = false;
                    this.selected = 0;
                    curGameMode = instances_type.Menu;
                    Execute();
                }
            }

            if (Developpermode)
            {
                SoundIs = false;
                developperXMouse = mouse1.X;
                developperYMouse = mouse1.Y;
                if (getkey.Contains(Keys.C) && !developperCoord)
                    developperCoord = true;
                if (getkey.Contains(Keys.X) && developperCoord)
                    developperCoord = false;
                if (getkey.Contains(Keys.M) && getkey.Contains(Keys.A) && getkey.Contains(Keys.P))
                {
                    curGameMode = instances_type.MapDevelopper;
                    selected = gameState.DeveloperMode;
                    developpermap = true;
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
                    (execute as Menu).AddElements(Text_Game["_mnuPlay"]);//Play
                    (execute as Menu).AddElements(Text_Game["_mnuOptions"]);//Options
                    (execute as Menu).AddElements(Text_Game["_mnuExit"]);//Exit game

                    break;
                case gameState.OptionMenu: /* OPTION MENU */
                    Sound("menu");
                    execute = new Menu(4, Text_Game["_mnuOptions"]);//options
                    (execute as Menu).AddElements(Text_Game["_mnuLanguage"]);//language 
                    //(execute as Menu).AddElements(Text_Game[4]);//fullscreen  
                    //     Next section modifies text on display mode.
                    if (Fullscreen)
                        (execute as Menu).AddElements(Text_Game["_mnuFullscreen"] + " (" + Text_Game["_mnuOn"] + ")"); //fullscreen on
                    else
                        (execute as Menu).AddElements(Text_Game["_mnuFullscreen"] + " (" + Text_Game["_mnuOff"] + ")"); //fullscreen off

                    //(execute as Menu).AddElements(Text_Game[8]);//sound
                    //      Next section modifies text on sound mode.
                    if (SoundIs)
                        (execute as Menu).AddElements(Text_Game["_mnuSound"] + " (" + Text_Game["_mnuOn"] + ")"); //sound on
                    else
                        (execute as Menu).AddElements((Text_Game["_mnuSound"] + " (" + Text_Game["_mnuOff"] + ")")); // sound off
                    (execute as Menu).AddElements(Text_Game["_mnuBack"]);//back

                    break;

                case gameState.LanguageMenu: /* Select language */
                    Sound("menu");
                    execute = new Menu(3, Text_Game["_mnuLanguage"]);
                    (execute as Menu).AddElements(Text_Game["_mnuEnglish"]);
                    (execute as Menu).AddElements(Text_Game["_mnuFrench"]);
                    (execute as Menu).AddElements(Text_Game["_mnuDutch"]);
                    break;

                case gameState.SoundMenu: /* SOUND MENU */
                    Sound("menu");
                    execute = new Menu(2, Text_Game["_mnuSound"]);//sound
                    (execute as Menu).AddElements(Text_Game["_mnuOn"]);//on
                    (execute as Menu).AddElements(Text_Game["_mnuOff"]);//off

                    break;
                case gameState.FullscreenMenu: /* FULL SCREEN */
                    Sound("menu");
                    execute = new Menu(2, Text_Game["_mnuFullscreen"]);//fullscreen
                    (execute as Menu).AddElements(Text_Game["_mnuOn"]);//on
                    (execute as Menu).AddElements(Text_Game["_mnuOff"]);//off
                    break;

                case gameState.SplashScreen: /* INTRODUCTION : SPLASHSCREEN */
                    this.curGameMode = instances_type.SplashScreen;
                    vidPlayer = new VideoPlayer();
                    vidRectangle = new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
                    vidPlayer.Play(Textures.vid);
                    break;
                case gameState.DeveloperMode:
                    this.curGameMode = instances_type.MapDevelopper;
                    execute = new DevelopperMap(45, 15);
                    tree = new Drawable(drawable_type.tree);
                    break;

                case gameState.WinterLevel: /*Level 2*/
                    Sound("menu");
                    Sound("Game");
                    score = 0;

                    Init_Game();
                    tilemap = new int[,]
                     {
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,1,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,1,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,2,1,0,0,0,2,0,0,0,0,2,1,1,2,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,2,0,0,0,3,1,0,3,0,2,3,1,0,0,0,1,0,0,0,0,1,0,1,0,1,1,1,1,0,0,1,0,0,0,0,2,1,0,0,0},
                    {0,0,0,0,3,1,0,2,0,1,0,0,1,1,1,1,1,0,2,1,1,0,3,0,0,0,0,1,0,0,3,2,2,1,0,1,0,0,0,3,1,0,2,3,3},
                    {0,0,0,0,1,1,3,1,1,2,0,1,0,0,0,0,1,0,1,0,0,0,1,1,1,0,0,1,0,0,1,1,2,0,1,1,0,0,2,1,0,0,1,1,1},
                    {0,0,2,1,0,0,1,1,2,3,2,0,0,0,0,2,0,0,3,2,0,3,2,2,2,3,0,1,3,0,0,0,3,0,2,0,0,3,2,0,0,0,3,2,2},
                    };



                    int[] iaMap = new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };

                    objects = new List<Rectangle>();
                    iaPerso = new List<Perso>();
                    bomb = new List<Bomb>();



                    /* IA CHARACTERS */
                    for (int x = 0; x < iaMap.Length; x++)
                        if (iaMap[x] == 1)
                            iaPerso.Add(new Perso(new Vector2(x * Textures.buche_texture_winter.Width, 0), CharacType.ia));

                    texlis = new List<Texture2D>();
                    mapSizeX = tilemap.GetLength(1);
                    mapSizeY = tilemap.GetLength(0);
                    blocks = new List<Rectangle>();
                    tile = new List<Rectangle>();

                    for (int x = 0; x < mapSizeX; x++)
                        for (int y = 0; y < mapSizeY; y++)
                            if (tilemap[y, x] == 1)
                                blocks.Add(new Rectangle(x * Textures.buche_texture_winter.Width,
                                                         y * Textures.buche_texture_winter.Height - 95,
                                                         Textures.buche_texture_winter.Width, Textures.buche_texture_winter.Height));
                            else if (tilemap[y, x] == 2)
                                objects.Add(new Rectangle(x * Textures.buche_texture_winter.Width + 50,
                                                          y * Textures.buche_texture_winter.Height - 86, 10, 10));
                            else if (tilemap[y, x] == 3)
                            {
                                int h;
                                if (y == tilemap.GetLength(0) - 1)
                                    h = 345;
                                else
                                    h = y * Textures.buche_texture.Height - 73;
                                bomb.Add(new Bomb(new Rectangle(x * Textures.buche_texture.Width + 50, h, 15, 10)));
                            }

                    execute = new Perso(new Vector2(200, 0), CharacType.player);
                    tree = new Drawable(drawable_type.winterTree);
                    tree_winter_entrance_inside = new Drawable(drawable_type.tree_winter_entrance_inside);
                    Ground = new Drawable(drawable_type.WinterGround);
                    debug = new Drawable(drawable_type.font);
                    scoreDisplay = new Drawable(drawable_type.font);

                    break;
                case gameState.AutumnLevel: /* GAME START */
                    Sound("menu");
                    Sound("Game");
                    score = 0;              // We start the game with the score = 0

                    Init_Game();
                    tilemap = new int[,]
                        {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,0,0,0,1,1,1,0,1,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,1,1,1,2,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,2,1,0,0,0,3,1,1,0,0,0,0,0,1,2,2,0,1,0,0,0,0,0,0,0,0,1,2,3,1,0,0,0,0,0,2,0,0,0},
                            {0,0,0,0,0,3,1,0,0,3,0,1,2,0,0,1,0,0,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,1,1,1,0,0,2,0,1,1,0,3,0},
                            {0,0,3,1,1,1,0,0,0,1,1,1,1,1,1,0,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                            {0,0,1,2,2,2,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,0,0,0,0,0,0,1,2,2,2,3,2,3,3,2},
                        };

                    iaMap = new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };

                    objects = new List<Rectangle>();
                    iaPerso = new List<Perso>();

                    /* IA CHARACTERS */
                    for (int x = 0; x < iaMap.Length; x++)
                        if (iaMap[x] == 1)
                            iaPerso.Add(new Perso(new Vector2(x * Textures.buche_texture.Width, 0), CharacType.ia));

                    texlis = new List<Texture2D>();
                    mapSizeX = tilemap.GetLength(1);
                    mapSizeY = tilemap.GetLength(0);
                    blocks = new List<Rectangle>();
                    tile = new List<Rectangle>();

                    for (int x = 0; x < mapSizeX; x++)
                        for (int y = 0; y < mapSizeY; y++)
                            if (tilemap[y, x] == 1)
                                blocks.Add(new Rectangle(x * Textures.buche_texture.Width, y * Textures.buche_texture.Height - 95, Textures.buche_texture.Width, Textures.buche_texture.Height));
                            else if (tilemap[y, x] == 2)
                                objects.Add(new Rectangle(x * Textures.buche_texture.Width + 50, y * Textures.buche_texture.Height - 86, 10, 10));
                            else if (tilemap[y, x] == 3)
                            {
                                int h;
                                if (y == tilemap.GetLength(0) - 1)
                                    h = 345;
                                else
                                    h = y * Textures.buche_texture.Height - 73;
                                bomb.Add(new Bomb(new Rectangle(x * Textures.buche_texture.Width + 50, h, 15, 10)));
                            }

                    execute = new Perso(new Vector2(200, 0), CharacType.player);
                    tree = new Drawable(drawable_type.tree);
                    tree_autumn_entrance_inside = new Drawable(drawable_type.tree_autumn_entrance_inside);
                    tree_autumn_exit = new Drawable(drawable_type.tree_autumn_exit);
                    tree_autumn_exit_inside = new Drawable(drawable_type.tree_autumn_exit_inside);
                    Ground = new Drawable(drawable_type.AutumnGround);
                    debug = new Drawable(drawable_type.font);
                    scoreDisplay = new Drawable(drawable_type.font);
                    break;

                default:
                    break;
            }
        }

        public void Sound(string type)
        {
            if (SoundIs)
                if (type == "menu")
                    Textures.buttonSound_Effect.Play();
                else
                    instancesound.Play();
        }

        public void GameOverAnimation(SpriteBatch sb, float transparency)
        {
            sb.Begin();
            Rectangle rec = new Rectangle(0, 0, 800, 530);
            sb.Draw(Textures.background, Vector2.Zero, Color.White * transparency);
            sb.Draw(Textures.autumn_ground_texture, new Vector2(0, 405), Color.White * transparency);
            sb.Draw(Textures.autumn_ground_texture, new Vector2(790, 405), Color.White * transparency);

            if (language == "english")
                sb.Draw(Textures.game_overTexture_en, rec, Color.White * transparency);
            else if (language == "french")
                sb.Draw(Textures.game_overTexture_fr, rec, Color.White * transparency);
            else
                sb.Draw(Textures.game_overTexture_ne, rec, Color.White * transparency);
            sb.End();
        }

        public void Display(SpriteBatch sb, GameTime gameTime)
        {
            //------------------------------------------------------------------
            // ES 23APR14
            // HOW TO DRAW STATIC VS MOVING SCREEN
            // Just use Sb.Begin() + sb.End() for static
            // and use sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix); + sb.End()
            // for shifting screen
            //------------------------------------------------------------------
            cameraClass.Position = this.cameraPos;

            if (this.selected != gameState.SplashScreen)
            {
                if (curGameMode == instances_type.Menu)
                {
                    sb.Begin();
                    (execute as Menu).Display(sb, Developpermode);
                    sb.End();
                }
                else if (curGameMode == instances_type.MapDevelopper)
                {
                            sb.Begin();
                            // Makes the background move slower than the camera to create an effect of depth.
                            sb.Draw(Textures.background, new Vector2(cameraClass.Position.X / 3 - 1, -43), Color.White * 0.9f);
                            sb.End();

                            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                            tree.Draw(sb, new Vector2(-100, 0));
                            (execute as DevelopperMap).Display(sb, gameTime);
                            sb.End();
                            sb.Begin();
                            sb.Draw(Textures.buche_texture, new Rectangle(22, 452, Textures.buche_texture.Width, Textures.buche_texture.Height), Color.White);
                            sb.Draw(Textures.eraser, new Rectangle(722, 452, Textures.eraser.Width, Textures.eraser.Height), Color.White);
                            sb.Draw(Textures.hitbox, new Rectangle(180, 452, 15, 10), Color.Gray);
                            sb.Draw(Textures.nut_texture, new Rectangle(250, 452, 10, 10), Color.White);
                            Drawable info = new Drawable(drawable_type.font);
                            Drawable ecri = new Drawable(drawable_type.font);
                            ecri.Draw(sb, "Send by email to ", new Vector2(250, 470), Color.Black, "normal");
                            ecri.Draw(sb, "Alpha", new Vector2(409, 470), Color.Black, "normal");
                            ecri.Draw(sb, "Elise", new Vector2(470, 470), Color.Black, "normal");
                            ecri.Draw(sb, "Thibault", new Vector2(525, 470), Color.Black, "normal");
                            ecri.Draw(sb, "Victor", new Vector2(614, 470), Color.Black, "normal");
                            info.Draw(sb, "S: show grid.      H: hide grid.    Right button to unselect", new Vector2(25, 494), Color.Black, "normal");
                            sb.End();

                }
                else if (pause)
                {
                    switch (this.selected)
                    {
                        case gameState.AutumnLevel:
                            sb.Begin();
                            sb.Draw(Textures.background, Vector2.Zero, Color.White);
                            sb.Draw(Textures.autumn_ground_texture, new Vector2(0, 405), Color.White);
                            sb.Draw(Textures.autumn_ground_texture, new Vector2(790, 405), Color.White);
                            sb.Draw(Textures.pausedTexture, Textures.pausedRectangle, Color.White);
                            Textures.btnPlay.Draw(sb);
                            Textures.btnMenu.Draw(sb);
                            Textures.btnQuit.Draw(sb);
                            sb.End();
                            break;

                        case gameState.WinterLevel:
                            sb.Begin();
                            sb.Draw(Textures.winterBackground, Vector2.Zero, Color.White);
                            sb.Draw(Textures.winter_ground_texture, new Vector2(0, 405), Color.White);
                            sb.Draw(Textures.winter_ground_texture, new Vector2(790, 405), Color.White);
                            sb.Draw(Textures.pausedTexture, Textures.pausedRectangle, Color.White);
                            Textures.btnPlay.Draw(sb);
                            Textures.btnMenu.Draw(sb);
                            Textures.btnQuit.Draw(sb);
                            sb.End();
                            break;
                    }
                }
                else if (game_over_i && playerActivate)
                    GameOverAnimation(sb, 1);
                else if (endLevel)
                {
                    switch (this.selected)
                    {
                        case gameState.AutumnLevel:
                            sb.Begin();
                            Rectangle rec = new Rectangle(0, 0, 800, 530);

                            sb.Draw(Textures.background, Vector2.Zero, Color.White);
                            sb.Draw(Textures.autumn_ground_texture, new Vector2(0, 405), Color.White);
                            sb.Draw(Textures.autumn_ground_texture, new Vector2(790, 405), Color.White);
                            sb.Draw(Textures.hitbox, new Rectangle(0, 0, 1100, 550), Color.Black * 0.5f);
                            scoreDisplay.Draw(sb, Text_Game["congrats"], new Vector2(50, 100), Color.Black, "42");
                            scoreDisplay.Draw(sb, Text_Game["finalScore"] + score, new Vector2(50, 150), Color.Black, "osef");
                            scoreDisplay.Draw(sb, Text_Game["finalBonus"] + nb_nuts, new Vector2(50, 200), Color.Black, "osef");
                            scoreDisplay.Draw(sb, Text_Game["total"] + (score + nb_nuts*0.5), new Vector2(50, 250), Color.Black, "42");
                            scoreDisplay.Draw(sb, Text_Game["space"], new Vector2(70, 300), Color.Black, "osef");
                            sb.End();
                            break;

                        case gameState.WinterLevel:
                            sb.Begin();
                            rec = new Rectangle(0, 0, 800, 530);

                            sb.Draw(Textures.winterBackground, Vector2.Zero, Color.White);
                            sb.Draw(Textures.winter_ground_texture, new Vector2(0, 405), Color.White);
                            sb.Draw(Textures.winter_ground_texture, new Vector2(790, 405), Color.White);
                            sb.Draw(Textures.hitbox, new Rectangle(0, 0, 1100, 550), Color.Black * 0.5f);
                            scoreDisplay.Draw(sb, Text_Game["congrats"], new Vector2(50, 100), Color.Black, "42");
                            scoreDisplay.Draw(sb, Text_Game["finalScore"] + score, new Vector2(50, 150), Color.Black, "osef");
                            scoreDisplay.Draw(sb, Text_Game["finalBonus"] + nb_nuts, new Vector2(50, 200), Color.Black, "osef");
                            scoreDisplay.Draw(sb, Text_Game["total"] + (score + nb_nuts*1.5), new Vector2(50, 250), Color.Black, "42");
                            scoreDisplay.Draw(sb, Text_Game["space"], new Vector2(70, 300), Color.Black, "osef");
                            sb.End();
                            break;
                    }

                }
                else if (help)
                {
                    switch (this.selected)
                    {
                        case gameState.AutumnLevel:
                            sb.Begin();
                            Rectangle rec = new Rectangle(0, 0, 800, 530);

                            sb.Draw(Textures.background, Vector2.Zero, Color.White);
                            sb.Draw(Textures.autumn_ground_texture, new Vector2(0, 405), Color.White);
                            sb.Draw(Textures.autumn_ground_texture, new Vector2(790, 405), Color.White);
                            sb.Draw(Textures.hitbox, new Rectangle(0, 0, 1100, 550), Color.Black * 0.5f);
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine1"], new Vector2(190, 100), Color.White, "help");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine2"], new Vector2(190, 130), Color.White, "help");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine3"], new Vector2(190, 160), Color.White, "help");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine4"], new Vector2(190, 200), Color.White, "help");
                            sb.End();
                            break;

                        case gameState.WinterLevel:
                            sb.Begin();
                            rec = new Rectangle(0, 0, 800, 530);

                            sb.Draw(Textures.winterBackground, Vector2.Zero, Color.White);
                            sb.Draw(Textures.winter_ground_texture, new Vector2(0, 405), Color.White);
                            sb.Draw(Textures.winter_ground_texture, new Vector2(790, 405), Color.White);
                            sb.Draw(Textures.hitbox, new Rectangle(0, 0, 1100, 550), Color.Black * 0.5f);
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine1"], new Vector2(190, 100), Color.White, "help");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine2"], new Vector2(190, 130), Color.White, "help");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine3"], new Vector2(190, 160), Color.White, "help");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelpLine4"], new Vector2(190, 200), Color.White, "help");
                            sb.End();
                            break;
                    }
                }
                else
                {
                    switch (this.selected)
                    {
                        case gameState.AutumnLevel:
                            sb.Begin();
                            // Makes the background move slower than the camera to create an effect of depth.
                            sb.Draw(Textures.background, new Vector2(cameraClass.Position.X / 3 - 1, -43), Color.White * 0.9f);
                            sb.End();

                            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                            tree.Draw(sb, new Vector2(-100, 0));
                            tree.Draw(sb, new Vector2(500, 0));
                            tree.Draw(sb, new Vector2(400, 0));
                            tree.Draw(sb, new Vector2(900, 0));
                            tree.Draw(sb, new Vector2(1050, 0));
                            tree.Draw(sb, new Vector2(1400, 0));
                            tree.Draw(sb, new Vector2(1800, 0));
                            tree.Draw(sb, new Vector2(2200, 0));
                            tree.Draw(sb, new Vector2(2400, 0));
                            tree.Draw(sb, new Vector2(3000, 0));
                            tree.Draw(sb, new Vector2(3400, 0));
                            tree.Draw(sb, new Vector2(3900, 0));
                            tree.Draw(sb, new Vector2(4050, 0));
                            tree.Draw(sb, new Vector2(4900, 0));
                            //tree_autumn_exit.Draw(sb, new Vector2(5000, 0));

                            // Draw ground image
                            for (int truc = 0; truc < 9; truc++)
                                Ground.Draw(sb, new Vector2(truc * Textures.autumn_ground_texture.Width, 355));

                            // Draw the platforms
                            foreach (Rectangle top in blocks)
                                sb.Draw(Textures.buche_texture, top, Color.White);

                            // Draw the objects
                            foreach (Rectangle dessine in objects)
                                sb.Draw(Textures.acorn_texture, dessine, Color.White);

                            //draw bomb
                            foreach (Bomb dessine in bomb)
                                dessine.Draw(sb, gameTime);

                            if (playerActivate)
                                (execute as Perso).Draw(sb, gameTime); /* Should be execute in the Drawable class */

                            // Draw IA characters
                            foreach (Perso iathings in iaPerso)
                                iathings.Draw(sb, gameTime);

                            //------------------------------------------------------------------
                            // ES 15APR14
                            // Draw foreground tree so that squirrel appears to enter the hole
                            //------------------------------------------------------------------
                            tree_autumn_entrance_inside.Draw(sb, new Vector2(-100, 0));
                            //tree_autumn_exit_inside.Draw(sb,new Vector2(5000,0));
                            

                            sb.End();

                            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                            Bloodscreen(gameTime, sb, cameraClass.Position);
                            sb.End();
                            sb.Begin();
                            sb.Draw(Textures.hitbox, new Rectangle(0, 420, 810, 100), Color.DimGray);//draw panel life + bonus + help + pause

                            scoreDisplay.Draw(sb, Text_Game["_gamescore"] + " : " + score, new Vector2(137, 487), Color.Black, "normal");

                            // this display the number of nuts that the perso has. 
                            scoreDisplay.Draw(sb, Text_Game["_gamebonus"] + " : " + nb_nuts, new Vector2(17, 487), Color.Black, "normal");

                            //draw text health
                            scoreDisplay.Draw(sb, Text_Game["_gameHealth"] + " :  " + Health + "/20", new Vector2(63, 425), Color.Black, "normal");

                            //help text
                            scoreDisplay.Draw(sb, Text_Game["_gamePause"], new Vector2(530, 440), Color.Black, "normal");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelp"], new Vector2(530, 468), Color.Black, "normal");

                            //Negative health
                            sb.Draw(Textures.healthBar_texture, new Rectangle(0,
                                450, Textures.healthBar_texture.Width, 28), new Rectangle(0, 31,
                                Textures.healthBar_texture.Width, 28), Color.Gray);
                            //health left
                            sb.Draw(Textures.healthBar_texture, new Rectangle(0,
                                450, (int)(Textures.healthBar_texture.Width * (double)Health / 20f),
                                28), new Rectangle(0, 31, Textures.healthBar_texture.Width, 44), Color.Red);
                            //healthBar bounds
                            sb.Draw(Textures.healthBar_texture, new Rectangle(0,
                                450, Textures.healthBar_texture.Width, 28), new Rectangle(0, 0,
                                Textures.healthBar_texture.Width, 28), Color.White);
                            
                            sb.End();

                            if (!playerActivate && timeElaspedGameOver > 1500)
                                GameOverAnimation(sb, transparencyAnimation);
                            break;

                        case gameState.WinterLevel:
                            sb.Begin();
                            // Makes the background move slower than the camera to create an effect of depth.
                            sb.Draw(Textures.winterBackground, new Vector2(cameraClass.Position.X / 3 - 1, -43), Color.White * 0.9f);
                            sb.End();

                            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                            tree.Draw(sb, new Vector2(-100, 10));
                            tree.Draw(sb, new Vector2(500,  10));
                            tree.Draw(sb, new Vector2(400,  10));
                            tree.Draw(sb, new Vector2(900,  10));
                            tree.Draw(sb, new Vector2(1050, 10));
                            tree.Draw(sb, new Vector2(1400, 10));
                            tree.Draw(sb, new Vector2(1800, 10));
                            tree.Draw(sb, new Vector2(2200, 10));
                            tree.Draw(sb, new Vector2(2400, 10));
                            tree.Draw(sb, new Vector2(3000, 10));
                            tree.Draw(sb, new Vector2(3400, 10));
                            tree.Draw(sb, new Vector2(3900, 10));
                            tree.Draw(sb, new Vector2(4050, 10));
                            tree.Draw(sb, new Vector2(4900, 10));

                            // Draw ground image
                            for (int truc = 0; truc < 9; truc++)
                                Ground.Draw(sb, new Vector2(truc * Textures.winter_ground_texture.Width, 345));

                            // Draw the platforms
                            foreach (Rectangle top in blocks)
                                sb.Draw(Textures.buche_texture_winter, top, Color.White);

                            // Draw the objects
                            foreach (Rectangle dessine in objects)
                                sb.Draw(Textures.acorn_texture, dessine, Color.White);

                            //draw bomb
                            foreach (Bomb dessine in bomb)
                                dessine.Draw(sb, gameTime);

                            if (playerActivate)
                                (execute as Perso).Draw(sb, gameTime);

                            // Draw IA characters
                            foreach (Perso iathings in iaPerso)
                                iathings.Draw(sb, gameTime);

                            tree_winter_entrance_inside.Draw(sb, new Vector2(-100, 0));

                            sb.End();

                            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                            Bloodscreen(gameTime, sb, cameraClass.Position);
                            sb.End();
                            sb.Begin();
                            sb.Draw(Textures.hitbox, new Rectangle(0, 420, 810, 100), Color.DimGray);//draw panel life + bonus + help + pause

                            scoreDisplay.Draw(sb, Text_Game["_gamescore"] + " : " + score, new Vector2(137, 487), Color.Black, "normal");

                            // this display the number of nuts that the perso has. 
                            scoreDisplay.Draw(sb, Text_Game["_gamebonus"] + " : " + nb_nuts, new Vector2(17, 487), Color.Black, "normal");

                            //draw text health
                            scoreDisplay.Draw(sb, Text_Game["_gameHealth"] + " :  " + Health + "/20", new Vector2(63, 425), Color.Black, "normal");

                            //help text
                            scoreDisplay.Draw(sb, Text_Game["_gamePause"], new Vector2(530, 440), Color.Black, "normal");
                            scoreDisplay.Draw(sb, Text_Game["_gameHelp"], new Vector2(530, 468), Color.Black, "normal");

                            //Negative health
                            sb.Draw(Textures.healthBar_texture, new Rectangle(0,
                                450, Textures.healthBar_texture.Width, 28), new Rectangle(0, 31,
                                Textures.healthBar_texture.Width, 28), Color.Gray);
                            //health left
                            sb.Draw(Textures.healthBar_texture, new Rectangle(0,
                                450, (int)(Textures.healthBar_texture.Width * (double)Health / 20f),
                                28), new Rectangle(0, 31, Textures.healthBar_texture.Width, 44), Color.Red);
                            //healthBar bounds
                            sb.Draw(Textures.healthBar_texture, new Rectangle(0,
                                450, Textures.healthBar_texture.Width, 28), new Rectangle(0, 0,
                                Textures.healthBar_texture.Width, 28), Color.White);

                            

                            sb.End();
                            if (!playerActivate && timeElaspedGameOver > 1500)
                                GameOverAnimation(sb, transparencyAnimation);
                            break;
                    }
                }
            }
            else // draw splashscreen
            {
                sb.Begin();
                Drawable.vidTexture = vidPlayer.GetTexture();
                sb.Draw(Drawable.vidTexture, vidRectangle, Color.White);
                sb.End();
            }

            if (Developpermode)
            {
                sb.Begin();
                Drawable developper = new Drawable(drawable_type.font);
                developper.Draw(sb, "DEVELOPPER MODE", new Vector2(50, 20), Color.Black, "normal");
                if (developperCoord)
                {
                    sb.Draw(Textures.hitbox, new Rectangle((int)developperXMouse, 0, 2, 1200), Color.Red * 0.5f);
                    sb.Draw(Textures.hitbox, new Rectangle(0, (int)developperYMouse, 1200, 2), Color.Red * 0.5f);
                    developper.Draw(sb, "Coord. X : " + (int)developperXMouse + " Y : " + (int)developperYMouse, new Vector2(250, 20), Color.Black, "normal");
                }
                sb.End();
            }
        }

        public void Bloodscreen(GameTime gameTime, SpriteBatch sb, Vector2 camera)
        {

            if (drawBloodScreen)
            {
                elapsedTimeBloodScreen += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                cameraClass.shake = true;

                if (elapsedTimeBloodScreen < 50)
                {
                    int positionX = (int)(execute as Perso).positionPerso.X - 400;
                    float correction = 0;
                    if (positionX + 400 >= 5000)
                        correction = 600;
                    sb.Draw(Textures.hitbox, new Rectangle(positionX - 20 - (int)correction, -20, 1800, 650), Color.Red * 0.5f);
                }
                else
                {
                    elapsedTimeBloodScreen = 0;
                    drawBloodScreen = false;
                    cameraClass.shake = false;
                }
            }

        }


        private void HandleSplashScreen(KeyboardState keyboardstate, MouseState mouse1)
        {
            if (vidPlayer.State == MediaState.Stopped || mouse1.LeftButton == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter))
            {
                vidPlayer.Stop();
                this.curGameMode = instances_type.Menu;
                this.selected = 0;
                Execute();
                Textures.openingSound_Effect1.Play();
            }
        }
    }
}
