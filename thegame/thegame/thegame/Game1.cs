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
        Perso mario;
        Plateforme plateforme1;
        Menu Menu;

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

            Textures.load(Content);
            FrameRate = new Drawable(drawable_type.font);
            mario = new Perso(new Vector2(0, 300));
            mario.LoadContent(Content, "mario");
            plateforme1 = new Plateforme(drawable_type.Plateform_default ,new Vector2(300, 300));
            plateforme1.LoadContent(Content, "plateforme");
            _font = Content.Load<SpriteFont>("FPS");
            Menu =  new Menu();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Menu.MenuBool == false)
            {
                base.Draw(gameTime);
                mario.Update(gameTime);
                frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Menu.Update(gameTime);
            }
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            
            if (Menu.MenuBool == false)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();
                mario.Draw(spriteBatch);
                plateforme1.Draw(spriteBatch, new Vector2(300,300));
                FrameRate.Draw(spriteBatch, _font, "FPS : " + frameRate.ToString() , new Vector2(10, 10));
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Beige);
                spriteBatch.Begin();
                Menu.Draw(spriteBatch, _font, "Press bar space", new Vector2(200, 200));
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

   


    }
}
