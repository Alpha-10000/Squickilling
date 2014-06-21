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
using X2DPE;
using X2DPE.Helpers;

namespace thegame
{
    //------------------------------------------------------------------
    // Define the game modes.
    //------------------------------------------------------------------
    public enum instances_type
    {
        Game,
        Menu,
        Multi,
        SplashScreen,
        MapDevelopper,
        Multiplayer
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
        SummerLevel,
        MultiplayerLoginRegister,
        MutilplayerDashboard,
        MultiplayerCreateGame,
        MultiplayerJoinGame,
        MultiplayerGame,
        MultiplayerSearchFriends,
        MultiplayerHasJoined,
        MultiplayerGetGamers
    }

    public class Instances
    {
        public Game1 game { get; private set; }


        MouseState OldMouse;
        MouseState mouse1;

        public instances_type curGameMode { get; set; }        // Current game mode.
        public object execute { get; private set; }            // Current activ object (Menu / Perso) 
        public gameState selected { get; private set; }        // Selected menu page.
        //public SoundEffect sound { get; private set; }       // USELESS!! 
        public bool CheckSound;

        private Camera cameraClass = new Camera();


        //OLDSOUND: private SoundEffectInstance instancesound;
        //OLDSOUND: private SoundEffectInstance instancesoundMenu;//sound for the menu
        public static Rectangle vidRectangle;
        public static VideoPlayer vidPlayer, vidPlayer2;

        private SoundEffectInstance CurrentBKSound;               // The currentBKSound Instance
        private string CurrentBKSoundName;                        // The name (Menu/Summer etc

        private bool developpermap = false;


        private bool Fullscreen;        // Set to true to switch to fullscreen
        private bool SoundIs = true;           // Set to true to switch the sound (on / off)
        private Drawable tree;
        public static string language = "";
        private int developperXMouse;
        private int developperYMouse;


        public Vector2 cameraPos = Vector2.Zero;

        private MultiplayerLogin multiplayerloginform;
        private Dashboard createthegame;

        MouseState mouse = Mouse.GetState();

        public bool Developpermode = false;//this is just for us. Activate the developper mode


        private bool developperCoord = false;

        public static ParticleComponent particleComponent;

        private Map thecurrentmap;
        private MapMulti thecurrentmultimap;
        private Create_game create_game;
        private Join_game join_game;

        private MultiMap multimap;
        private Search_friends search_friends;
        private HasJoined hasjoined;
        private GetGamers getgamers;

        private Server server;
        private Client client;

        /* DEVELOPPER OPTION TO BYPASS MULTIPLAYER MENU */
        private bool bypassLoginForm = false;

        Animation blood;
        private bool GoToTheMultiExperiment = false;

        public Instances(Game1 game)
        {

            if (bypassLoginForm)//If by pass enter login credentials.
            {
                Session.NewSession("9", "victor.boissiere@gmail.com", "hellohello", "Victor");//if you want to login with your account ask me your id
            }
            /* LANGAGE PAR DÉFAUT AU CHARGEMENT */
            this.curGameMode = instances_type.Menu;
            this.selected = 0;

            //OLDSOUND: instancesound = Textures.gameSound_EffectAutumn.CreateInstance();
            //OLDSOUND: instancesound.IsLooped = true;

            //OLDSOUND: instancesoundMenu = Textures.gameSound_EffectMenu.CreateInstance();
            //OLDSOUND: instancesoundMenu.IsLooped = true;

            blood = new Animation(new Vector2(100, 100), new Vector2(2, 3));
            blood.AnimationSprite = Textures.bloodEmitter;

            /* DEFAULT LOADING  : EVERYTHING THAT HAS TO LOAD BY DEFAULT */

            this.game = game;
            CheckSound = false;
            SoundIs = true;
            this.selected = gameState.SplashScreen;
            this.Execute();
        }


        public void UpdateByKey(GameTime gametime)
        {

            OldMouse = mouse1;
            mouse1 = Mouse.GetState();

            Inputs.upDate();
            if (curGameMode == instances_type.Game)
                if (thecurrentmap.themapstate == Map.MapState.endlevel && thecurrentmap.thegamestate == gameState.SummerLevel)
                {
                    if (Inputs.isKeyRelease(Keys.Space))
                    {
                        CurrentBKSound.Stop();
                        this.selected = gameState.SplashScreen;
                        Execute();
                    }
                }

            if (Session.session_isset)
                Session.update(gametime);

            if (this.selected == gameState.MainMenu && !CheckSound)
            {
                SoundIs = true;
                //OLDSOUND: instancesound.Stop();
                //OLDSOUND: instancesoundMenu.Play();
            }

            if (curGameMode == instances_type.Menu)// MENU
            {
                (execute as Menu).Update(gametime, SoundIs);
            }

            if (this.selected == gameState.MainMenu && (execute as Menu).IChooseSomething)
            {
                switch ((execute as Menu).selected)
                {
                    case 0:             // Jouer
                        (execute as Menu).MenuBool = false;
                        this.curGameMode = instances_type.Game;
                        //OLDSOUND: instancesoundMenu.Stop();
                        this.selected = gameState.AutumnLevel;
                        //PlayBackGroundSound("Autumn");
                        Execute();
                        break;
                    case 1:             // Multi
                        if (Session.session_isset || bypassLoginForm)
                        {
                            selected = gameState.MutilplayerDashboard;
                            curGameMode = instances_type.Multiplayer;
                            Execute();
                        }
                        else if (GoToTheMultiExperiment)
                        {
                            selected = gameState.MultiplayerGame;
                            curGameMode = instances_type.Multiplayer;
                            Execute();
                        }
                        else
                        {
                            this.selected = gameState.MultiplayerLoginRegister;
                            curGameMode = instances_type.Multiplayer;
                            Execute();
                        }
                        break;
                    case 2:             // Options
                        this.selected = gameState.OptionMenu;
                        Execute();
                        break;
                    case 3:             // Quitter
                        game.Exit();
                        break;
                    default:
                        break;
                }
            }

            //-------------------------------------------
            // OPTION PANEL
            //-------------------------------------------
            else if (selected == gameState.OptionMenu && (execute as Menu).IChooseSomething)
            {
                switch ((execute as Menu).selected)
                {
                    case 0:     // Language
                        this.selected = gameState.LanguageMenu;
                        Execute();
                        break;
                    case 1:     // Fullscreen
                        Fullscreen = !Fullscreen;       // Toggle between fullscreen and window

                        Game1.graphics.ToggleFullScreen();
                        //Game1.graphics.IsFullScreen = !Game1.graphics.IsFullScreen;
                        //Game1.graphics.ApplyChanges();
                        if (Fullscreen)
                        {
                            (execute as Menu).tab[1] = Language.Text_Game["_mnuFullscreen"] + " (" + Language.Text_Game["_mnuOn"] + ")"; //fullscreen on
                        }
                        else
                        {
                            (execute as Menu).tab[1] = Language.Text_Game["_mnuFullscreen"] + " (" + Language.Text_Game["_mnuOff"] + ")"; //fullscreen off
                        }
                        (execute as Menu).IChooseSomething = false;
                        break;

                    case 2:     // Sound
                        SoundIs = !SoundIs;
                        CheckSound = !CheckSound;
                        if (SoundIs)
                        {
                            (execute as Menu).tab[2] = Language.Text_Game["_mnuSound"] + " (" + Language.Text_Game["_mnuOn"] + ")"; //sound on
                            //OLDSOUND: instancesoundMenu.Play();
                        }
                        else
                        {
                            (execute as Menu).tab[2] = Language.Text_Game["_mnuSound"] + " (" + Language.Text_Game["_mnuOff"] + ")"; // sound off
                            //OLDSOUND: instancesoundMenu.Stop();
                        }
                        (execute as Menu).IChooseSomething = false;
                        PlayBackGroundSound("Menu");
                        break;
                    case 3:     // Back to MainMenu Panel
                        this.selected = gameState.MainMenu;
                        Execute();
                        break;
                    default:
                        break;
                }
            }

            //-------------------------------------------
            // LANGUAGE SETTINGS
            //-------------------------------------------
            else if (selected == gameState.LanguageMenu && (execute as Menu).IChooseSomething)
            {

                switch ((execute as Menu).selected)
                {
                    case 0:
                        Language.change("english");
                        language = "english";
                        this.selected = gameState.MainMenu;      // => Shortcut to MenuMain Page
                        Execute();
                        break;
                    case 1:
                        Language.change("french");
                        language = "french";
                        this.selected = gameState.MainMenu;
                        Execute();
                        break;
                    case 2:
                        Language.change("nederlands");
                        this.selected = gameState.MainMenu;
                        Execute();
                        break;
                    case 3:                                      // Back to OPTION PANEL
                        this.selected = gameState.OptionMenu;
                        Execute();
                        break;
                    default:
                        break;
                }
            }

            // SOUND SETTING
            else if (selected == gameState.SoundMenu && (execute as Menu).IChooseSomething)
            {
                //HandleSoundSetting();
                PlayBackGroundSound("Menu");
            }
            // START SCREEN HANDLER    
            else if (selected == gameState.SplashScreen) HandleSplashScreen();
            else if (selected == gameState.DeveloperMode)
            {
                cameraPos = (execute as DevelopperMap).cameraPos;
                (execute as DevelopperMap).UpdateMap(gametime);
            }

            else if (curGameMode == instances_type.Game)// THIS IS THE GAME 
            {

                thecurrentmap.Update(gametime, game, ref cameraClass, ref cameraPos, Developpermode, ref SoundIs);
                PlayBackGroundSound(thecurrentmap.BackgroundSoundName);
                if (thecurrentmap.themapstate == Map.MapState.gobackmenu)
                {
                    curGameMode = instances_type.Menu;
                    this.selected = gameState.MainMenu;
                    PlayBackGroundSound("Menu");
                    Execute();
                }
            }
            else if (curGameMode == instances_type.Multi)// Multi
            {

                thecurrentmultimap.Update(gametime, game, ref cameraClass, ref cameraPos, Developpermode);
                if (thecurrentmultimap.themapstate == MapMulti.MapState.gobackmenu)
                {
                    curGameMode = instances_type.Menu;
                    this.selected = gameState.MainMenu;
                    Execute();
                }
            }
            else if (selected == gameState.MultiplayerLoginRegister)
            {

                if (selected == gameState.MultiplayerLoginRegister)
                    multiplayerloginform.Update(gametime);
                if (multiplayerloginform.mainmenu)
                {
                    this.selected = gameState.MainMenu;
                    curGameMode = instances_type.Menu;
                    Execute();
                }
                if (multiplayerloginform.session_isset)
                {
                    Session.NewSession(multiplayerloginform.session_id, multiplayerloginform.session_email, multiplayerloginform.session_password, multiplayerloginform.session_name);
                    selected = gameState.MutilplayerDashboard;
                    Execute();
                }
            }
            else if (selected == gameState.MutilplayerDashboard)
            {
                if (createthegame.mainmenu)
                {
                    this.selected = gameState.MainMenu;
                    curGameMode = instances_type.Menu;
                    Execute();
                }
                createthegame.Update(gametime);
                if (createthegame.Create_game)
                {
                    selected = gameState.MultiplayerCreateGame;
                    Execute();
                }
                if (createthegame.Join_game)
                {
                    selected = gameState.MultiplayerJoinGame;
                    Execute();
                }
                if (createthegame.searchFriendButton.Clicked)
                {
                    selected = gameState.MultiplayerSearchFriends;
                    Execute();
                }
            }
            else if (selected == gameState.MultiplayerCreateGame)
            {
                create_game.Update(gametime);
                if (create_game.goback)
                {
                    this.selected = gameState.MutilplayerDashboard;
                    Execute();
                }
                if (create_game.PlayerToGame)
                {
                    this.selected = gameState.MultiplayerGetGamers;
                    Execute();
                }
            }
            else if (selected == gameState.MultiplayerJoinGame)
            {
                join_game.Update(gametime);
                if (join_game.goback)
                {
                    this.selected = gameState.MutilplayerDashboard;
                    Execute();
                }
                if (join_game.HasJoined)
                {
                    this.selected = gameState.MultiplayerHasJoined;
                    Execute();
                }
              
            }
            else if (selected == gameState.MultiplayerGame)
            {
                multimap.Update(gametime);
            }
            else if (selected == gameState.MultiplayerSearchFriends)
            {
                search_friends.Update(gametime);
                if (search_friends.goback)
                {
                    this.selected = gameState.MutilplayerDashboard;
                    Execute();
                }
            }
            else if (selected == gameState.MultiplayerHasJoined)
            {
                hasjoined.Update(gametime);
                if (hasjoined.goback)
                {
                    this.selected = gameState.MultiplayerJoinGame;
                    Execute();
                }
                if (hasjoined.thereisone)
                {
                    this.selected = gameState.MultiplayerGetGamers;
                    Execute();
                }
                
            }
            else if (selected == gameState.MultiplayerGetGamers)
            {
                getgamers.Update(gametime);
            }

            Keys[] getkey = Keyboard.GetState().GetPressedKeys();

            //-------------------------------------------
            // ACTIVATE DEVELOPER MODE BY PRESSING THE WORD TEAM. SAME TIME
            //-------------------------------------------
            if (getkey.Contains(Keys.D) && getkey.Contains(Keys.E) && getkey.Contains(Keys.V))
                Developpermode = true;

            if (getkey.Contains(Keys.N) && getkey.Contains(Keys.O) && Developpermode)
            {
                Developpermode = false;
                if (SoundIs) CurrentBKSound.Volume = 1;
                else CurrentBKSound.Volume = 0;
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
                CurrentBKSound.Volume = 0;
                developperXMouse = mouse1.X - (int)cameraPos.X;
                developperYMouse = mouse1.Y;
                //OLDSOUND: instancesound.Stop();
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
                    PlayBackGroundSound("Menu");
                    execute = new Menu(4, "Squickilling");
                    (execute as Menu).AddElements(Language.Text_Game["_mnuPlay"]);//Play
                    (execute as Menu).AddElements(Language.Text_Game["_mnuMultiplayer"]);//Play
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOptions"]);//Options
                    (execute as Menu).AddElements(Language.Text_Game["_mnuExit"]);//Exit game

                    break;
                case gameState.OptionMenu: /* OPTION MENU */
                    PlayBackGroundSound("Menu");
                    execute = new Menu(4, Language.Text_Game["_mnuOptions"]);//options
                    (execute as Menu).activateBackSpace = true;
                    (execute as Menu).AddElements(Language.Text_Game["_mnuLanguage"]);//language 

                    if (Fullscreen)
                        (execute as Menu).AddElements(Language.Text_Game["_mnuFullscreen"] + " (" + Language.Text_Game["_mnuOn"] + ")"); //fullscreen on
                    else
                        (execute as Menu).AddElements(Language.Text_Game["_mnuFullscreen"] + " (" + Language.Text_Game["_mnuOff"] + ")"); //fullscreen off

                    if (SoundIs)
                        (execute as Menu).AddElements(Language.Text_Game["_mnuSound"] + " (" + Language.Text_Game["_mnuOn"] + ")"); //sound on
                    else
                        (execute as Menu).AddElements((Language.Text_Game["_mnuSound"] + " (" + Language.Text_Game["_mnuOff"] + ")")); // sound off
                    (execute as Menu).AddElements(Language.Text_Game["_mnuBack"]);//back

                    break;

                case gameState.LanguageMenu: /* Select language */
                    PlayBackGroundSound("Menu");
                    execute = new Menu(4, Language.Text_Game["_mnuLanguage"]);
                    (execute as Menu).activateBackSpace = true;
                    (execute as Menu).AddElements(Language.Text_Game["_mnuEnglish"]);
                    (execute as Menu).AddElements(Language.Text_Game["_mnuFrench"]);
                    (execute as Menu).AddElements(Language.Text_Game["_mnuDutch"]);
                    (execute as Menu).AddElements(Language.Text_Game["_mnuBack"]);//back
                    break;

                case gameState.SoundMenu: /* SOUND MENU */
                    PlayBackGroundSound("Menu");
                    execute = new Menu(2, Language.Text_Game["_mnuSound"]);//sound
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOn"]);//on
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOff"]);//off

                    break;
                case gameState.FullscreenMenu: /* FULL SCREEN */
                    PlayBackGroundSound("Menu");
                    execute = new Menu(2, Language.Text_Game["_mnuFullscreen"]);//fullscreen
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOn"]);//on
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOff"]);//off
                    break;

                case gameState.SplashScreen: /* INTRODUCTION : SPLASHSCREEN */
                    this.curGameMode = instances_type.SplashScreen;
                    vidPlayer = new VideoPlayer();
                    vidRectangle = new Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
                    if (language == "")
                        vidPlayer.Play(Textures.vid);
                    else if (language == "french")
                        vidPlayer.Play(Textures.credits_fr);
                    else vidPlayer.Play(Textures.credits_en);
                    break;
                case gameState.DeveloperMode:
                    this.curGameMode = instances_type.MapDevelopper;
                    execute = new DevelopperMap(45, 15);
                    tree = new Drawable(drawable_type.tree);
                    break;

                case gameState.AutumnLevel: /* GAME START */
                    if (curGameMode == instances_type.Multi)
                        thecurrentmultimap = new MapMulti(selected, ref cameraClass, SoundIs);
                    else
                    {
                        thecurrentmap = new Map(selected, ref cameraClass);
                        PlayBackGroundSound("Autumn");
                        curGameMode = instances_type.Game;
                    }
                    break;

                case gameState.WinterLevel: /* GAME START */
                    PlayBackGroundSound("Winter");
                    if (curGameMode == instances_type.Multi)
                        thecurrentmultimap = new MapMulti(selected, ref cameraClass, SoundIs);
                    else
                    {
                        thecurrentmap = new Map(selected, ref cameraClass);
                        curGameMode = instances_type.Game;
                    }
                    break;
                case gameState.MultiplayerLoginRegister:
                    curGameMode = instances_type.Multiplayer;
                    multiplayerloginform = new MultiplayerLogin();
                    break;
                case gameState.MutilplayerDashboard:
                    createthegame = new Dashboard();
                    break;
                case gameState.MultiplayerCreateGame:
                    create_game = new Create_game();
                    break;
                case gameState.MultiplayerJoinGame:
                    join_game = new Join_game();
                    break;
                case gameState.MultiplayerGame:
                    multimap = new MultiMap();
                    break;
                case gameState.MultiplayerSearchFriends:
                    search_friends = new Search_friends();
                    break;
                case gameState.MultiplayerHasJoined:
                    hasjoined = new HasJoined();
                    break;
                case gameState.MultiplayerGetGamers:
                    getgamers = new GetGamers();
                    break;
                default:
                    break;
            }
        }

        public void Sound(string type)
        {
            //OLDSOUND: instancesound.Stop();
            if (SoundIs)
                if (type == "menu")
                {
                    Textures.buttonSound_Effect.Play();
                    //OLDSOUND: if (instancesoundMenu.State != SoundState.Playing && !Developpermode)
                    //OLDSOUND: instancesoundMenu.Play();
                }
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

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                blood.Update(gameTime);
                sb.Begin();
                blood.Draw(sb);
                sb.End();
            }

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
                sb.Draw(Textures.autumnBackground, new Vector2(cameraClass.Position.X / 3 - 1, -43), Color.White * 0.9f);
                sb.End();

                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                tree.Draw(sb, new Vector2(-100, 0));
                (execute as DevelopperMap).Display(sb, gameTime);
                sb.End();
                sb.Begin();
                sb.Draw(Textures.buche_texture, new Rectangle(22, 452, Textures.buche_texture.Width, Textures.buche_texture.Height), Color.White);
                sb.Draw(Textures.eraser, new Rectangle(722, 452, Textures.eraser.Width, Textures.eraser.Height), Color.White);
                sb.Draw(Textures.mine_gray, new Rectangle(180, 452, Textures.mine_gray.Width, Textures.mine_gray.Height), Color.White);
                sb.Draw(Textures.acorn_texture, new Rectangle(250, 452, 15, 15), Color.White);
                sb.Draw(Textures.medecine, new Rectangle(280, 452, Textures.medecine.Width, Textures.medecine.Height), Color.White);
                Drawable info = new Drawable(drawable_type.font);
                Drawable ecri = new Drawable(drawable_type.font);
                ecri.Draw(sb, "Send by email to ", new Vector2(250, 470), Color.Black, "normal");
                ecri.Draw(sb, "Alpha", new Vector2(409, 470), Color.Black, "normal");
                ecri.Draw(sb, "Elise", new Vector2(470, 470), Color.Black, "normal");
                ecri.Draw(sb, "Thibault", new Vector2(535, 470), Color.Black, "normal");
                ecri.Draw(sb, "Victor", new Vector2(614, 470), Color.Black, "normal");
                info.Draw(sb, "S: show grid.      H: hide grid.    Right button to unselect", new Vector2(25, 494), Color.Black, "normal");
                sb.End();


            }
            else if (curGameMode == instances_type.Game)
            {
                thecurrentmap.Display(sb, gameTime, cameraClass, SoundIs);
            }
            else if (curGameMode == instances_type.Multi)
            {
                thecurrentmultimap.Display(sb, gameTime, cameraClass);
            }
            else if (curGameMode == instances_type.SplashScreen) // draw splashscreen
            {
                sb.Begin();
                Drawable.vidTexture = vidPlayer.GetTexture();
                sb.Draw(Drawable.vidTexture, vidRectangle, Color.White);
                sb.End();
            }
            else if (curGameMode == instances_type.Multiplayer)
            {
                if (selected == gameState.MultiplayerLoginRegister)
                    multiplayerloginform.Display(sb);
                else if (selected == gameState.MutilplayerDashboard)
                    createthegame.Display(sb);
                else if (selected == gameState.MultiplayerCreateGame)
                    create_game.Display(sb);
                else if (selected == gameState.MultiplayerJoinGame)
                    join_game.Display(sb);
                else if (selected == gameState.MultiplayerGame)
                    multimap.Display(sb);
                else if (selected == gameState.MultiplayerSearchFriends)
                    search_friends.Display(sb);
                else if (selected == gameState.MultiplayerHasJoined)
                    hasjoined.Display(sb);
                else if (selected == gameState.MultiplayerGetGamers)
                    getgamers.Display(sb);

                if (MultiplayerLogin.create_game.Clicked)
                {
                    //this.curGameMode = instances_type.Multi;
                    //this.selected = gameState.AutumnLevel;
                    //Execute();
                    server = new Server();
                }
                if (MultiplayerLogin.join_game.Clicked)
                {
                    //this.curGameMode = instances_type.Multi;
                    //this.selected = gameState.AutumnLevel;
                    //Execute();
                    client = new Client();
                }
                if (server != null)
                    server.Update(ref thecurrentmultimap);
                if (client != null)
                    client.Update(ref thecurrentmultimap);
            }

            if (Developpermode)
            {
                sb.Begin();
                Drawable developper = new Drawable(drawable_type.font);
                developper.Draw(sb, "DEVELOPER MODE", new Vector2(50, 20), Color.Black, "normal");
                //TODO : reactivate
                if (developperCoord)
                {
                    sb.Draw(Textures.hitbox, new Rectangle((int)developperXMouse + (int)cameraPos.X, 0, 2, 1200), Color.Red * 0.5f);
                    sb.Draw(Textures.hitbox, new Rectangle(0, (int)developperYMouse, 1200, 2), Color.Red * 0.5f);
                    developper.Draw(sb, "Coord. X : " + (int)developperXMouse + " Y : " + (int)developperYMouse, new Vector2(250, 20), Color.Black, "normal");
                }
                sb.End();
            }

        }
        // SPLASH SCREEN HANDLER
        private void HandleSplashScreen()
        {
            if (vidPlayer.State == MediaState.Stopped || Inputs.isLMBClick() || Inputs.isKeyRelease(Keys.Enter))
            {
                vidPlayer.Stop();
                this.curGameMode = instances_type.Menu;
                this.selected = 0;

                Execute();
            }
        }
        //-------------------------------------------
        // SOUND SETTINGS
        //-------------------------------------------
        //private void HandleSoundSetting()
        //{
        //    switch ((execute as Menu).selected)
        //    {
        //        case 0:
        //            SoundIs = true;
        //            this.selected = gameState.MainMenu;
        //            Execute();
        //            break;
        //        case 1:
        //            SoundIs = false;
        //            this.selected = gameState.MainMenu;
        //            Execute();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public void PlayBackGroundSound(string SoundToPlay)
        {
            if (SoundToPlay == CurrentBKSoundName)
            {
                if (!SoundIs) CurrentBKSound.Volume = 0;
                else CurrentBKSound.Volume = 1;
                return;             // DO not change anything, keep playing current sound
            }

            // First switch off current background sound
            if (CurrentBKSound != null)
            {
                CurrentBKSound.Stop();
                CurrentBKSound.Dispose();
            }

            // Now select the sound
            switch (SoundToPlay)
            {
                case "Menu":
                    CurrentBKSound = Textures.gameSound_EffectMenu.CreateInstance();
                    CurrentBKSoundName = "Menu";
                    break;
                default:
                    CurrentBKSound = thecurrentmap.BackgroundSound;
                    CurrentBKSoundName = thecurrentmap.BackgroundSoundName;
                    break;
            }


            if (!SoundIs) CurrentBKSound.Volume = 0;
            else CurrentBKSound.Volume = 1;

            // Now Play the sound
            CurrentBKSound.IsLooped = true;
            CurrentBKSound.Play();
            
            
        }
    }
}
