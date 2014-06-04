using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using X2DPE;
using X2DPE.Helpers;

namespace thegame
{

    
    class Map
    {
        public enum MapState
        {
            game,
            pause,
            help,
            gameover,
            endlevel,
            gobackmenu
        }


        private static int[,] autumnTileMap = new int[,]
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
                            {0,0,0,0,0,0,0,4,1,1,1,2,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,2,1,0,0,0,3,1,1,0,0,0,0,0,1,2,2,0,1,0,0,0,0,0,0,0,0,1,2,3,1,0,0,0,0,0,2,0,0,0},
                            {0,0,0,0,0,3,1,0,0,3,0,1,2,0,0,1,0,0,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,1,1,1,0,0,2,0,1,1,0,3,0},
                            {0,0,3,1,1,1,0,0,0,1,1,1,1,1,1,0,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
                            {0,0,1,4,2,2,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,0,0,0,0,0,0,1,2,2,2,3,2,3,3,2},
                        };

        private static int[] autumnAiMap = new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };

        private static int[,] winterTileMap = new int[,]
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

        private static int[] winterAiMap = new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };


        MouseState mouse1;

        public instances_type curGameMode { get; set; }        // Current game mode.
        public object execute { get; private set; }            // Current activ object (Menu / Perso) 
        public gameState selected { get; private set; }              // Selected menu page.
        public SoundEffect sound { get; private set; }

        private bool drawBloodScreen = false;//variable for the bloodscreen
        private float elapsedTimeBloodScreen = 0;

        private Camera cameraClass = new Camera();

        private Drawable scoreDisplay;

        private SoundEffectInstance instancesound;

        public List<Rectangle> blocks;

        public List<Rectangle> tile;
        private List<Projectile> projectiles;
        private List<Rectangle> objects = new List<Rectangle> { };
        private List<Bomb> bomb = new List<Bomb> { };
        private List<Rectangle> medecines = new List<Rectangle>();

        private List<Perso> iaPerso = new List<Perso>();

        private int nb_nuts;
        // TODO: Should score stay here or should it be elsewhere?

        private bool SoundIs;       // Set to true to switch the sound (on / off)
        private Drawable tree;
        private Drawable tree_autumn_entrance;
        private Drawable tree_autumn_entrance_inside;
        private Drawable tree_autumn_exit;
        private Drawable tree_autumn_exit_inside;

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

        MouseState mouse = Mouse.GetState();

        public bool Developpermode = false;//this is just for us. Activate the developper mode

        private float developperXMouse;
        private float developperYMouse;
        private bool developperCoord = false;

        private Language Language = new Language();
        public static ParticleComponent particleComponent;
        Random random;
        Emitter testEmitter2;
        bool snow = false;
        private int score = 0;

        private gameState thegamestate;

        private Perso theperso;

        private Texture2D ground_texture;
        private Texture2D Background;
        Drawable thetree, tree_entrance_inside, Ground;

        public MapState themapstate = MapState.game;

        private PauseMenu pauseMenu = new PauseMenu();

        public Map(gameState thegamestate)
        {
            this.thegamestate = thegamestate;
            this.NewGame();
        }

        private void NewGame()
        {
            int[,] thetile;
            int[] IAtile;
            themapstate = MapState.game;

            switch (thegamestate)
            {
                case gameState.AutumnLevel:
                        ground_texture = Textures.autumn_ground_texture;
                        Background = Textures.autumnBackground;
                        thetile = autumnTileMap;
                        IAtile = autumnAiMap;
                        thetree = new Drawable(drawable_type.tree);
                        tree_entrance_inside = new Drawable(drawable_type.tree_autumn_entrance);
                        Ground = new Drawable(drawable_type.AutumnGround);
                    break;
                default:
                        ground_texture = Textures.winter_ground_texture;
                        Background = Textures.winterBackground;
                        thetile = winterTileMap;
                        IAtile = winterAiMap;
                        thetree = new Drawable(drawable_type.winterTree);
                        tree_entrance_inside = new Drawable(drawable_type.tree_winter_entrance_inside);
                        Ground = new Drawable(drawable_type.WinterGround);
                    break;
            }

             Init_Game();
             objects = new List<Rectangle>();
             iaPerso = new List<Perso>();
           /* IA CHARACTERS */
             for (int x = 0; x < IAtile.Length; x++)
                 if (IAtile[x] == 1)
                   iaPerso.Add(new Perso(new Vector2(x * Textures.buche_texture.Width, 0), CharacType.ia));

           texlis = new List<Texture2D>();
           mapSizeX = thetile.GetLength(1);
           mapSizeY = thetile.GetLength(0);
           blocks = new List<Rectangle>();
           tile = new List<Rectangle>();

           for (int x = 0; x < mapSizeX; x++)
               for (int y = 0; y < mapSizeY; y++)
                   if (thetile[y, x] == 1)
                       blocks.Add(new Rectangle(x * Textures.buche_texture.Width, y * Textures.buche_texture.Height - 95, Textures.buche_texture.Width, Textures.buche_texture.Height));
                   else if (thetile[y, x] == 2)
                       objects.Add(new Rectangle(x * Textures.buche_texture.Width + 50, y * Textures.buche_texture.Height - 86, 10, 10));
                   else if (thetile[y, x] == 3)
                       bomb.Add(new Bomb(new Rectangle(x * Textures.buche_texture.Width + 50, (y == mapSizeY - 1) ? 345 : y * Textures.buche_texture.Height - 73, 15, 10)));
                   else if (thetile[y, x] == 4)
                       medecines.Add(new Rectangle(x * Textures.buche_texture.Width + 50,( (y == mapSizeY - 1) ? 345 : y * Textures.buche_texture.Height - 73) - 7, Textures.medecine.Width, Textures.medecine.Height));

                    theperso = new Perso(new Vector2(200, 0), CharacType.player);
                    tree = new Drawable(drawable_type.tree);
                    tree_autumn_entrance = new Drawable(drawable_type.tree_autumn_entrance);
                    tree_autumn_entrance_inside = new Drawable(drawable_type.tree_autumn_entrance_inside);
                    tree_autumn_exit = new Drawable(drawable_type.tree_autumn_exit);
                    tree_autumn_exit_inside = new Drawable(drawable_type.tree_autumn_exit_inside);
                    debug = new Drawable(drawable_type.font);
                    scoreDisplay = new Drawable(drawable_type.font);

        }

        /* EVERYTHING THAT HAS TO BE INIT AT EACH LEVEL*/
        private void Init_Game()
        {
            score = 0;
            this.nb_nuts = 0;
            this.Health = 20;
            drawBloodScreen = false;
            cameraClass.shake = false;
            bomb = new List<Bomb>();
            random = new Random();
            medecines = new List<Rectangle>();


            if (thegamestate == gameState.WinterLevel)
            {
                testEmitter2 = new Emitter();
                testEmitter2.Active = false;
                testEmitter2.TextureList.Add(Textures.snowdrop);
                testEmitter2.RandomEmissionInterval = new RandomMinMax(64.0d);
                testEmitter2.ParticleLifeTime = 3000;
                testEmitter2.ParticleDirection = new RandomMinMax(170);
                testEmitter2.ParticleSpeed = new RandomMinMax(2.5f);
                testEmitter2.ParticleRotation = new RandomMinMax(0);
                testEmitter2.RotationSpeed = new RandomMinMax(0f);
                testEmitter2.ParticleFader = new ParticleFader(false, true, 800);
                testEmitter2.ParticleScaler = new ParticleScaler(false, 1.0f);
                testEmitter2.Opacity = 255;
                particleComponent.particleEmitterList.Add(testEmitter2);
            }
            
            if (selected == gameState.AutumnLevel)
            {
                instancesound = Textures.gameSound_Effect.CreateInstance();
                instancesound.IsLooped = true;
                snow = false;

            }
            else
            {
                instancesound = Textures.gameSound_EffectWinter.CreateInstance();
                instancesound.IsLooped = true;
                snow = true;
            }

            if (!Developpermode && SoundIs)
                instancesound.Play();
        }


        public void Update(GameTime gametime, Game game, Camera  cameraClass, Vector2 cameraPos)
        {


            if (Inputs.isKeyRelease(Keys.Escape) || Inputs.isKeyRelease(Keys.P)) /* Exit the game */
                themapstate = MapState.pause;

            if (thegamestate == gameState.WinterLevel)
            {
                if (Inputs.isKeyRelease(Keys.S) && Developpermode)
                    snow = !snow;
                if (snow)
                    particleComponent.particleEmitterList[0].Active = !particleComponent.particleEmitterList[0].Active;

                particleComponent.particleEmitterList[0].Active = true;
                Emitter t2 = particleComponent.particleEmitterList[0];
                t2.Position = new Vector2((float)random.NextDouble() * (Game1.graphics.GraphicsDevice.Viewport.Width), 0);
                if (t2.EmittedNewParticle)
                {
                    float f = MathHelper.ToRadians(t2.LastEmittedParticle.Direction + 180);
                    t2.LastEmittedParticle.Rotation = f;
                }
            }



            bool justchange = false;
            if (themapstate == MapState.help && (Inputs.AnyKeyPressed() || Inputs.isLMBClick()))
            {
                themapstate = MapState.game;
                justchange = true;
            }
            
            

            if (theperso.positionPerso.X > 5350)
            {
                themapstate = MapState.endlevel;
                if (Inputs.isKeyRelease(Keys.Space))
                {
                    endLevel = false;
                    thegamestate = gameState.WinterLevel;
                    NewGame();
                    Init_Game();
                }
            }
            else if (themapstate == MapState.game)
            {
                if (Inputs.isKeyRelease(Keys.H) && !justchange)
                    themapstate = MapState.help;

                if (Developpermode)
                    Health = 20;

                for (int j = medecines.Count - 1; j >= 0; j--)
                    if (theperso.hitBoxPerso.Intersects(medecines[j]))
                    {
                        medecines.Remove(medecines[j]);
                        Health += (Health <= 15) ? 5 : (20 - Health);
                    }
                int checkBlood = 0;
                foreach (Perso iathings in iaPerso)
                {
                    if (playerActivate)
                        checkBlood += iathings.TryToKill(ref Health, theperso.hitBoxPerso, SoundIs);
                    iathings.UpdateIA(gametime, blocks, theperso.hitBoxPerso);
                }

                bool touchedByBomb = false;

                /* CHECK IF CHARACTER CROSS A MINE */
                foreach (Bomb checkCrossed in bomb)
                {
                    if (theperso.hitBoxPerso.Intersects(checkCrossed.Object) && !checkCrossed.activateExplosion)
                    {
                        touchedByBomb = true;
                        checkCrossed.activateExplosion = true;
                        drawBloodScreen = true;
                        if (checkCrossed.checkBlood && SoundIs)
                            Textures.gameExplosion_Effect.Play();
                        checkCrossed.BloodOnce(ref Health);
                        break;
                    }
                    if (checkCrossed.activateExplosion)// important to keep the blood screen active until the end of the explosion
                        drawBloodScreen = true;
                }

                if (touchedByBomb)//fait clignoter le perso
                {
                    theperso.PersoHitted = true;
                    theperso.compteurHitted = 0;
                }

                bomb.RemoveAll(x => x.checkIfFinish);//remove bomb when explosion animation is complete

                if (playerActivate)
                {
                    cameraPos = theperso.cameraPos;

                    projectiles = new List<Projectile>();
                    theperso.Update(gametime, blocks, projectiles, objects, ref nb_nuts, Developpermode);//TODO : remove keyboardState and oldkey because class now
                    this.objects = theperso.objects;
                    iaPerso = theperso.CollisionIAProjec(iaPerso, ref score);

                    if (checkBlood > 0)
                    {
                        drawBloodScreen = true;
                        theperso.PersoHitted = true;
                        theperso.compteurHitted = 0;
                    }

                    if (Health <= 0)
                    {
                        Health = 0;
                        playerActivate = drawBloodScreen = false;
                    }
                }
                else
                {
                    timeElaspedGameOver += gametime.ElapsedGameTime.Milliseconds;
                    if (timeElaspedGameOver > 2500)
                    {
                        timeElaspedGameOver = 0;
                        playerActivate = game_over_i = true;
                    }
                    if (timeElaspedGameOver > 1500)
                        transparencyAnimation = (timeElaspedGameOver - 1500) / 1000;
                }


            }
            else if (themapstate == MapState.pause)
            {
                if (thegamestate == gameState.WinterLevel)
                    if (particleComponent.particleEmitterList[0].Active)
                        particleComponent.particleEmitterList[0].Active = false;

                pauseMenu.Update(gametime, SoundIs);
                if (pauseMenu.IChooseSomething) // OPTION PANNEL
                {
                    switch (pauseMenu.selected)
                    {
                        case 0:
                            themapstate = MapState.game;
                            pauseMenu = new PauseMenu();
                            break;
                        case 1:
                            themapstate = MapState.gobackmenu;
                            break;
                        default:
                            game.Exit();
                            break;
                    }
                }

            }
            else if (themapstate == MapState.gameover)
                {
                    if (thegamestate == gameState.WinterLevel)
                    {
                        if (particleComponent.particleEmitterList[0].Active)
                            particleComponent.particleEmitterList[0].Active = false;
                    }
                    if (Inputs.isKeyRelease(Keys.Space))
                    {
                        themapstate = MapState.game;
                        Init_Game();
                        NewGame();
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
                        //TODO : GO BACK TO THE MENU
                    }
                }

                if (Developpermode)
                {
                    SoundIs = false;
                    developperXMouse = mouse1.X;
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
                        // TODO MOVE THAT
                    }
                    if (getkey.Contains(Keys.NumPad2) || getkey.Contains(Keys.D2))
                    {
                        Init_Game();
                        this.curGameMode = instances_type.Game;
                        selected = gameState.WinterLevel;
                        //TODO: MOVE THAT
                    }
                    if (getkey.Contains(Keys.NumPad1) || getkey.Contains(Keys.D1))
                    {
                        Init_Game();
                        try
                        {
                            if (particleComponent.particleEmitterList[0].Active == true)
                                particleComponent.particleEmitterList[0].Active = false;
                        }
                        catch { }
                        thegamestate = gameState.AutumnLevel;
                        Init_Game();
                    }

                }

                

            
        }
        

        public void Display(SpriteBatch sb, GameTime gameTime)
        {
            
         if (themapstate == MapState.gameover && playerActivate)
                    GameOverAnimation(sb, 1);
                if(themapstate == MapState.endlevel || themapstate == MapState.help)
                {
                     sb.Begin();
                     sb.Draw(Background, Vector2.Zero, Color.White);
                     sb.Draw(ground_texture, new Vector2(0, 405), Color.White);
                     sb.Draw(ground_texture, new Vector2(790, 405), Color.White);
                     sb.Draw(Textures.hitbox, new Rectangle(0, 0, 1100, 550), Color.Black * 0.5f);
                     sb.End();
                }
                if (themapstate == MapState.endlevel)
                {
                     sb.Begin();
                     scoreDisplay.Draw(sb, Language.Text_Game["congrats"], new Vector2(50, 100), Color.White, "42");
                     scoreDisplay.Draw(sb, Language.Text_Game["finalScore"] + score, new Vector2(50, 150), Color.White, "osef");
                     scoreDisplay.Draw(sb, Language.Text_Game["finalBonus"] + nb_nuts, new Vector2(50, 200), Color.White, "osef");
                     scoreDisplay.Draw(sb, Language.Text_Game["total"] + (score + nb_nuts * 0.5), new Vector2(50, 250), Color.Red, "42");
                     scoreDisplay.Draw(sb, Language.Text_Game["space"], new Vector2(70, 300), Color.White, "osef");
                     sb.End();
                }
                else if (themapstate == MapState.help)
                {
                    sb.Begin();
                    scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine1"], new Vector2(190, 100), Color.White, "help");
                    scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine2"], new Vector2(190, 130), Color.White, "help");
                    scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine3"], new Vector2(190, 160), Color.White, "help");
                    scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine4"], new Vector2(190, 200), Color.White, "help");
                     sb.End();
                }
                else if(themapstate == MapState.pause)
                    pauseMenu.Display(sb, thegamestate);
                else
                {
                            sb.Begin();
                            // Makes the background move slower than the camera to create an effect of depth.
                            sb.Draw(Background, new Vector2(cameraClass.Position.X / 3 - 1, -43), Color.White * 0.9f);
                            sb.End();

                            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                            tree_autumn_entrance.Draw(sb, new Vector2(-100, 0));
                            thetree.Draw(sb, new Vector2(500, 0));
                            thetree.Draw(sb, new Vector2(400, 0));
                            thetree.Draw(sb, new Vector2(900, 0));
                            thetree.Draw(sb, new Vector2(1050, 0));
                            thetree.Draw(sb, new Vector2(1400, 0));
                            thetree.Draw(sb, new Vector2(1800, 0));
                            thetree.Draw(sb, new Vector2(2200, 0));
                            thetree.Draw(sb, new Vector2(2400, 0));
                            thetree.Draw(sb, new Vector2(3000, 0));
                            thetree.Draw(sb, new Vector2(3400, 0));
                            thetree.Draw(sb, new Vector2(3900, 0));
                            thetree.Draw(sb, new Vector2(4050, 0));
                            thetree.Draw(sb, new Vector2(4900, 0));

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

                            foreach (Rectangle dessine in medecines)
                                sb.Draw(Textures.medecine, dessine, Color.White);

                            if (playerActivate)
                                theperso.Draw(sb, gameTime); /* Should be execute in the Drawable class */

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
                            sb.Draw(Textures.hitbox, new Rectangle(0, 420, Game1.graphics.PreferredBackBufferWidth+40, 120), Color.DimGray);//draw panel life + bonus + help + pause

                            scoreDisplay.Draw(sb, Language.Text_Game["_gamescore"] + " : " + score, new Vector2(137, 487), Color.Black, "normal");

                            // this display the number of nuts that the perso has. 
                            scoreDisplay.Draw(sb, Language.Text_Game["_gamebonus"] + " : " + nb_nuts, new Vector2(17, 487), Color.Black, "normal");

                            //draw text health
                            scoreDisplay.Draw(sb, Language.Text_Game["_gameHealth"] + " :  " + Health + "/20", new Vector2(63, 425), Color.Black, "normal");

                            //help text
                            scoreDisplay.Draw(sb, Language.Text_Game["_gamePause"], new Vector2(530, 440), Color.Black, "normal");
                            scoreDisplay.Draw(sb, Language.Text_Game["_gameHelp"], new Vector2(530, 468), Color.Black, "normal");

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

                
            }
        }

        private void GameOverAnimation(SpriteBatch sb, float transparency)
        {
                    sb.Begin();
                    Rectangle rec = new Rectangle(-4, -4, Game1.graphics.PreferredBackBufferWidth + 50, 530);
                    sb.Draw(Background, new Vector2(-2, -2), Color.White * transparency);
                    sb.Draw(ground_texture, new Vector2(0, 405), Color.White * transparency);
                    sb.Draw(ground_texture, new Vector2(790, 405), Color.White * transparency);

                   
                        sb.Draw(Textures.game_overTexture_en, rec, Color.White * transparency);
                   
                    sb.End();
        }

        private void Bloodscreen(GameTime gameTime, SpriteBatch sb, Vector2 camera)
        {

            if (drawBloodScreen)
            {
                elapsedTimeBloodScreen += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                cameraClass.shake = true;

                if (elapsedTimeBloodScreen < 50)
                {
                    int positionX = (int)theperso.positionPerso.X - 400;
                    float correction = 0;
                    if (positionX + 400 >= 5000)
                        correction = 600;
                    sb.Draw(Textures.hitbox, new Rectangle(positionX - 20 - (int)correction, -20, 1800, 650), Color.Red * 0.5f);
                }
                else
                {
                    elapsedTimeBloodScreen = 0;
                    drawBloodScreen = cameraClass.shake =  false;
                }
            }

        }


    }
}
