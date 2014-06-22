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
    class MapMulti
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

        private static int[] autumnAiMap = new int[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };

        private static int[,] winterTileMap = new int[,]
                     {
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,1,0,0,0,0,0,0,0,0,0,0,2,0,0,0,4,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,3,1,1,1,1,1,1,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,2,1,0,0,0,2,0,0,0,0,2,1,1,2,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0},
                    {0,0,0,0,0,2,0,0,0,3,1,0,3,0,2,3,1,0,0,0,1,0,0,0,0,1,0,1,0,1,1,1,1,0,0,1,0,0,0,0,2,1,0,0,0},
                    {0,0,0,0,3,1,0,2,0,1,0,0,1,1,1,1,1,0,2,1,1,0,3,0,0,0,0,1,0,0,3,2,2,1,0,1,0,0,0,3,1,0,2,3,3},
                    {0,0,0,0,1,1,3,1,1,2,0,1,0,0,0,0,1,0,1,0,0,0,1,1,1,0,0,1,0,0,1,1,2,0,1,1,0,0,2,1,0,0,1,1,1},
                    {0,0,2,1,0,0,1,1,2,3,2,0,0,0,0,2,0,0,3,2,0,3,2,2,2,3,0,1,3,0,0,0,3,0,2,0,0,3,2,0,0,0,3,2,2},
                    };

        private static int[] winterAiMap = new int[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };

        private static int[,] springTileMap = new int[,]
                    {
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,0,2,0,0,0,0,0,0,0,2,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,0,1,1,1,1,1,1,0,0,2,1,0,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    {0,0,3,0,1,0,2,2,0,0,0,1,0,4,0,1,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0},
                    {0,0,1,0,1,0,1,1,0,0,3,0,0,1,0,0,3,0,0,0,0,0,0,0,0,0,4,4,1,0,0,0,1,2,1,0,0,1,1,1,0,0,0,0,0},
                    {0,0,0,2,1,3,0,1,0,0,1,2,0,1,0,2,1,3,0,0,0,0,0,0,3,0,1,1,1,0,0,0,0,1,0,3,1,0,0,0,0,0,0,0,0},
                    {0,0,0,1,1,1,2,0,1,0,0,1,2,0,2,1,0,1,0,0,0,0,0,1,1,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,2,0,0,0,0},
                    {0,0,0,0,1,0,1,0,1,3,0,0,1,0,1,0,0,1,0,0,0,0,2,0,0,1,0,0,0,0,0,1,0,0,0,1,0,0,0,0,1,2,0,0,0},
                    {0,0,1,0,1,0,2,4,1,1,0,0,0,4,0,0,0,1,0,0,0,2,1,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,1,1,2,0,0},
                    {0,0,0,2,1,4,1,1,0,0,0,0,0,1,0,0,0,1,0,0,2,1,1,0,0,0,0,0,0,0,1,4,2,0,0,0,0,0,0,2,1,1,1,2,0},
                    {0,0,0,1,1,0,0,0,0,0,0,1,2,1,2,1,0,1,0,2,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2},
                    {0,0,0,0,1,0,2,2,0,2,0,2,1,1,1,2,0,1,0,1,1,1,1,0,3,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1},
                    };

        private static int[] springAiMap = new int[] { 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0 };

        private static int[,] summerTileMap = new int[,]
                      {
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0},
                        {0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,4,1,0,0,0,0,1,0,0},
                        {0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,0,0},
                        {0,0,0,0,0,0,0,0,1,0,0,1,2,0,0,0,1,1,0,0,1,0,0,1,2,0,0,0,0,0,0,0,0,0,0,1,1,2,0,1,0,0,1,0,0},
                        {0,0,0,0,0,0,0,0,0,2,0,1,2,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,1,1,0,0,0,0,1,0,0,0,1,0,1,0,0},
                        {0,0,0,0,0,0,0,1,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,0,1,0,0,3,0,2,3,4,0,0,1,0,1,0,1,2,0,3,0,0,0},
                        {0,0,0,0,0,0,0,0,1,0,0,1,2,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,1,1,0,2,1,0,1,1,0,0,1,0,2,0,0},
                        {0,0,0,1,0,0,0,1,0,1,0,1,0,1,0,0,1,1,0,0,0,1,0,1,1,1,0,0,2,0,0,0,0,1,0,0,1,0,2,1,2,0,1,0,0},
                        {0,0,0,3,1,0,0,0,0,0,0,1,0,1,0,1,0,0,1,0,0,1,0,1,0,0,0,0,1,0,0,0,0,1,0,0,1,4,0,0,1,0,1,2,0},
                        {0,0,1,0,1,0,0,1,3,0,1,1,0,1,1,0,0,0,0,1,0,1,1,3,0,1,1,2,1,0,0,2,1,2,0,1,1,0,1,2,1,2,1,1,2},
                        {0,0,1,2,1,3,1,0,1,1,1,1,3,0,0,0,0,0,0,0,0,1,4,0,2,1,2,3,1,3,0,2,1,2,0,2,3,2,0,2,1,3,0,2,0},
                       };

        private static int[] summerAiMap = new int[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };


        public instances_type curGameMode { get; set; }        // Current game mode.
        public object execute { get; private set; }            // Current activ object (Menu / Perso) 
        public SoundEffect sound { get; private set; }

        private bool drawBloodScreen = false;//variable for the bloodscreen
        private float elapsedTimeBloodScreen = 0;

        private Button PauseButton, HelpButton;

        private Drawable scoreDisplay;

        private SoundEffectInstance instancesound;

        public List<Rectangle> blocks;

        public List<Rectangle> tile;
        private List<Projectile> projectiles;
        private List<Rectangle> objects = new List<Rectangle> { };
        private List<Bomb> bomb = new List<Bomb> { };
        private List<Rectangle> medecines = new List<Rectangle>();

        private List<Perso> iaPerso = new List<Perso>();

        private bool SoundIs;       // Set to true to switch the sound (on / off)
        private Drawable tree_autumn_exit;
        private Drawable tree_autumn_exit_inside;

        private Drawable debug;
        //private Drawable scoreDisplay;

        private List<Texture2D> texlis;
        private int mapSizeX;
        private int mapSizeY;

        private float timeElaspedGameOver = 0;
        //private bool playerActivate = true;
        private float transparencyAnimation = 0;
        public bool pause = false;
        public bool help = false;
        public bool endLevel = false;
        private float treeScale = 1;

        MouseState mouse = Mouse.GetState();

        private Language Language = new Language();
        public static ParticleComponent particleComponent;
        Random random;
        Emitter testEmitter2;
        bool snow = false;

        private gameState thegamestate;

        public List<Perso> persos;

        Texture2D Background, buche_texture, ground_texture;
        Drawable thetree, tree_entrance_inside, tree_entrance, tree_exit_inside, tree_exit, Ground;

        public MapState themapstate = MapState.game;

        private PauseMenu pauseMenu = new PauseMenu();
        Chat chat;

        public MapMulti(gameState thegamestate, ref Camera cameraClass, bool SoundIs)
        {
            chat = new Chat();
            this.thegamestate = thegamestate;
            this.NewGame(ref cameraClass);
            this.SoundIs = SoundIs;
            PauseButton = new Button("P / Pause", 524, 437, Textures.fontnormal_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));
            HelpButton = new Button("H / Help", 524 + 120, 437, Textures.fontnormal_texture, new Color(122, 184, 0), Color.White, new Color(122, 184, 0));

        }

        public void NewGame(ref Camera cameraClass)
        {
            Init_Game(ref cameraClass);
            int[,] thetile;
            int[] IAtile;
            themapstate = MapState.game;


            if (instancesound != null)
                instancesound.Stop();

            if (thegamestate == gameState.AutumnLevel)
            {
                instancesound = Textures.gameSound_EffectAutumn.CreateInstance();
                instancesound.IsLooped = true;
                snow = false;

            }
            else if (thegamestate == gameState.WinterLevel)
            {
                instancesound = Textures.gameSound_EffectWinter.CreateInstance();
                instancesound.IsLooped = true;
                snow = true;
            }
            else if (thegamestate == gameState.SpringLevel)
            {
                instancesound = Textures.gameSound_EffectSpring.CreateInstance();
                instancesound.IsLooped = true;
                snow = false;
            }
            else if (thegamestate == gameState.SummerLevel)
            {
                instancesound = Textures.gameSound_EffectSummer.CreateInstance();
                instancesound.IsLooped = true;
                snow = false;
            }

            if (SoundIs) instancesound.Play();



            // All the images and objects related to season levels are loaded here
            switch (thegamestate)
            {
                case gameState.AutumnLevel:
                    treeScale = 1;
                    ground_texture = Textures.autumn_ground_texture;
                    Background = Textures.autumnBackground;
                    thetile = autumnTileMap;
                    IAtile = autumnAiMap;
                    buche_texture = Textures.buche_texture;
                    thetree = new Drawable(drawable_type.tree);
                    tree_entrance = new Drawable(drawable_type.tree_autumn_entrance);
                    tree_entrance_inside = new Drawable(drawable_type.tree_autumn_entrance_inside);
                    tree_exit = new Drawable(drawable_type.tree_autumn_exit);
                    tree_exit_inside = new Drawable(drawable_type.tree_autumn_exit_inside);
                    Ground = new Drawable(drawable_type.AutumnGround);
                    break;
                case gameState.WinterLevel:
                    treeScale = 0.55f;
                    ground_texture = Textures.winter_ground_texture;
                    Background = Textures.winterBackground;
                    thetile = winterTileMap;
                    IAtile = winterAiMap;
                    buche_texture = Textures.buche_texture_winter;
                    thetree = new Drawable(drawable_type.winterTree);
                    tree_entrance = new Drawable(drawable_type.tree_winter_entrance);
                    tree_entrance_inside = new Drawable(drawable_type.tree_winter_entrance_inside);
                    tree_exit = new Drawable(drawable_type.tree_winter_exit);
                    tree_exit_inside = new Drawable(drawable_type.tree_winter_exit_inside);
                    Ground = new Drawable(drawable_type.WinterGround);
                    break;
                case gameState.SpringLevel:
                    treeScale = 0.55f;
                    ground_texture = Textures.spring_ground_texture;
                    Background = Textures.springBackground;
                    thetile = springTileMap;
                    IAtile = springAiMap;
                    buche_texture = Textures.buche_texture_spring;
                    thetree = new Drawable(drawable_type.springTreeHarmed);
                    tree_entrance = new Drawable(drawable_type.tree_spring_entrance);
                    tree_entrance_inside = new Drawable(drawable_type.tree_spring_entrance_inside);
                    tree_exit = new Drawable(drawable_type.tree_spring_exit);
                    tree_exit_inside = new Drawable(drawable_type.tree_spring_exit_inside);
                    Ground = new Drawable(drawable_type.SpringGround);
                    break;
                case gameState.SummerLevel:
                    treeScale = 0.55f;
                    ground_texture = Textures.summer_ground_texture;
                    Background = Textures.summerBackground;
                    thetile = summerTileMap;
                    IAtile = summerAiMap;
                    buche_texture = Textures.buche_texture_summer;
                    thetree = new Drawable(drawable_type.summerTreeHarmed);
                    tree_entrance = new Drawable(drawable_type.tree_summer_entrance);
                    tree_entrance_inside = new Drawable(drawable_type.tree_summer_entrance_inside);
                    Ground = new Drawable(drawable_type.SummerGround);
                    break;
                default: //Osef
                    ground_texture = Textures.winter_ground_texture;
                    Background = Textures.winterBackground;
                    thetile = winterTileMap;
                    IAtile = winterAiMap;
                    buche_texture = Textures.buche_texture_winter;
                    thetree = new Drawable(drawable_type.winterTree);
                    tree_entrance_inside = new Drawable(drawable_type.tree_winter_entrance_inside);
                    Ground = new Drawable(drawable_type.WinterGround);
                    break;
            }



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
                        medecines.Add(new Rectangle(x * Textures.buche_texture.Width + 50, ((y == mapSizeY - 1) ? 345 : y * Textures.buche_texture.Height - 73) - 7, Textures.medecine.Width, Textures.medecine.Height));

            persos = new List<Perso>();
            persos.Add(new Perso(new Vector2(150, 0), CharacType.player));
            persos.Add(new Perso(new Vector2(250, 0), CharacType.player));

            tree_autumn_exit = new Drawable(drawable_type.tree_autumn_exit);
            tree_autumn_exit_inside = new Drawable(drawable_type.tree_autumn_exit_inside);
            debug = new Drawable(drawable_type.font);
            scoreDisplay = new Drawable(drawable_type.font);

        }

        /* EVERYTHING THAT HAS TO BE INITIALIZED AT EACH LEVEL*/
        private void Init_Game(ref Camera cameraClass)
        {
            drawBloodScreen = false;
            cameraClass.shake = false;
            bomb = new List<Bomb>();
            random = new Random();
            medecines = new List<Rectangle>();


            if (thegamestate == gameState.WinterLevel || thegamestate == gameState.AutumnLevel)
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


            particleComponent.particleEmitterList.Add(
                new Emitter()
                {
                    Active = false,
                    TextureList = new List<Texture2D>() {
			      Textures.leaf_brown,
                  Textures.leaf_orange,
                  Textures.leaf_red,
                  Textures.leaf_yellow
			    },
                    RandomEmissionInterval = new RandomMinMax(150.0d),
                    ParticleLifeTime = 3500,
                    ParticleDirection = new RandomMinMax(170),
                    ParticleSpeed = new RandomMinMax(2.5f),
                    ParticleRotation = new RandomMinMax(0, 10),
                    RotationSpeed = new RandomMinMax(0.01f),
                    ParticleFader = new ParticleFader(false, true, 800),
                    ParticleScaler = new ParticleScaler(true, 0.1f)
                }
             );
        }

        public void Update(GameTime gametime, Game game, ref Camera cameraClass, ref Vector2 cameraPos, bool Developpermode)
        {
            // Pause and Help Buttons are created
            PauseButton.Update();
            HelpButton.Update();
            chat.Update(gametime);
            // Conditions to access the Pause Page
            if ((Inputs.isKeyRelease(Keys.Escape) || Inputs.isKeyRelease(Keys.P) || PauseButton.Clicked) && !Chat.text.isSelected) themapstate = MapState.pause;

            if (thegamestate == gameState.WinterLevel)
            {
                if (Inputs.isKeyRelease(Keys.S) && Developpermode) snow = !snow;
                if (snow) particleComponent.particleEmitterList[0].Active = !particleComponent.particleEmitterList[0].Active;
                particleComponent.particleEmitterList[1].Active = false;
                particleComponent.particleEmitterList[0].Active = true;
                Emitter t2 = particleComponent.particleEmitterList[0];
                t2.Position = new Vector2((float)random.NextDouble() * (Game1.graphics.GraphicsDevice.Viewport.Width), 0);
                if (t2.EmittedNewParticle)
                {
                    float f = MathHelper.ToRadians(t2.LastEmittedParticle.Direction + 180);
                    t2.LastEmittedParticle.Rotation = f;
                }
            }

            else if (thegamestate == gameState.AutumnLevel)
            {
                particleComponent.particleEmitterList[0].Active = false;
                particleComponent.particleEmitterList[1].Active = true;
                particleComponent.particleEmitterList[1].Position = new Vector2((float)random.NextDouble() * (Game1.graphics.GraphicsDevice.Viewport.Width), 0);
                if (particleComponent.particleEmitterList[1].EmittedNewParticle)
                {
                    float f = MathHelper.ToRadians(particleComponent.particleEmitterList[1].LastEmittedParticle.Direction + 180);
                    particleComponent.particleEmitterList[1].LastEmittedParticle.Rotation = f;
                }
            }

            else particleComponent.particleEmitterList[0].Active = particleComponent.particleEmitterList[1].Active = false;
            if (thegamestate == gameState.MainMenu && thegamestate == gameState.MainMenu)
                particleComponent.particleEmitterList[0].Active = particleComponent.particleEmitterList[1].Active = false;

            if (thegamestate != gameState.AutumnLevel && thegamestate != gameState.WinterLevel)
                particleComponent.particleEmitterList[0].Active = particleComponent.particleEmitterList[1].Active = false;

            bool justchange = false;
            if (themapstate == MapState.help && (Inputs.AnyKeyPressed() || Inputs.isLMBClick()))
            {
                themapstate = MapState.game;
                justchange = true;
            }

            // CHECK IF WE ARE AT THE END OF A LEVEL
            foreach (Perso p in persos)
            {
                if (p.positionPerso.X > 5350)
                {
                    themapstate = MapState.endlevel;
                    if (Inputs.isKeyRelease(Keys.Space))
                    {
                        endLevel = false;    // We begin a new level

                        // We switch to the next level
                        if (thegamestate == gameState.AutumnLevel) thegamestate = gameState.WinterLevel;
                        else if (thegamestate == gameState.WinterLevel) thegamestate = gameState.SpringLevel;
                        else if (thegamestate == gameState.SpringLevel) thegamestate = gameState.SummerLevel;
                        else thegamestate = gameState.AutumnLevel;

                        NewGame(ref cameraClass);
                    }
                }
                else if (themapstate == MapState.game)
                {
                    // Displays the Help page according to several conditions
                    if (((Inputs.isKeyRelease(Keys.H) || HelpButton.Clicked) && !justchange) && !Chat.text.isSelected)
                        themapstate = MapState.help;

                    if (Developpermode)
                        p.health = 20;

                    for (int j = medecines.Count - 1; j >= 0; j--)
                        if (p.hitBoxPerso.Intersects(medecines[j]))
                        {
                            medecines.Remove(medecines[j]);
                            p.health += (p.health <= 15) ? 5 : (20 - p.health);
                        }
                    int checkBlood = 0;
                    foreach (Perso iathings in iaPerso)
                    {
                        if (!p.gameover)
                            checkBlood += iathings.TryToKill(ref p.health, p.hitBoxPerso, SoundIs);
                        iathings.UpdateIA(gametime, blocks, p.hitBoxPerso, Developpermode);
                    }

                    bool touchedByBomb = false;

                    /* CHECK IF CHARACTER CROSS A MINE */
                    foreach (Bomb checkCrossed in bomb)
                    {
                        if (p.hitBoxPerso.Intersects(checkCrossed.Object) && !checkCrossed.activateExplosion)
                        {
                            touchedByBomb = true;
                            checkCrossed.activateExplosion = true;
                            drawBloodScreen = true;
                            if (checkCrossed.checkBlood && SoundIs)
                                Textures.gameExplosion_Effect.Play();
                            checkCrossed.BloodOnce(ref p.health);
                            break;
                        }
                        if (checkCrossed.activateExplosion)// important to keep the blood screen active until the end of the explosion
                            drawBloodScreen = true;
                    }

                    if (touchedByBomb)//fait clignoter le perso
                    {
                        p.PersoHitted = true;
                        p.compteurHitted = 0;
                    }

                    bomb.RemoveAll(x => x.checkIfFinish);//remove bomb when explosion animation is complete


                    if (!p.gameover)//player can move
                    {
                        if (p.utilisable)
                            cameraPos = p.cameraPos;

                        projectiles = new List<Projectile>();
                        p.Update(gametime, blocks, projectiles, objects, ref p.nbNuts, Developpermode);//TODO : remove keyboardState and oldkey because class now
                        this.objects = p.objects;
                        iaPerso = p.CollisionIAProjec(iaPerso, ref p.score);

                        if (checkBlood > 0)
                        {
                            p.PersoHitted = drawBloodScreen = true;
                            p.compteurHitted = 0;
                        }

                        if (p.health <= 0)
                        {
                            p.health = 0;
                            p.gameover = true;
                            drawBloodScreen = false;
                        }
                    }
                    else
                    {
                        int i = 0;
                        foreach (Perso p2 in persos)
                        {
                            if (!p2.gameover)
                                i++;
                        }
                        if (i == 0)
                        {
                            timeElaspedGameOver += gametime.ElapsedGameTime.Milliseconds;
                            if (timeElaspedGameOver > 2500)
                            {
                                timeElaspedGameOver = 0;
                                foreach (Perso p3 in persos)
                                    p3.gameover = false;
                                themapstate = MapState.gameover;
                            }
                            if (timeElaspedGameOver > 1500)
                                transparencyAnimation = (timeElaspedGameOver - 1500) / 1000;
                        }
                    }
                }
                else if (themapstate == MapState.pause)
                {
                    if (thegamestate == gameState.WinterLevel)
                        if (particleComponent.particleEmitterList[0].Active)
                            particleComponent.particleEmitterList[0].Active = false;
                    if (thegamestate == gameState.AutumnLevel)
                        if (particleComponent.particleEmitterList[1].Active)
                            particleComponent.particleEmitterList[1].Active = false;

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
                        if (particleComponent.particleEmitterList[1].Active && thegamestate == gameState.AutumnLevel)
                            particleComponent.particleEmitterList[1].Active = false;
                    }
                    if (Inputs.isKeyRelease(Keys.Space))
                    {
                        themapstate = MapState.game;
                        NewGame(ref cameraClass);
                    }
                }
            }
            Keys[] getkey = Keyboard.GetState().GetPressedKeys();

            if (Developpermode)
            {
                // Lorsque l'on appuie sur les touches 1, 2, 3, ou 4 on change de niveau
                if (getkey.Contains(Keys.NumPad2) || getkey.Contains(Keys.D2))
                {
                    // gerer les problemes de particules entre chgmts de niveaux
                    try
                    {
                        if (particleComponent.particleEmitterList[0].Active == true)
                            particleComponent.particleEmitterList[0].Active = false;
                    }
                    catch { }
                    thegamestate = gameState.WinterLevel;
                    NewGame(ref cameraClass);
                }
                if (getkey.Contains(Keys.NumPad1) || getkey.Contains(Keys.D1))
                {
                    try
                    {
                        if (particleComponent.particleEmitterList[0].Active == true)
                            particleComponent.particleEmitterList[0].Active = false;
                    }
                    catch { }
                    thegamestate = gameState.AutumnLevel;
                    NewGame(ref cameraClass);
                }
                if (getkey.Contains(Keys.NumPad3) || getkey.Contains(Keys.D3))
                {
                    try
                    {
                        if (particleComponent.particleEmitterList[0].Active == true)
                            particleComponent.particleEmitterList[0].Active = false;
                    }
                    catch { }
                    thegamestate = gameState.SpringLevel;
                    NewGame(ref cameraClass);
                }
                if (getkey.Contains(Keys.NumPad4) || getkey.Contains(Keys.D4))
                {
                    try
                    {
                        if (particleComponent.particleEmitterList[0].Active == true)
                            particleComponent.particleEmitterList[0].Active = false;
                    }
                    catch { }
                    thegamestate = gameState.SummerLevel;
                    NewGame(ref cameraClass);
                }
            }
        }

        public void Display(SpriteBatch sb, GameTime gameTime, Camera cameraClass)
        {
            /*sb.Begin();
            chat.Draw(sb);
            sb.End();*/
            foreach (Perso p in persos)
                if (themapstate == MapState.gameover && !p.gameover)
                    GameOverAnimation(sb, 1);
            if (themapstate == MapState.endlevel || themapstate == MapState.help)
            {
                sb.Begin();
                sb.Draw(Background, Vector2.Zero, Color.White);
                sb.Draw(ground_texture, new Vector2(0, 405), Color.White);
                sb.Draw(ground_texture, new Vector2(790, 405), Color.White);
                sb.Draw(Textures.hitbox, new Rectangle(0, 0, 1100, 550), Color.Black * 0.5f);
                sb.End();
            }

            // Displays the end level page
            if (themapstate == MapState.endlevel)
            {

                sb.Begin();
                scoreDisplay.Draw(sb, Language.Text_Game["congrats"], new Vector2(85, 35), Color.White, "42");
                for (int i = 0; i < persos.Count; i++)
                {
                    scoreDisplay.Draw(sb, Language.Text_Game["player"] + "  " + (i + 1), new Vector2(30 + Pow(i + 1, 5) + (150 * i), 120), Color.White, "multi");
                    scoreDisplay.Draw(sb, Language.Text_Game["finalScore"] + persos[i].score, new Vector2(30 + Pow(i + 1, 5) + (150 * i), 190), Color.White, "multi");
                    scoreDisplay.Draw(sb, Language.Text_Game["finalBonus"] + persos[i].nbNuts, new Vector2(30 + Pow(i + 1, 5) + (150 * i), 220), Color.White, "multi");
                    scoreDisplay.Draw(sb, Language.Text_Game["total"] + (persos[i].score + persos[i].nbNuts * 0.5), new Vector2(30 + Pow(i + 1, 5) + (150 * i), 280), Color.OrangeRed, "multi");
                    scoreDisplay.Draw(sb, Language.Text_Game["space"], new Vector2(70, 400), Color.White, "osef");
                }
                sb.End();


            }
            // Displays the Help page
            else if (themapstate == MapState.help)
            {
                sb.Begin();
                scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine1"], new Vector2(190, 100), Color.White, "help");
                scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine2"], new Vector2(190, 130), Color.White, "help");
                scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine3"], new Vector2(190, 160), Color.White, "help");
                scoreDisplay.Draw(sb, Language.Text_Game["_gameHelpLine4"], new Vector2(190, 200), Color.White, "help");
                sb.End();
            }
            else if (themapstate == MapState.pause)
                pauseMenu.Display(sb, thegamestate);
            else if (themapstate == MapState.game)
            {
                sb.Begin();
                // Makes the background move slower than the camera to create an effect of depth.
                sb.Draw(Background, new Vector2(cameraClass.Position.X / 3 - 1, -43), Color.White * 0.9f);
                sb.End();

                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);

                float scale = 1;
                // Apply what is below only if thetree = summerTree with gameState.SummerLevel
                //                             thetree = springTreeHarmed with gameState.SpringLevel 
                if (thegamestate == gameState.SpringLevel)
                {
                    scale = 0.55f;
                }
                tree_entrance.Draw(sb, new Vector2(-100, 10), treeScale);
                thetree.Draw(sb, new Vector2(500, 0), scale);
                thetree.Draw(sb, new Vector2(400, 0), scale);
                thetree.Draw(sb, new Vector2(900, 0), scale);
                thetree.Draw(sb, new Vector2(1050, 0), scale);
                thetree.Draw(sb, new Vector2(1400, 0), scale);
                thetree.Draw(sb, new Vector2(1800, 0), scale);
                thetree.Draw(sb, new Vector2(2200, 0), scale);
                thetree.Draw(sb, new Vector2(2400, 0), scale);
                thetree.Draw(sb, new Vector2(3000, 0), scale);
                thetree.Draw(sb, new Vector2(3400, 0), scale);
                thetree.Draw(sb, new Vector2(3900, 0), scale);
                thetree.Draw(sb, new Vector2(4050, 0), scale);
                thetree.Draw(sb, new Vector2(4900, 0), scale);
                if (thegamestate != gameState.SummerLevel) tree_exit.Draw(sb, new Vector2(5200, 10), 0.55f);


                // Draw ground image
                for (int truc = 0; truc < 10; truc++)
                    Ground.Draw(sb, new Vector2(truc * ground_texture.Width, 355));

                // Draw the platforms
                foreach (Rectangle top in blocks)
                    sb.Draw(buche_texture, top, Color.White);

                // Draw the objects
                foreach (Rectangle dessine in objects)
                    sb.Draw(Textures.acorn_texture, dessine, Color.White);

                //draw bomb
                foreach (Bomb dessine in bomb)
                    dessine.Draw(sb, gameTime);

                foreach (Rectangle dessine in medecines)
                    sb.Draw(Textures.medecine, dessine, Color.White);
                foreach (Perso p in persos)
                    if (!p.gameover)
                        p.Draw(sb, gameTime);
                //Draw all players


                // Draw IA characters
                foreach (Perso iathings in iaPerso)
                    iathings.Draw(sb, gameTime);

                //------------------------------------------------------------------
                // ES 15APR14
                // Draw foreground tree so that squirrel appears to enter the hole
                //------------------------------------------------------------------
                tree_entrance_inside.Draw(sb, new Vector2(-100, 10), treeScale);
                if (thegamestate != gameState.SummerLevel) tree_exit_inside.Draw(sb, new Vector2(5200, 10), 0.55f);

                chat.Draw(sb);

                sb.End();

                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraClass.TransformMatrix);
                Bloodscreen(gameTime, sb, cameraClass.Position, cameraClass);
                sb.End();
                sb.Begin();
                sb.Draw(Textures.hitbox, new Rectangle(0, 420, Game1.graphics.PreferredBackBufferWidth + 40, 120), Color.DimGray);//draw panel life + bonus + help + pause

                for (int i = 0; i < persos.Count; i++)
                {
                    scoreDisplay.Draw(sb, Language.Text_Game["_gamescore"] + " : " + persos[i].score, new Vector2(100 + Pow(i + 1, 5) + (20 * i), 487), Color.Black, "normal");
                    // this display the number of nuts that the perso has. 
                    scoreDisplay.Draw(sb, Language.Text_Game["_gamebonus"] + " : " + persos[i].nbNuts, new Vector2(Pow(i + 1, 5) + (20 * i), 487), Color.Black, "normal");

                    //draw text health
                    scoreDisplay.Draw(sb, Language.Text_Game["_gameHealth"] + " :  " + persos[i].health + "/20", new Vector2(25 + Pow(i + 1, 5) + (20 * i), 425), Color.Black, "normal");


                    //------------------------------------------------------------------
                    // ES 16JUI14
                    // Draws the Pause and Help Buttons on the Underbar
                    //------------------------------------------------------------------
                    PauseButton.Display(sb);
                    HelpButton.Display(sb);

                    //Negative health
                    sb.Draw(Textures.healthBar_texture, new Rectangle(0 + Pow(i + 1, 5) + (20 * i),
                        457, Textures.healthBar_texture.Width / 2, Textures.healthBar_texture.Height / 3), new Rectangle(0 + Pow(i + 1, 5) + (30 * i), 31,
                        Textures.healthBar_texture.Width, 28), Color.Gray);
                    //health left
                    sb.Draw(Textures.healthBar_texture, new Rectangle(0 + Pow(i + 1, 5) + (20 * i),
                        457, (int)(Textures.healthBar_texture.Width * (double)persos[i].health / 20f) / 2,
                        Textures.healthBar_texture.Height / 3), new Rectangle(0 + Pow(i + 1, 5) + (30 * i), 450 + Textures.healthBar_texture.Height / 2, Textures.healthBar_texture.Width, Textures.healthBar_texture.Height / 2), Color.Red);
                    //healthBar bounds
                    sb.Draw(Textures.healthBar_texture, new Rectangle(0 + Pow(i + 1, 5) + (20 * i),
                        457, Textures.healthBar_texture.Width / 2, Textures.healthBar_texture.Height / 3), new Rectangle(0, 0,
                        Textures.healthBar_texture.Width, 28), Color.White);
                }

                sb.End();
                foreach (Perso p in persos)
                    if (p.gameover && timeElaspedGameOver > 1500)
                        GameOverAnimation(sb, transparencyAnimation);
            }

        }

        private int Pow(int i, int n)
        {
            int pow = 1;
            for (int j = 0; j < n; j++)
            {
                pow += pow * i;
            }
            return pow;
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

        private void Bloodscreen(GameTime gameTime, SpriteBatch sb, Vector2 camera, Camera cameraClass)
        {
            if (drawBloodScreen)
            {
                elapsedTimeBloodScreen += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                cameraClass.shake = true;
                foreach (Perso p in persos)
                {
                    if (elapsedTimeBloodScreen < 50)
                    {
                        int positionX = (int)p.positionPerso.X - 400;
                        float correction = 0;
                        if (positionX + 400 >= 5000)
                            correction = 600;
                        sb.Draw(Textures.hitbox, new Rectangle(positionX - 20 - (int)correction, -20, 1800, 650), Color.Red * 0.5f);
                    }
                    else
                    {
                        elapsedTimeBloodScreen = 0;
                        drawBloodScreen = cameraClass.shake = false;
                    }
                }
            }
        }
    }
}