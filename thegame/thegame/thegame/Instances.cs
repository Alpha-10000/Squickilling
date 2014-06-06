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
        MultiplayerLoginRegister
    }

    public class Instances
    {
        public Game1 game { get; private set; }


        MouseState OldMouse;
        MouseState mouse1;

        public instances_type curGameMode { get; set; }        // Current game mode.
        public object execute { get; private set; }            // Current activ object (Menu / Perso) 
        public gameState selected { get; private set; }              // Selected menu page.
        public SoundEffect sound { get; private set; }



        private Camera cameraClass = new Camera();


        private SoundEffectInstance instancesound;
        private SoundEffectInstance instancesoundMenu;//sound for the menu
        public static Rectangle vidRectangle;
        public static VideoPlayer vidPlayer, vidPlayer2;



        private bool developpermap = false;


        private bool Fullscreen;        // Set to true to switch to fullscreen
        private bool SoundIs;       // Set to true to switch the sound (on / off)
        private Drawable tree;
      
        private int developperXMouse;
        private int  developperYMouse;
       

        public Vector2 cameraPos = Vector2.Zero;

        private MultiplayerLogin multiplayerloginform;

      

        MouseState mouse = Mouse.GetState();

        public bool Developpermode = false;//this is just for us. Activate the developper mode

    
        private bool developperCoord = false;

        public static ParticleComponent particleComponent;

        private Map thecurrentmap;




        public Instances(Game1 game)
        {
            /* LANGUAGE PAR DÉFAUT AU CHARGEMENT */
            this.curGameMode = instances_type.Menu;
            this.selected = 0;

            instancesound = Textures.gameSound_Effect.CreateInstance();
            instancesound.IsLooped = true;

            instancesoundMenu = Textures.gameSound_EffectMenu.CreateInstance();
            instancesoundMenu.IsLooped = true;


            /* DEFAUT LOADING  : EVERYTHING THAT HAS TO LOAD BY DEFAULT */

            this.game = game;

            SoundIs = true;
            this.selected = gameState.SplashScreen;
            this.Execute();
        }


        public void UpdateByKey(GameTime gametime)
        {

            OldMouse = mouse1;
            mouse1 = Mouse.GetState();

            Inputs.upDate();

           

                if (curGameMode == instances_type.Menu)// MENU
                {
                    (execute as Menu).Update(gametime, SoundIs);
                }

                if (this.selected == gameState.MainMenu && (execute as Menu).IChooseSomething)
                {
                    switch ((execute as Menu).selected)
                    {
                        case 0:
                            (execute as Menu).MenuBool = false;
                            this.curGameMode = instances_type.Game;
                            instancesoundMenu.Stop();
                            this.selected = gameState.AutumnLevel;
                            Execute();
                            break;
                        case 1:
                            this.selected = gameState.MultiplayerLoginRegister;
                            curGameMode = instances_type.Multiplayer;
                            Execute();
                            break;
                        case 2:
                            this.selected = gameState.OptionMenu;
                            Execute();
                            break;
                        case 3:
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
                                (execute as Menu).tab[1] = Language.Text_Game["_mnuFullscreen"] + " (" + Language.Text_Game["_mnuOn"] + ")"; //fullscreen on
                            else
                                (execute as Menu).tab[1] = Language.Text_Game["_mnuFullscreen"] + " (" + Language.Text_Game["_mnuOff"] + ")"; //fullscreen off
                            (execute as Menu).IChooseSomething = false;
                            break;

                        case 2:
                            //this.selected = 4;
                            SoundIs = !SoundIs;

                            if (SoundIs)
                            {
                                (execute as Menu).tab[2] = Language.Text_Game["_mnuSound"] + " (" + Language.Text_Game["_mnuOn"] + ")"; //sound on
                                instancesoundMenu.Play();
                            }
                            else
                            {
                                (execute as Menu).tab[2] = Language.Text_Game["_mnuSound"] + " (" + Language.Text_Game["_mnuOff"] + ")"; // sound off
                                instancesoundMenu.Stop();
                            }
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
                            Language.change("english");
                            this.selected = 0;
                            Execute();
                            break;
                        case 1:
                            Language.change("french");
                            this.selected = 0;
                            Execute();
                            break;
                        case 2:
                            Language.change("nederlands");
                            this.selected = 0;      // This takes it to the first menu page
                            Execute();
                            break;
                        case 3:
                            this.selected = gameState.OptionMenu;
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
                else if (selected == gameState.SplashScreen) HandleSplashScreen(mouse1);
                else if (selected == gameState.DeveloperMode)
                {
                    cameraPos = (execute as DevelopperMap).cameraPos;
                    (execute as DevelopperMap).UpdateMap(gametime);
                }

                else if (selected == gameState.AutumnLevel || selected == gameState.WinterLevel || selected == gameState.SummerLevel || selected == gameState.SpringLevel)// THIS IS THE GAME 
                {

                    thecurrentmap.Update(gametime, game,ref cameraClass, ref cameraPos, Developpermode);
                    if (thecurrentmap.themapstate == Map.MapState.gobackmenu)
                    {
                        curGameMode = instances_type.Menu;
                        this.selected = gameState.MainMenu;
                        Execute();
                    }
                }
                else if (curGameMode == instances_type.Multiplayer)
                {
                    if (selected == gameState.MultiplayerLoginRegister)
                        multiplayerloginform.Update(gametime);
                    if (multiplayerloginform.mainmenu)
                    {
                        this.selected = gameState.MainMenu;
                        curGameMode = instances_type.Menu;
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
                    instancesound.Play();
                    SoundIs = true;
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
                    developperXMouse = mouse1.X - (int)cameraPos.X;
                    developperYMouse = mouse1.Y;
                    instancesound.Stop();
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

                if (!Developpermode && SoundIs)
                    instancesound.Play();
            
        }

        /* END OF THE GAME CODE */

        public void Execute()
        {
            cameraPos = Vector2.Zero;
            switch (this.selected)
            {
                case 0: /* MAIN MENU */
                    Sound("menu");
                    execute = new Menu(4, "Squickilling");
                    (execute as Menu).AddElements(Language.Text_Game["_mnuPlay"]);//Play
                    (execute as Menu).AddElements(Language.Text_Game["_mnuMultiplayer"]);//Play
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOptions"]);//Options
                    (execute as Menu).AddElements(Language.Text_Game["_mnuExit"]);//Exit game

                    break;
                case gameState.OptionMenu: /* OPTION MENU */
                    Sound("menu");
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
                    Sound("menu");
                    execute = new Menu(4, Language.Text_Game["_mnuLanguage"]);
                    (execute as Menu).activateBackSpace = true;
                    (execute as Menu).AddElements(Language.Text_Game["_mnuEnglish"]);
                    (execute as Menu).AddElements(Language.Text_Game["_mnuFrench"]);
                    (execute as Menu).AddElements(Language.Text_Game["_mnuDutch"]);
                    (execute as Menu).AddElements(Language.Text_Game["_mnuBack"]);//back
                    break;

                case gameState.SoundMenu: /* SOUND MENU */
                    Sound("menu");
                    execute = new Menu(2, Language.Text_Game["_mnuSound"]);//sound
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOn"]);//on
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOff"]);//off

                    break;
                case gameState.FullscreenMenu: /* FULL SCREEN */
                    Sound("menu");
                    execute = new Menu(2, Language.Text_Game["_mnuFullscreen"]);//fullscreen
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOn"]);//on
                    (execute as Menu).AddElements(Language.Text_Game["_mnuOff"]);//off
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

                case gameState.AutumnLevel: /* GAME START */
                    Sound("menu");
                    Sound("Game");
                    thecurrentmap = new Map(selected, ref cameraClass);
                    
                    curGameMode = instances_type.Game;
                    break;

                case gameState.WinterLevel: /* GAME START */
                    Sound("menu");
                    Sound("Game");
                    thecurrentmap = new Map(selected, ref cameraClass);
                    curGameMode = instances_type.Game;
                    break;
                case gameState.MultiplayerLoginRegister:
                    curGameMode = instances_type.Multiplayer;
                    multiplayerloginform = new MultiplayerLogin();
                    break;

                default:
                    break;
            }
        }

        public void Sound(string type)
        {
            instancesound.Stop();
            if (SoundIs)
                if (type == "menu")
                {
                    Textures.buttonSound_Effect.Play();
                    if (instancesoundMenu.State != SoundState.Playing && !Developpermode)
                        instancesoundMenu.Play();
                }
                else
                {
                    instancesound.Play();
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
                    sb.Draw(Textures.hitbox, new Rectangle(180, 452, 15, 10), Color.Gray);
                    sb.Draw(Textures.nut_texture, new Rectangle(250, 452, 10, 10), Color.White);
                    sb.Draw(Textures.hitbox, new Rectangle(280, 452, 15, 10), Color.Green);
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
                    thecurrentmap.Display(sb, gameTime, cameraClass);
                }
                else if(curGameMode == instances_type.SplashScreen) // draw splashscreen
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

        


        private void HandleSplashScreen( MouseState mouse1)
        {
            if (vidPlayer.State == MediaState.Stopped || Inputs.isLMBClick() || Inputs.isKeyRelease(Keys.Enter))
            {
                vidPlayer.Stop();
                this.curGameMode = instances_type.Menu;
                this.selected = 0;
                Execute();
            }
        }
    }
}
