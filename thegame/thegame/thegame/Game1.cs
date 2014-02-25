using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace thegame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float frameRate;
        private SpriteFont _font;
        public Drawable FrameRate;
        public  Instances instancesobject;
        private TimeSpan gameTimePause = TimeSpan.Zero;
        private TimeSpan realTimePause = TimeSpan.Zero;
        //Pause
        bool paused = false;
        Button btnPlay, btnQuit;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
          //  this.graphics.PreferredBackBufferHeight = 500;
          //  this.graphics.PreferredBackBufferWidth = 800;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            frameRate = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            btnPlay = new Button();
            btnQuit = new Button();
            btnQuit.Load(Content.Load<Texture2D>(@"Play"), new Vector2(350,275));
            btnPlay.Load(Content.Load<Texture2D>(@"Quit"), new Vector2(350, 225));

            Textures.load(Content);
            
            FrameRate = new Drawable(drawable_type.font);
            

            _font = Content.Load<SpriteFont>("FPS");
            instancesobject = new Instances(this);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            
            MouseState mouse = Mouse.GetState();
            
            if (!paused)
            {
                base.Draw(gameTime);
                instancesobject.UpdateByKey(gameTime);
                frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    paused = true;
                    btnPlay.isClicked = false;
                }

            }
            else if (paused)
            {
                if (btnPlay.isClicked)
                    paused = false;
                if (btnQuit.isClicked)
                    Exit();


                btnPlay.Update(mouse);
                btnQuit.Update(mouse);
            }
           
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            
            

                if (instancesobject.type == instances_type.Game)
                {
                    //GraphicsDevice.Clear(Color.ForestGreen);
                    spriteBatch.Begin();
                    spriteBatch.Draw(Textures.background, Vector2.Zero, Color.White*0.9f);
                    instancesobject.Display(spriteBatch);
                    FrameRate.Draw(spriteBatch, "FPS : " + frameRate.ToString(), new Vector2(600, 10), Color.Black, "normal");
                    spriteBatch.End();
                }
                else 
                {
                    GraphicsDevice.Clear(Color.Beige);
                    spriteBatch.Begin();
                    instancesobject.Display(spriteBatch);
                    spriteBatch.End();
                }
            
            spriteBatch.Begin();
            if (paused)
            {
                spriteBatch.Draw(Textures.pausedTexture, Textures.pausedRectangle, Color.White);
                btnPlay.Draw(spriteBatch);
                btnQuit.Draw(spriteBatch);
            }
            spriteBatch.End();


            
            base.Draw(gameTime);
        }

   


    }
}
