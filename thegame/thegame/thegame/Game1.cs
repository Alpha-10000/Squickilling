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

        Perso mario = new Perso();
        Plateforme plateforme = new Plateforme();
        Menu Menu = new Menu();

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
            mario.Initialize(0, 300);
            plateforme.Initialize(300, 200);
            frameRate = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mario.LoadContent(Content, "mario");
            plateforme.LoadContent(Content, "plateforme");
            _font = Content.Load<SpriteFont>("FPS");
         
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

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
                Startgame();
            }
            else
            {
                Menu_start();
            }
            base.Draw(gameTime);
        }

        private void Menu_start()
        {
            GraphicsDevice.Clear(Color.Beige);
        }

        private void Startgame()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            mario.Draw(spriteBatch);
            plateforme.Draw(spriteBatch);
            spriteBatch.DrawString(_font, "FPS : " + frameRate.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }


    }
}
